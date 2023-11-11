using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour {
    [SerializeField] private float timer;
    private float initTime;

    void Start() {
        initTime = Time.time;
    }

    void Update() {
        if (Time.time - initTime > timer) {
            RaycastDirection(Vector3.forward);
            RaycastDirection(Vector3.back);
            RaycastDirection(Vector3.left);
            RaycastDirection(Vector3.right);
            Destroy(this.gameObject);
        }
    }

    void RaycastDirection(Vector3 direction) {
        float rayLength = 2.0f;

        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y+1f, transform.position.z), direction);
        Debug.DrawRay(ray.origin, ray.direction * 5f, Color.red, 1f);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, rayLength)) {
            if (hitInfo.collider.CompareTag("Block")) {
                Destroy(hitInfo.collider.gameObject);
                //hitInfo.collider.gameObject.GetComponent<Explode>().DestroyCube();
            }
            if (hitInfo.collider.CompareTag("Player")) {
                Debug.Log("YOU ARE DEAD");
                //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().isPlayerAlive = false;
                //Invoke("ReloadScene", 2f);
            }
        }
    }
}
