using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileChecker : MonoBehaviour {
    private void OnCollisionEnter(Collision collision) {
        Debug.Log("aaa");
        if (collision.gameObject.CompareTag("Tile")) {
            Debug.Log("Hit: " + collision.gameObject.name + "!");
        }
    }
}
