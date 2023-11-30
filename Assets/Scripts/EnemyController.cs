using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private ParticleSystem smokePS;
    [SerializeField] private GameObject skeleton;
    [SerializeField] private List<GameObject> spawnPoints;
    private Animator animator;
    private const int STIDLE = 1;
    private const int STWALK = 2;
    private const int STTURN = 3;
    private const int STATTACK = 4;
    private const int STDIE = 5;
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
    private float initialEnemyRotationY;
    private float actualEnemyRotationY;
    private float turnDegress;

    void Start() {
        //navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        state = prevState = STWALK;
        lastPosition = transform.position;
        smokePS.Stop();
        int randomIndex = Random.Range(0, spawnPoints.Count);
        this.transform.position = spawnPoints[randomIndex].transform.position;
    }

    void Update() {
        //navMeshAgent.destination = target.transform.position;
        
        if (prevState != state) {
            prevState = state;
            lastTimeChecked = Time.time;

        } else {
            switch (state) {
                case STIDLE:
                    if (!animator.GetBool("Idle")) {
                        SetAnimationTo("Idle");
                        rb.velocity = Vector3.zero;
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
                    if (isChecked && checkForCollisions) {
                        CheckLeftCollisions();
                        CheckRightCollisions();
                        //CheckBackCollisions();
                        checkForCollisions = false;
                    }

                    //Check erratic behaviour
                    if ((Time.time - lastTimeChecked) > 3f) {
                        lastTimeChecked = Time.time;
                        if (Vector3.Distance(transform.position, lastPosition) < 0.1f) {
                            Debug.Log(this.transform.gameObject.name + "ERRATIC CORRECTION");
                            initialEnemyRotationY = transform.localRotation.y;
                            turnDegress = 90;
                            state = STTURN;
                        }
                        lastPosition = transform.position;
                    }
                    break;
                
                case STTURN:
                    //Debug.Log("volta");
                    rb.velocity = Vector3.zero;
                    Turn(turnDegress);
                    break;

                case STATTACK:
                    if (!animator.GetBool("Attack")) {
                        SetAnimationTo("Attack");
                        rb.velocity = Vector3.zero;
                    }
                    break;

                case STDIE:
                    if (!animator.GetBool("Die")) {
                        SetAnimationTo("Die");
                        rb.velocity = Vector3.zero;
                    }
                    break;

                default:
                    break;
            }
        }
    }

    void Turn(float f) {
        Vector3 currentRotation = transform.localRotation.eulerAngles;
        float speedRot = 30;//90;
        actualEnemyRotationY += speedRot * Time.deltaTime;

        Vector3 newRotation = new Vector3(currentRotation.x, currentRotation.y + speedRot * Time.deltaTime, currentRotation.z);
        transform.localEulerAngles = newRotation;
        Debug.Log(Mathf.Abs(currentRotation.y - initialEnemyRotationY));

        if (Mathf.Abs(currentRotation.y-initialEnemyRotationY) > f) {
            Debug.Log(this.transform.gameObject.name + " DONE ROT");
            transform.localEulerAngles = new Vector3(newRotation.x, initialEnemyRotationY + f, newRotation.z);
            turnDegress = 0;
            state = STWALK;
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
        if (state != STATTACK) {
            state = STATTACK;
        } 
    }

    public void DoneWithAttack() {
        turnDegress = 180;
        state = STTURN;
    }

    void CheckFrontCollisions() {
        Vector3 v = new Vector3(this.transform.parent.transform.position.x, this.transform.parent.transform.position.y+1f, this.transform.parent.transform.position.z);
        ray = new Ray(v, transform.InverseTransformDirection(Vector3.forward));
        Debug.DrawRay(ray.origin, ray.direction, Color.blue, 1.5f);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 1f)) {
            if (hit.transform.tag == "Player") {
                Debug.Log("Front Col: player");
                MakeAttack();
            } else if (hit.transform.tag == "Bomb") {
                Debug.Log("Front Col: bomb");
                turnDegress = 180;
                state = STTURN;
            } else if (hit.transform.tag == "Wall" || hit.transform.tag == "Obstacle" || hit.transform.tag == "Enemy") {
                Debug.Log("Front Col: "+ hit.transform.tag);
                turnDegress = 180;
                state = STTURN;
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
            Debug.Log(this.transform.gameObject.name + " LEFT");
            turnDegress = -90;
            state = STTURN;
        }
    }

    void CheckRightCollisions() {
        Vector3 v = new Vector3(this.transform.position.x, this.transform.position.y + 1f, this.transform.position.z);
        ray = new Ray(v, transform.InverseTransformDirection(Vector3.right));
        Debug.DrawRay(ray.origin, ray.direction * 2, Color.red, 1f);

        if (!Physics.Raycast(ray.origin, ray.direction, out hit, 2f)){
            Debug.Log(this.transform.gameObject.name + " RIGHT");
            turnDegress = 90;
            state = STTURN;
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
        skeleton.SetActive(false);
        smokePS.Play();
        Invoke("DestroyEnemy", 5f);
    }
}
