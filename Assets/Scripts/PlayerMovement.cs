using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    PlayerControls controls;
    Vector2 move;

    public GameObject pointer;
    [SerializeField] private GameObject bomb;
    [SerializeField] private GameObject cam;
    GameObject newBomb;

    private CharacterController controller;
    [SerializeField] private float speed;
    public float gravity = -9.81f * 2f;

    Vector3 velocity;
    bool isMoving;
    private Vector3 lastPosition = new Vector3(0f,0f,0f);

    Vector2 rotate;
    public float mouseSensitivity;
    float xRotation = 0f;
    float yRotation = 0f;

    [SerializeField] private float topClamp;
    [SerializeField] private float bottomClamp;

    void Awake() {
        controls = new PlayerControls();
        controls.Gameplay.Action.performed += ctx => Action();
       
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;

        controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;
    }

    void Action() {
        Ray ray = new Ray(pointer.gameObject.transform.position, Vector3.down);
        //Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 1f);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.CompareTag("Block")) {
                Debug.Log("BLOCK");
            } else if (hit.collider.CompareTag("Tile")) {
                newBomb = Instantiate(bomb, hit.collider.gameObject.transform.position, hit.collider.gameObject.transform.rotation);
                newBomb.GetComponent<Collider>().enabled = true;
                newBomb.transform.SetParent(hit.collider.gameObject.transform);    
            }
        }
    }

    void OnEnable() {
        controls.Gameplay.Enable();
    }

    void OnDisable() {
        controls.Gameplay.Disable();
    }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
    }

    void Update() {
        if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().isPlayerAlive) {
            //MOVIMENT
            Vector3 m = new Vector3(move.x, 0, move.y);
            controller.Move(m * speed * Time.deltaTime);
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            if (lastPosition != this.gameObject.transform.position) {
                isMoving = true;
            } else {
                isMoving = false;
            }

            Vector3 offset = new Vector3(0f, 0f, 2f);
            Vector3 desiredPosition = this.gameObject.transform.TransformPoint(offset);

            pointer.transform.position = new Vector3(desiredPosition.x, pointer.transform.position.y, desiredPosition.z);

            //CAMERA
            float mouseX = (rotate.y) * mouseSensitivity * Time.deltaTime;
            float mouseY = (rotate.x) * mouseSensitivity * Time.deltaTime;
            
            xRotation -= mouseX;
            xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

            yRotation += mouseY;

            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            //transform.Rotate(0f, yRotation, 0f);
            transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        }
    }
}
