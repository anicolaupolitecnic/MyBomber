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
            // Raycast in the up direction
            RaycastDirection(Vector3.up);

            // Raycast in the down direction
            RaycastDirection(Vector3.down);

            // Raycast in the left direction
            RaycastDirection(Vector3.left);

            // Raycast in the right direction
            RaycastDirection(Vector3.right);

            Destroy(this.gameObject);
        }
    }

    void RaycastDirection(Vector3 direction) {
        float rayLength = 2.0f;

        // Create a ray from the current position in the specified direction
        Ray ray = new Ray(transform.position, direction);

        // Perform the raycast
        if (Physics.Raycast(ray, out RaycastHit hitInfo, rayLength)) {
            // Collision detected, do something
            Debug.Log($"Collision detected in {direction} direction. Hit object: {hitInfo.collider.gameObject.name}");
        } else {
            // No collision detected
            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.green);
        }
    }
}
