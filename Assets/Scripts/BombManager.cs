using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class BombManager : MonoBehaviour {
    [SerializeField] private float timer;
    private float initTime;
    [SerializeField] private GameObject explosion;
    private bool destroyBomb = false;
    [SerializeField] private float force;
	[SerializeField] private float radius;
    private GameManager gManager;
    private NavMeshSurface surface;
    private AudioSource aS;
    [SerializeField] private AudioClip wick;
    [SerializeField] private AudioClip explode;

    void Start() {
        initTime = Time.time;
        gManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        surface = GameObject.FindGameObjectWithTag("Surface").GetComponent<NavMeshSurface>();
        aS = this.gameObject.GetComponent<AudioSource>();
    }

    void Update() {
        if (Time.time - initTime > timer) {
            if (!destroyBomb) {
                destroyBomb = true;
                explodeBomb();
            }
        }

        if (!gManager.isPlayerAlive)
            Destroy(this.gameObject);
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
        
        aS.Stop();
        aS.clip = explode;
        aS.loop = false;
        aS.Play();
        PlayExplosionPS(this.gameObject.transform);

        for (int i = 0; i < gManager.numFire; i++) {
            Ray ray = new Ray();
            RaycastHit hitInfo;
            ray = new Ray(new Vector3(transform.position.x - (1.5f), transform.position.y - 1f, transform.position.z), Vector3.up);
            Debug.DrawRay(ray.origin, ray.direction * (1.5f), Color.green, 1f);
            if (Physics.Raycast(ray, out hitInfo, 1.5f)) {
                if (hitInfo.collider.CompareTag("Tile")) {
                    PlayExplosionPS(hitInfo.collider.transform);
                }
            }
            ray = new Ray(new Vector3(transform.position.x + (1.5f), transform.position.y - 1f, transform.position.z), Vector3.up);
            Debug.DrawRay(ray.origin, ray.direction * (1.5f), Color.green, 1f);
            if (Physics.Raycast(ray, out hitInfo, 1.5f)) {
                if (hitInfo.collider.CompareTag("Tile")) {
                    PlayExplosionPS(hitInfo.collider.transform);
                }
            }
            ray = new Ray(new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z - (1.5f)), Vector3.up);
            Debug.DrawRay(ray.origin, ray.direction * (1.5f), Color.green, 1f);
            if (Physics.Raycast(ray, out hitInfo, 1.5f)) {
                if (hitInfo.collider.CompareTag("Tile")) {
                    PlayExplosionPS(hitInfo.collider.transform);
                }
            }
            ray = new Ray(new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z + (1.5f)), Vector3.up);
            Debug.DrawRay(ray.origin, ray.direction * (1.5f), Color.green, 1f);
            if (Physics.Raycast(ray, out hitInfo, 1.5f)) {
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
        surface.BuildNavMesh();
        Destroy(bomb);
    }

    void CheckCollisions(Vector3 direction) {
        Ray ray1 = new Ray(new Vector3(transform.position.x - 0.75f, transform.position.y+0.5f, transform.position.z), direction);
        Ray ray2 = new Ray(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), direction);
        Ray ray3 = new Ray(new Vector3(transform.position.x + 0.75f, transform.position.y + 0.5f, transform.position.z), direction);
        Debug.DrawRay(ray1.origin, ray1.direction * (gManager.numFire*2), Color.red, 1f);
        Debug.DrawRay(ray2.origin, ray2.direction * (gManager.numFire * 2), Color.red, 1f);
        Debug.DrawRay(ray3.origin, ray3.direction * (gManager.numFire * 2), Color.red, 1f);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray1, out hitInfo, gManager.numFire*2) ||
            Physics.Raycast(ray2, out hitInfo, gManager.numFire * 2) ||
            Physics.Raycast(ray3, out hitInfo, gManager.numFire * 2)) {
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
            if (hitInfo.collider.CompareTag("Enemy")) {
                Debug.Log("Enemy");
                gManager.IncNumDeadEnemies();
                hitInfo.transform.gameObject.GetComponent<EnemyController>().PlayDead();

            }
            HandleCollision(hitInfo.collider.gameObject);
        }
    }

    void HandleCollision(GameObject other) {
        if (other.CompareTag("IconFire") || other.CompareTag("IconBomb") || other.CompareTag("IconKey")) {
            Destroy(other.gameObject);
        }
    }
}