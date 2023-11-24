using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    [SerializeField] private List<AudioClip> music;
    [SerializeField]private AudioSource aS;

    void Start() {
        PlayNextSong();
    }

    void Update() {
        
    }

    void PlayNextSong() {
        GetComponent<AudioSource>().clip = music[Random.Range(0, music.Count)];
        GetComponent<AudioSource>().Play();
        Invoke("PlayNextSong", GetComponent<AudioSource>().clip.length);
    }
}