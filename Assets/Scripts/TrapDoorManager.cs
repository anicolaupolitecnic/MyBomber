using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoorManager : MonoBehaviour {
    private GameManager gManager;
    private AudioSource aS;
    [SerializeField] private AudioClip openDoor;

    private void Awake() {
        aS = this.gameObject.transform.GetComponent<AudioSource>();
    }

    void Start() {
        gManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnEnable() {
        aS.Stop();
        aS.clip = openDoor;
        aS.loop = false;
        aS.Play();
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (gManager.isIconKey)
                gManager.LevelClear();
        }
    }
}
