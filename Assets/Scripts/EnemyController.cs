using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    private NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject target;

    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        navMeshAgent.destination = target.transform.position;
    }
}
