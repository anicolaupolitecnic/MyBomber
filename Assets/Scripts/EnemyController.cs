using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private float lastTimeChecked;
    private Vector3 lastPosition;

    void Start() {
        //navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        state = prevState = STWALK;
        lastPosition = transform.position;
        //CheckFrontCollisions();
    }

    void Update() {
        //navMeshAgent.destination = target.transform.position;
        
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
                    //Animation
                    if (!animator.GetBool("Walk")) {
                        SetAnimationTo("Walk");
                    }
                    //Movement
                    Vector3 initialVelocity = transform.forward * speed;
                    rb.velocity = initialVelocity;

                    //Check environment
                    CheckFrontCollisions();
                    if (isChecked && checkForCollisions)
                    {
                        CheckLeftCollisions();
                        CheckRightCollisions();
                        //CheckBackCollisions();
                        checkForCollisions = false;
                    }

                    //Check erratic behaviour
                    if ((Time.time - lastTimeChecked) > 1f) {
                        lastTimeChecked = Time.time;
                        if (Vector3.Distance(transform.position, lastPosition) < 0.1f) {
                            Debug.Log("ERRATIC CORRECTION");
                            Turn(180);
                        }

                        lastPosition = transform.position;
                    }

                    break;

                case STATTACK:
                    if (!animator.GetBool("Attack")) {
                        SetAnimationTo("Attack");
                    }
                    break;

                case STDIE:
                    if (!animator.GetBool("Die")) {
                        SetAnimationTo("Die");
                    }
                    break;

                default:
                    break;
            }
        }
    }

    void Turn(float f) {
        Vector3 currentRotation = transform.localRotation.eulerAngles;
        Vector3 newRotation = new Vector3(currentRotation.x, currentRotation.y + f, currentRotation.z);
        transform.localEulerAngles = newRotation;
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
        if (state != STATTACK) {
            state = STATTACK;
        } 
    }

    public void DoneWithAttack() {
        state = STWALK;
        Turn(180);
    }

    void CheckFrontCollisions() {
        Vector3 v = new Vector3(this.transform.position.x, this.transform.position.y+1f, this.transform.position.z);
        ray = new Ray(v, transform.InverseTransformDirection(Vector3.forward));
        Debug.DrawRay(ray.origin, ray.direction, Color.blue, 1.5f);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 1f)) {
            if (hit.transform.tag == "Player") {
                Debug.Log("Front Col: player");
                MakeAttack();
            } else if (hit.transform.tag == "Bomb") {
                Debug.Log("Front Col: bomb");
                Turn(180);
            } else if (hit.transform.tag == "Wall" || hit.transform.tag == "Obstacle") {
                Debug.Log("Front Col: "+ hit.transform.tag);
                Turn(180);
            } 
        }else {
            Debug.Log("Front Col: NONE");
            if (state != STWALK) { 
                state = STWALK; 
            }
        }
    }

    void CheckLeftCollisions() {
        Vector3 v = new Vector3(this.transform.position.x, this.transform.position.y + 1f, this.transform.position.z);
        ray = new Ray(v, transform.InverseTransformDirection(Vector3.left));
        Debug.DrawRay(ray.origin, ray.direction * 2, Color.red, 1f);

        if (!Physics.Raycast(ray.origin, ray.direction, out hit, 2f)) {
            Debug.Log("LEFT");
            Turn(-90);
        }
    }

    void CheckRightCollisions() {
        Vector3 v = new Vector3(this.transform.position.x, this.transform.position.y + 1f, this.transform.position.z);
        ray = new Ray(v, transform.InverseTransformDirection(Vector3.right));
        Debug.DrawRay(ray.origin, ray.direction * 2, Color.red, 1f);

        if (!Physics.Raycast(ray.origin, ray.direction, out hit, 2f)){
            Debug.Log("RIGHT");
            Turn(90);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Tile")) { 
            //checkForCollisions = true;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag.Equals("Tile")) {
            float distance = Vector3.Distance(other.transform.position, this.transform.position);
            if (!isChecked && (distance < 0.5f)) {
                isChecked = checkForCollisions = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag.Equals("Tile")) {
            isChecked = checkForCollisions = false;
        }
    }

    private void DestroyEnemy() {
        Destroy(this.gameObject);
    }

    public void PlayDead() {
        state = STDIE;
        Invoke("DestroyEnemy", 5f);
    }
}
