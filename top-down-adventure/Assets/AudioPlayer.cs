using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource[] allAudioSources;

    [Header("SFX")]
    [SerializeField] AudioClip playerAttackSFX1;
    [SerializeField] AudioClip playerAttackSFX2;
    [SerializeField] AudioClip playerGetsHitSFX;
    [SerializeField] AudioClip slimeAttacksSFX;
    [SerializeField] AudioClip slimeGetsHitSFX;
    [SerializeField] AudioClip grassGetsHitSFX;
    [SerializeField] AudioClip rupeePickupSFX;

    [Header("BG Music")]
    [SerializeField] AudioClip backgroundMusic1;
    [SerializeField] AudioClip backgroundMusic2;
    
    [Header("Audio Player")]
    private static AudioPlayer instance = null;
    public  static AudioPlayer Instance{get {return instance;}}

    void Awake(){
        if( instance != null && instance != this){
            Destroy(this.gameObject);
            return;
        }else{
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start(){
        PlayBGMusic(backgroundMusic1);
    }

    private void PlayClip(AudioClip clip){
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, 0.1f);
    }

    private void StopAllAudio() {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach( AudioSource audioS in allAudioSources) {
            audioS.Stop();
        }
    }

    public void PlayBGMusic(AudioClip audioClip){
        StopAllCoroutines();
        StopAllAudio();
        StartCoroutine("PlayMusicInLoop", audioClip);
    }

    IEnumerator PlayMusicInLoop(AudioClip music){
        AudioSource.PlayClipAtPoint(music, Camera.main.transform.position, 0.3f);
        yield return new WaitForSeconds(music.length);
        StartCoroutine("WaitForMusicToEnd");
    }

    public void PlayBattleSong(){
        PlayBGMusic(backgroundMusic2);
    }

    public void PlayThemeSong(){
        PlayBGMusic(backgroundMusic1);
    }

    public void RupeePickup(){PlayClip(rupeePickupSFX);}

    public void GrassGetsHit(){PlayClip(grassGetsHitSFX);}

    public void SlimeGetsHit(){PlayClip(slimeGetsHitSFX);}

    public void SlimeAttacks(){PlayClip(slimeAttacksSFX);}

    public void PlayerGetsHit(){PlayClip(playerGetsHitSFX);}

    public void PlayerAttacks(){
        if(Random.Range(0,2) == 0){
            PlayClip(playerAttackSFX1);
        }else{
            PlayClip(playerAttackSFX2);
        }
    } 

    
}
