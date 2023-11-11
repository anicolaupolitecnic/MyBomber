using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    //public GameObject tilePlayer;
    public GameObject pointer;
    private Vector3 difference;

    private CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f * 2f;

    Vector3 velocity;
    bool isMoving;
    bool isPlayerAlive;
    private Vector3 lastPosition = new Vector3(0f,0f,0f);


    void Start() {
        isPlayerAlive = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().isPlayerAlive;
        controller = GetComponent<CharacterController>();
    }

    void Update() {
        if (isPlayerAlive) {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

            if (lastPosition != this.gameObject.transform.position) {
                isMoving = true;
            } else {
                isMoving = false;
            }

            Vector3 offset = new Vector3(0f, 0f, 2f);
            Vector3 desiredPosition = this.gameObject.transform.TransformPoint(offset);

            // Set the position of this object to the desired position
            pointer.transform.position = new Vector3(desiredPosition.x, pointer.transform.position.y, desiredPosition.z);
        }
    }
}
