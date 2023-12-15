using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IconManager : MonoBehaviour {
    private GameManager gManager;
    [SerializeField] private float rotationSpeed;
    private AudioSource aS;
    [SerializeField] private AudioClip pickItem;

    void Start() {
        gManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        aS = GameObject.FindGameObjectWithTag("FX_AudioSource").GetComponent<AudioSource>();
    }

    void Update() {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            aS.Stop();
            aS.clip = pickItem;
            aS.loop = false;
            aS.Play();
            if (this.gameObject.transform.CompareTag("IconFire"))
                gManager.IncNumFire();
            if (this.gameObject.transform.CompareTag("IconBomb"))
                gManager.IncNumBombs();
            if (this.gameObject.transform.CompareTag("IconKey"))
                gManager.EnableDoor();
            Destroy(this.gameObject);
        }
    }
}
