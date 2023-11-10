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

    private Vector3 lastPosition = new Vector3(0f,0f,0f);


    void Start() {
        controller = GetComponent<CharacterController>();
        //difference = this.gameObject.transform.position - pointer.gameObject.transform.position;
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

        Vector3 offset = new Vector3(0f, 0f, 2f);
        Vector3 desiredPosition = this.gameObject.transform.TransformPoint(offset);

        // Set the position of this object to the desired position
        pointer.transform.position = new Vector3(desiredPosition.x, pointer.transform.position.y, desiredPosition.z);

        // Rotate this object to match the rotation of the reference object
        //pointer.transform.rotation = Quaternion.Euler(this.gameObject.transform.rotation.eulerAngles.x, 0f, 0f);
        //pointer.transform.position = this.transform.position + difference;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Tile")) {
            //Debug.Log("TilePlayer aaa");
            //tilePlayer = collision.gameObject;
        }
    }
}
