using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : MonoBehaviour {
    private Animator animator;
    private const int STIDLE = 1;
    private const int STWALK = 2;
    private const int STATTACK = 3;
    private const int STDIE = 4;
    private int state;
    private int prevState;

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
        ray = new Ray();
    }

    void Update() {
        //navMeshAgent.destination = target.transform.position;

        if (prevState != state) {
            prevState = state;


        } else {
            CheckFrontCollisions();
            //CheckSideCollisions();

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
                    Vector3 initialVelocity = transform.forward * 3f;
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

    void CheckSideCollisions() {
        //CheckBackCollisions();
        CheckLeftCollisions();
        CheckRightCollisions();
    }

    void CheckFrontCollisions() {
        ray = new Ray(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Vector3.forward);
        //Debug.DrawRay(ray.origin, ray.direction * 2, Color.red, 1f);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 2f)) {
            if (hit.transform.tag == "Player") {
                MakeAttack();
                Debug.Log("Front Col: player");
            } else if (hit.transform.tag == "Bomb") {
                Debug.Log("Front Col: bomb");
            } else if (hit.transform.tag == "Wall" || hit.transform.tag == "Obstacle") {
                Debug.Log("Front Col: "+ hit.transform.tag);
                Vector3 currentRotation = transform.rotation.eulerAngles;
                Vector3 newRotation = new Vector3(currentRotation.x, 90f, currentRotation.z);
                transform.rotation = Quaternion.Euler(newRotation);
            } 
        }else {
            Debug.Log("Front Col: NONE");
            if (state != STWALK) { 
                state = STWALK; 
            }
        }
    }

    void CheckBackCollisions() {
        ray = new Ray(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Vector3.back);
        //Debug.DrawRay(ray.origin, ray.direction * 2, Color.red, 1f);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 2f)) {
            CheckHitCollisions(hit);
        }
    }

    void CheckLeftCollisions() {
        ray = new Ray(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Vector3.left);
        //Debug.DrawRay(ray.origin, ray.direction * 2, Color.red, 1f);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 2f)){
            CheckHitCollisions(hit);
        }
    }

    void CheckRightCollisions() {
        ray = new Ray(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Vector3.right);
        //Debug.DrawRay(ray.origin, ray.direction * 2, Color.red, 1f);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 2f)){
            CheckHitCollisions(hit);
        }
    }

    void CheckHitCollisions(RaycastHit hit) {
        if (hit.transform.tag == "Player") {

        } else if (hit.transform.tag == "Wall") {

        } else if (hit.transform.tag == "Bomb") {

        }
    }
}
