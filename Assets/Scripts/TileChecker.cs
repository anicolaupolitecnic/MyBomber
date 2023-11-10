using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChecker : MonoBehaviour {
    [SerializeField] private GameObject bomb;
    private GameObject tileTarget;
    GameObject newBomb;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (tileTarget.transform.childCount == 0) {
                if (GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<PlayerTileChecker>().tilePlayer != tileTarget) {
                    Debug.Log("playerTile: " + GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<PlayerTileChecker>().tilePlayer);
                    Debug.Log("tileTarget: " + tileTarget.gameObject.name);
                    newBomb = Instantiate(bomb, tileTarget.transform.position, tileTarget.transform.rotation);
                    newBomb.GetComponent<Collider>().enabled = true;
                    newBomb.transform.SetParent(tileTarget.transform);
                    StartCoroutine(DestroyBomb(newBomb, 5f));
                }
            }
        }
    }

    IEnumerator DestroyBomb(GameObject bomb, float time) {
        yield return new WaitForSeconds(time);
        Destroy(bomb);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Tile")) {
            tileTarget = collision.gameObject;
        }
    }
}
