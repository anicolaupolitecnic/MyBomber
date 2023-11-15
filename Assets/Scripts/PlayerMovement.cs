using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    PlayerControls controls;
    Vector2 move;
    Vector2 rotate;

    public GameObject pointer;
    private Vector3 difference;

    private CharacterController controller;
    [SerializeField] private float speed;
    public float gravity = -9.81f * 2f;

    Vector3 velocity;
    bool isMoving;
    bool isPlayerAlive;
    private Vector3 lastPosition = new Vector3(0f,0f,0f);

    void Awake() {
        controls = new PlayerControls();
        controls.Gameplay.Action.performed += ctx => Action();
       
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;

        controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;
    }

    void Action() {

    }

    void OnEnable() {
        controls.Gameplay.Enable();
    }

    void OnDisable() {
        controls.Gameplay.Disable();
    }

    void Start() {
        isPlayerAlive = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().isPlayerAlive;
        controller = GetComponent<CharacterController>();
    }

    void Update() {
        if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().isPlayerAlive) {
            //float x = Input.GetAxis("Horizontal");
            //float z = Input.GetAxis("Vertical");

            Vector3 m = new Vector3(move.x, 0, move.y);//transform.right * x + transform.forward * z;
            
            controller.Move(m * speed * Time.deltaTime);
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            Vector2 r = new Vector2(-rotate.y, -rotate.x) * 100f * Time.deltaTime;

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
