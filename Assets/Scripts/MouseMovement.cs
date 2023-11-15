using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour {
    //public GameObject cam;
    public float mouseSensitivity;
    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;
    bool isPlayerAlive;
    
    void Start() {
        isPlayerAlive = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().isPlayerAlive;
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update() {
        if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().isPlayerAlive) {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

            yRotation += mouseX;
            this.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }
}
