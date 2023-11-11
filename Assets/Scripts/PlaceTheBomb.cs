using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceTheBomb : MonoBehaviour {
    [SerializeField] private GameObject bomb;
    //private GameObject tileTarget;
    GameObject newBomb;

    void Update() {
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, 5f, this.gameObject.transform.position.z);
    
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = new Ray(this.gameObject.transform.position, Vector3.down);
            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 1f);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.CompareTag("Block")) {
                    Debug.Log("BLOCK");
                } else if (hit.collider.CompareTag("Tile")) {
                    newBomb = Instantiate(bomb, hit.collider.gameObject.transform.position, hit.collider.gameObject.transform.rotation);
                    newBomb.GetComponent<Collider>().enabled = true;
                    newBomb.transform.SetParent(hit.collider.gameObject.transform);    
                }
            }
/*
            if (tileTarget.transform.childCount == 0) {
                newBomb = Instantiate(bomb, tileTarget.transform.position, tileTarget.transform.rotation);
                newBomb.GetComponent<Collider>().enabled = true;
                newBomb.transform.SetParent(tileTarget.transform);
                //StartCoroutine(DestroyBomb(newBomb, 5f));
            }
*/
        }
    }
/*
    IEnumerator DestroyBomb(GameObject bomb, float time) {
        yield return new WaitForSeconds(time);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Tile")) {
            tileTarget = collision.gameObject;
            //Debug.Log("Tile: " + collision.gameObject.name);
        }
    }
*/
}
