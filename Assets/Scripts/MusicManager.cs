using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    [SerializeField]private AudioSource aSMusic1;
    [SerializeField] private AudioSource aSMusic2;
    [SerializeField] private AudioSource aSFX;
    [SerializeField] private List<AudioClip> music;

    private GameManager gManager;

    void Start() {
        gManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        aSMusic1.clip = music[0];
        aSMusic1.Play();
    }

    private void Update() {
        if (gManager.prevState != gManager.state) {
            gManager.prevState = gManager.state;
            if (gManager.state == 0) {
                aSMusic1.clip = music[0];
            } else {
                aSMusic1.clip = music[1];
                PlayNextAmbientSound();
            }
            aSMusic1.Play();
        }
    }

    void PlayNextAmbientSound() {
        aSMusic2.clip = music[Random.Range(2, music.Count)];
        aSMusic2.Play();
        Invoke("PlayNextSong", aSMusic1.clip.length);
    }
}
