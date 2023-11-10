using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    //public GameObject tilePlayer;
    private CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f * 2f;

    Vector3 velocity;
    bool isMoving;

    private Vector3 lastPosition = new Vector3(0f,0f,0f);


    void Start() {
        controller = GetComponent<CharacterController>();
    }

    void Update() {
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
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Tile")) {
            //Debug.Log("TilePlayer aaa");
            //tilePlayer = collision.gameObject;
        }
    }
}
