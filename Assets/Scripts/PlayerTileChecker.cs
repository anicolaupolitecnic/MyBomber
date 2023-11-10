using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTileChecker : MonoBehaviour {
    public GameObject tilePlayer;
    Vector3 playerPos;
    Quaternion playerRot;

    void Start() {
        playerPos = this.gameObject.transform.position;
        playerRot = this.gameObject.transform.rotation;
    }

    void Update() {
        this.gameObject.transform.rotation = playerRot;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Tile")) {
            tilePlayer = collision.gameObject;
        }
    }

}
