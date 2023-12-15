using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    [SerializeField] private MenuPauseController menuPauseController;
    PlayerControls controls;
    GameManager gManager;
    Vector2 move;

    public GameObject pointer;
    [SerializeField] private float pointerDistance;
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

    private Image panelPlayerDie;

    private AudioSource aS;
    [SerializeField] private AudioClip playerStep;

    void Awake() {
        panelPlayerDie = GameObject.FindGameObjectWithTag("PanelPlayerDie").GetComponent<Image>();
        aS= GameObject.FindGameObjectWithTag("FX_AudioSource").GetComponent<AudioSource>();

        controls = new PlayerControls();
        controls.Gameplay.Action.performed += ctx => Action();

        controls.Gameplay.Pause.performed += ctx => Pause();

        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;

        controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;
    }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        gManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Action() {
        if (gManager.isPlayerAlive) {
            Vector3 v = pointer.gameObject.transform.position;
            v = new Vector3(v.x,v.y+4f, v.z);
            Ray ray = new Ray(v, Vector3.down);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.blue, 1f);

            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.CompareTag("Block") || hit.collider.CompareTag("Wall") || HandleCollision(hit.collider.gameObject)) {
                    Debug.Log("BLOCKED");
                } else if (hit.collider.CompareTag("Tile")) {
                    if (hit.collider.gameObject.transform.childCount == 0) {
                        if (gManager.numBombsThrown < gManager.numBombs) {
                            Debug.Log("TILE");
                            gManager.IncNumBombsThrown();
                            Vector3 v1 = new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y+0.5f, hit.collider.gameObject.transform.position.z);
                            newBomb = Instantiate(bomb, v1, hit.collider.gameObject.transform.rotation);
                            newBomb.GetComponent<Collider>().enabled = true;
                            newBomb.transform.SetParent(hit.collider.gameObject.transform);

                        }
                    }
                }
            }
        }
    }

    void Pause() {
        menuPauseController.Pause();
    }

    bool HandleCollision(GameObject other) {
        if (other.CompareTag("IconFire") || other.CompareTag("IconBomb")) {
            return true;
        }
        return false;  
    }

    void OnEnable() {
        controls.Gameplay.Enable();
    }

    void OnDisable() {
        controls.Gameplay.Disable();
    }

    Vector3 v = new Vector3 (0, 0, 0);
    void Update() {
        if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().isPlayerAlive) {
            //FadeIn panel
            if (panelPlayerDie.color.a > 0) {
                Color c = panelPlayerDie.color;
                c.a = 0;
                panelPlayerDie.color = c;
            }

            //MOVIMENT
            //Vector3 m = new Vector3(move.x * transform.forward.x, 0, move.y * transform.forward.z);
            Vector3 m = transform.forward * move.y + transform.right * move.x;
            controller.Move(m.normalized * speed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            if (lastPosition != this.gameObject.transform.position) { 
                isMoving = true;
                lastPosition = transform.position;
            } else 
                isMoving = false;
        
            if (isMoving) {
                if (!aS.isPlaying) { 
                    aS.Stop();
                    aS.clip = playerStep;
                    aS.loop = false;
                    aS.Play();
                }
            } else {
                aS.Stop();
            }

            Vector3 offset = new Vector3(0f, 0f, pointerDistance);
            Vector3 desiredPosition = this.gameObject.transform.TransformPoint(offset);

            pointer.transform.position = new Vector3(desiredPosition.x, pointer.transform.position.y, desiredPosition.z);

            //CAMERA
            float mouseX = (rotate.y) * mouseSensitivity * Time.deltaTime;
            float mouseY = (rotate.x) * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseX;
            xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            yRotation += mouseY;
            transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        } else {
            if (gManager.numLives > 0)
                if (panelPlayerDie.color.a < 255) {
                    Color c = panelPlayerDie.color;
                    c.a += 5f * Time.deltaTime;
                    panelPlayerDie.color = c;
                }
        }
    }
}
