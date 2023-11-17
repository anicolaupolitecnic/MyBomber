using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconManager : MonoBehaviour {
    [SerializeField] private float rotationSpeed;
    
    void Update() {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Player has entered the trigger zone!");
            Destroy(this.gameObject);
        }
    }
}
