using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour {
    [SerializeField] private float timer;
    private float initTime;
    [SerializeField] private GameObject explosion;
    private bool destroyBomb = false;
    [SerializeField] private float force;
	[SerializeField] private float radius;
    private GameManager gManager;

    void Start() {
        initTime = Time.time;
        gManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();   
    }

    void Update() {
        if (Time.time - initTime > timer) {
            if (!destroyBomb) {
                destroyBomb = true;
                explodeBomb();
            }
        }
    }

    void explodeBomb() {
        gManager.DecNumBombsThrown();
        DisableAllChildrenRecursive(this.gameObject.transform);
        this.gameObject.GetComponent<Collider>().enabled = false;
        destroyBomb = true;
        RaycastDirection(Vector3.forward);
        RaycastDirection(Vector3.back);
        RaycastDirection(Vector3.left);
        RaycastDirection(Vector3.right);
        explosion.SetActive(true);
        explosion.GetComponent<ParticleSystem>().Stop();
        explosion.GetComponent<ParticleSystem>().Clear();
        explosion.GetComponent<ParticleSystem>().Play();
        StartCoroutine(DestroyBomb(this.gameObject, 5f));
    }

    void DisableAllChildrenRecursive(Transform parent) {
        foreach (Transform child in parent) {
            child.gameObject.SetActive(false);
            DisableAllChildrenRecursive(child.transform);
        }
    }

    IEnumerator DestroyBomb(GameObject bomb, float time) {
        yield return new WaitForSeconds(time);
        Destroy(bomb);
    }

    void RaycastDirection(Vector3 direction) {
        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y+1f, transform.position.z), direction);
        Debug.DrawRay(ray.origin, ray.direction * (gManager.numFire*2), Color.red, 1f);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, gManager.numFire*2)) {
            if (hitInfo.collider.CompareTag("Block")) {
                hitInfo.collider.gameObject.GetComponent<Explode>().DestroyCube();
            }
            if (hitInfo.collider.CompareTag("Player")) {
                gManager.RespawnPlayer();
            }
            if (hitInfo.collider.CompareTag("Bomb")) {
                Debug.Log("bomba");
                hitInfo.collider.gameObject.GetComponent<BombManager>().explodeBomb();
            }
            HandleCollision(hitInfo.collider.gameObject);
        }
    }

    void HandleCollision(GameObject other) {
        if (other.CompareTag("Icon")) {
            Destroy(other.gameObject);
        }
    }
}