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

    void Start() {
        initTime = Time.time;
    }

    void Update() {
        if (Time.time - initTime > timer) {
            if (!destroyBomb) {
                destroyBomb = true;
                RaycastDirection(Vector3.forward);
                RaycastDirection(Vector3.back);
                RaycastDirection(Vector3.left);
                RaycastDirection(Vector3.right);
                DisableAllChildrenRecursive(this.gameObject.transform);
                explosion.SetActive(true);
                explosion.GetComponent<ParticleSystem>().Stop();
                explosion.GetComponent<ParticleSystem>().Clear();
                explosion.GetComponent<ParticleSystem>().Play();
                //this.gameObject.GetComponent<Rigidbody>().AddExplosionForce(force, this.gameObject.transform.position, radius);
                this.gameObject.GetComponent<Collider>().enabled = false;
                StartCoroutine(DestroyBomb(this.gameObject, 5f));
            }
        }
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
        float rayLength = 2.0f;

        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y+1f, transform.position.z), direction);
        Debug.DrawRay(ray.origin, ray.direction * 5f, Color.red, 1f);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, rayLength)) {
            if (hitInfo.collider.CompareTag("Block")) {
                hitInfo.collider.gameObject.GetComponent<Explode>().DestroyCube();
            }
            if (hitInfo.collider.CompareTag("Player")) {
                StartCoroutine(ReloadSceneAfterTime(2f));
            }
        }
    }

    private IEnumerator ReloadSceneAfterTime(float delay) {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().isPlayerAlive = false;
        yield return new WaitForSeconds(delay);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().ReloadScene(); 
    } 
}