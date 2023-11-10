using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTileChecker : MonoBehaviour {
    public GameObject tilePlayer;

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Tile")) {
            tilePlayer = collision.gameObject;
        }
    }

}
