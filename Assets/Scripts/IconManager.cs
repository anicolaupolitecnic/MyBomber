using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconManager : MonoBehaviour {
    private GameManager gManager;
    [SerializeField] private float rotationSpeed;
    
    void Start() {
        gManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update() {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (this.gameObject.transform.CompareTag("IconFire"))
                gManager.IncNumFire();
            if (this.gameObject.transform.CompareTag("IconBomb"))
                gManager.IncNumBombs();
            Destroy(this.gameObject);
        }
    }
}
