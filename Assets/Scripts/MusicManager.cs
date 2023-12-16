using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    [SerializeField]private AudioSource aSMusic1;
    [SerializeField] private AudioSource aSMusic2;
    [SerializeField] public AudioSource aSFX;
    [SerializeField] private List<AudioClip> music;

    [SerializeField] public AudioClip gameOver;
    [SerializeField] public AudioClip gameClear;
    
    [SerializeField] public AudioClip playerDie;
    [SerializeField] public AudioClip playerSpawn;
    [SerializeField] public AudioClip playerStep;

    [SerializeField] public AudioClip wick;
    [SerializeField] public AudioClip explode;

    private GameManager gManager;

    void Start() {
        MainMenuMusic();
    }

    public void MainMenuMusic() {
        aSMusic1.clip = music[0];
        aSMusic1.Play();
    }

    public void GameMusic() {
        aSMusic1.clip = music[1];
        PlayNextAmbientSound();
        aSMusic1.Play();
    }

    void PlayNextAmbientSound() {
        aSMusic2.clip = music[Random.Range(2, music.Count)];
        aSMusic2.Play();
        Invoke("PlayNextSong", aSMusic1.clip.length);
    }

    public void PlayFX(AudioClip fx) { 
        aSFX.clip = fx; 
    }

}
