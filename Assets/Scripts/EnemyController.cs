using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : MonoBehaviour {
    [SerializeField] private float speed;
    private Animator animator;
    private const int STIDLE = 1;
    private const int STWALK = 2;
    private const int STATTACK = 3;
    private const int STDIE = 4;
    private int state;
    private int prevState;

    private bool checkForCollisions = false;
    private bool isChecked = false;
    private NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject target;
    private Rigidbody rb;
    private Ray ray;
    RaycastHit hit;

    void Start() {
        //navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        state = STIDLE;
        prevState = STIDLE;
        CheckFrontCollisions();
    }

    void Update() {
        //navMeshAgent.destination = target.transform.position;
        
        if (isChecked && checkForCollisions) {
            CheckLeftCollisions();
            CheckRightCollisions();
            CheckFrontCollisions();
            //CheckBackCollisions();
            checkForCollisions = false;
        }

        if (prevState != state) {
            prevState = state;

        } else {
            switch (state) {
                case STIDLE:
                    if (!animator.GetBool("Idle")) {
                        SetAnimationTo("Idle");
                    }
                    break;

                case STWALK:
                    if (!animator.GetBool("Walk")) {
                        SetAnimationTo("Walk");
                    }
                    Vector3 initialVelocity = transform.forward * speed;
                    rb.velocity = initialVelocity;
                    break;

                case STATTACK:
                    if (!animator.GetBool("Attack")) {
                        SetAnimationTo("Attack");
                    }
                    break;

                case STDIE:
                    break;

                default:
                    break;
            }
        }
    }

    void SetAnimationTo(string s) {
        DisableAllAnimations();
        animator.SetBool(s, true);
    }

    void DisableAllAnimations() {
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Die", false);
        animator.SetBool("Attack", false);
    }

    void MakeAttack() { 
        if (prevState != state && state != STATTACK) {
            state = STATTACK;
        } 
    }

    public void DoneWithAttack() {
        state = STIDLE;
    }

    void CheckFrontCollisions() {
        Vector3 v = transform.TransformDirection(transform.position);
        ray = new Ray(new Vector3(v.x, v.y + 1f, v.z), transform.TransformDirection(Vector3.forward));
        Debug.DrawRay(ray.origin, ray.direction * 2, Color.red, 1f);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 2f)) {
            if (hit.transform.tag == "Player") {
                MakeAttack();
                Debug.Log("Front Col: player");
            } else if (hit.transform.tag == "Bomb") {
                Debug.Log("Front Col: bomb");
            } else if (hit.transform.tag == "Wall" || hit.transform.tag == "Obstacle") {
                Debug.Log("Front Col: "+ hit.transform.tag);
                Vector3 currentRotation = transform.rotation.eulerAngles;
                Vector3 newRotation = new Vector3(currentRotation.x, currentRotation.y+180f, currentRotation.z);
                transform.rotation = Quaternion.Euler(newRotation);
            } 
        }else {
            Debug.Log("Front Col: NONE");
            if (state != STWALK) { 
                state = STWALK; 
            }
        }
    }

    void CheckLeftCollisions() {
        Vector3 v = transform.TransformDirection(transform.position);
        ray = new Ray(new Vector3(v.x, v.y + 1f, v.z), transform.TransformDirection(Vector3.left));
        Debug.DrawRay(ray.origin, ray.direction * 2, Color.red, 1f);
        if (!Physics.Raycast(ray.origin, ray.direction, out hit, 2f)) {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            Vector3 newRotation = new Vector3(currentRotation.x, currentRotation.y - 90f, currentRotation.z);
            transform.rotation = Quaternion.Euler(newRotation);
        }
    }

    void CheckRightCollisions() {
        Vector3 v = transform.TransformDirection(transform.position);
        ray = new Ray(new Vector3(v.x, v.y + 1f, v.z), transform.TransformDirection(Vector3.right));
        Debug.DrawRay(ray.origin, ray.direction * 2, Color.red, 1f);
        if (!Physics.Raycast(ray.origin, ray.direction, out hit, 2f)){
            Vector3 currentRotation = transform.rotation.eulerAngles;
            Vector3 newRotation = new Vector3(currentRotation.x, currentRotation.y + 90f, currentRotation.z);
            transform.rotation = Quaternion.Euler(newRotation);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Tile")) { 
            //checkForCollisions = true;
            Debug.Log("TILE ON");
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag.Equals("Tile")) {
            float distance = Vector3.Distance(other.transform.position, this.transform.position);
            if (!isChecked && (distance < 0.5f)) {
                isChecked = checkForCollisions = true;
                Debug.Log("COLLISIONS ON");
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag.Equals("Tile")) {
            isChecked = checkForCollisions = false;
            Debug.Log("COLLISIONS OFF");
        }
    }
}
