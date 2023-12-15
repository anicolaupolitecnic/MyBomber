using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeveLManager : MonoBehaviour {
    [SerializeField] public GameObject spawnPoint;
    void Start() {
        spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
