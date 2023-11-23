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
        CheckCollisions(Vector3.forward);
        CheckCollisions(Vector3.back);
        CheckCollisions(Vector3.left);
        CheckCollisions(Vector3.right);
        CheckCollisions(Vector3.down);
        PlayExplosionPS(this.gameObject.transform);

        for (int i = 0; i < gManager.numFire; i++) {
            Ray ray = new Ray();
            RaycastHit hitInfo;
            ray = new Ray(new Vector3(transform.position.x - (2f* gManager.numFire), transform.position.y - 1f, transform.position.z), Vector3.up);
            Debug.DrawRay(ray.origin, ray.direction * (gManager.numFire * 2), Color.red, 1f);
            if (Physics.Raycast(ray, out hitInfo, gManager.numFire * 2)) {
                if (hitInfo.collider.CompareTag("Tile")) {
                    PlayExplosionPS(hitInfo.collider.transform);
                }
            }
            ray = new Ray(new Vector3(transform.position.x + (2f * gManager.numFire), transform.position.y - 1f, transform.position.z), Vector3.up);
            Debug.DrawRay(ray.origin, ray.direction * (gManager.numFire * 2), Color.red, 1f);
            if (Physics.Raycast(ray, out hitInfo, gManager.numFire * 2)) {
                if (hitInfo.collider.CompareTag("Tile")) {
                    PlayExplosionPS(hitInfo.collider.transform);
                }
            }
            ray = new Ray(new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z - (2f * gManager.numFire)), Vector3.up);
            Debug.DrawRay(ray.origin, ray.direction * (gManager.numFire * 2), Color.red, 1f);
            if (Physics.Raycast(ray, out hitInfo, gManager.numFire * 2)) {
                if (hitInfo.collider.CompareTag("Tile")) {
                    PlayExplosionPS(hitInfo.collider.transform);
                }
            }
            ray = new Ray(new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z + (2f * gManager.numFire)), Vector3.up);
            Debug.DrawRay(ray.origin, ray.direction * (gManager.numFire * 2), Color.red, 1f);
            if (Physics.Raycast(ray, out hitInfo, gManager.numFire * 2)) {
                if (hitInfo.collider.CompareTag("Tile")) {
                    PlayExplosionPS(hitInfo.collider.transform);
                }
            }
        }
        StartCoroutine(DestroyBomb(this.gameObject, 5f));
    }

    void PlayExplosionPS(Transform t) {
        GameObject xplosion = Instantiate(explosion, t.position, t.rotation);
        xplosion.SetActive(true);
        xplosion.GetComponent<ParticleSystem>().Stop();
        xplosion.GetComponent<ParticleSystem>().Clear();
        xplosion.GetComponent<ParticleSystem>().Play();
        StartCoroutine(DestroyBomb(xplosion.gameObject, 5f));
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

    void CheckCollisions(Vector3 direction) {
        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), direction);
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
        if (other.CompareTag("IconFire") || other.CompareTag("IconBomb")) {
            Destroy(other.gameObject);
        }
    }
}