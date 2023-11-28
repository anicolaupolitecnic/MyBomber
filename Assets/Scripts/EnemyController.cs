using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : MonoBehaviour {

    private NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject target;
    private Ray ray;
    RaycastHit hit;

    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        ray = new Ray();
    }

    void Update() {
        //navMeshAgent.destination = target.transform.position;
        Debug.DrawRay(ray.origin, ray.direction * 20, Color.red, Mathf.Infinity);
        if (Physics.Raycast(this.transform.position, this.transform.TransformDirection(Vector3.forward), out hit, 2f) && hit.transform.tag == "Wall") {
            Debug.Log("Wall front");
        } else {
            Debug.Log("Nothing front");
        }

        if (Physics.Raycast(this.transform.position, this.transform.TransformDirection(Vector3.back), out hit, 2f) && hit.transform.tag == "Wall") {
            Debug.Log("Wall back");
        } else {
            Debug.Log("Nothing back");
        }
    }
}
