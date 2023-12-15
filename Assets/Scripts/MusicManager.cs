using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    [SerializeField] private List<AudioClip> music;
    [SerializeField]private AudioSource aS1;
    [SerializeField] private AudioSource aS2;
    private GameManager gManager;

    void Start() {
        gManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        aS1.clip = music[0];
        aS1.Play();
    }

    private void Update() {
        if (gManager.prevState != gManager.state) {
            gManager.prevState = gManager.state;
            if (gManager.state == 0) {
                aS1.clip = music[0];
            } else {
                aS1.clip = music[1];
                PlayNextAmbientSound();
            }
            aS1.Play();
        }
    }

    void PlayNextAmbientSound() {
        aS2.clip = music[Random.Range(2, music.Count)];
        aS2.Play();
        Invoke("PlayNextSong", GetComponent<AudioSource>().clip.length);
    }
}
