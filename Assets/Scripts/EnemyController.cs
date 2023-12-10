using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : MonoBehaviour {
    private GameManager gManager;
    private AudioSource aS;
    [SerializeField] private AudioClip woosh;
    [SerializeField] private AudioClip die;
    [SerializeField] private AudioClip step;
    [SerializeField] private float rangeOfView;
    [SerializeField] private float attackRange;
    [SerializeField] private ParticleSystem smokePS;
    [SerializeField] private GameObject skeleton;
    [SerializeField] private GameObject player;
    [SerializeField] private List<GameObject> spawnPoints;

    private Animator animator;
    private const int STIDLE = 1;
    private const int STWALK = 2;
    private const int STATTACK = 3;
    private const int STDIE = 4;
    private int state;
    private int prevState;
    private bool isChasingPlayer = false;

    private bool isChecked = false;
    private NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject target;
    private Rigidbody rb;

    int randomIndex;

    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        aS = GetComponent<AudioSource>();
        gManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        RestartEnemy();
    }

    public void RestartEnemy() {
        state = prevState = STWALK;
        smokePS.Stop();
        randomIndex = Random.Range(0, spawnPoints.Count);

        navMeshAgent.enabled = false;
        transform.parent.transform.position = spawnPoints[randomIndex].transform.position;
        navMeshAgent.enabled = true;
        getDestinationPath();
    }

    void Update() {        
        if (prevState != state) {
            prevState = state;
            DisableAllAnimations();
            //lastTimeChecked = Time.time;

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

                    Vector3 newPosition = this.transform.position + transform.forward * 0.75f * Time.deltaTime;
                    rb.MovePosition(newPosition);

                    ChasePlayer();
                    if (Vector3.Distance(this.transform.position, spawnPoints[randomIndex].transform.position) < 1f) {
                        getDestinationPath();
                        navMeshAgent.destination = spawnPoints[randomIndex].transform.position;
                    }
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

    void getDestinationPath() {
        if (spawnPoints.Count > 1) {
            int i = Random.Range(0, spawnPoints.Count);

            NavMeshPath navMeshPath = new NavMeshPath();
            if (navMeshAgent.CalculatePath(spawnPoints[i].transform.position, navMeshPath) && 
                navMeshPath.status == NavMeshPathStatus.PathComplete &&
                randomIndex != i) {
                navMeshAgent.SetPath(navMeshPath);
                randomIndex = i;
            } else {
                getDestinationPath();
            }
        }
    }


    void ChasePlayer() {
        float distanceToTarget = Vector3.Distance(transform.position, player.transform.position);

        if (gManager.isPlayerAlive && distanceToTarget < attackRange) {
            if (!isChasingPlayer) { 
                navMeshAgent.destination = player.transform.position;
                isChasingPlayer = true;
            }
            if (distanceToTarget < (attackRange/2)+ (attackRange/4)) {
                MakeAttack();
                //isChasingPlayer = false;
                Debug.Log("attack");
            }
        } else {
            isChasingPlayer = false;
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
        //Debug.Log("done with attack");
        if (state != STWALK) {
            state = STWALK;
        }
    }

    public void SoundWoosh() {
        aS.Stop();
        aS.clip = woosh;
        aS.loop = false;
        aS.Play();
    }

    public void CheckPlayerHit() {
        if (isChasingPlayer) {
            Debug.Log("Player dead");
            if (gManager.isPlayerAlive) {
                gManager.KillPlayer();
                RestartEnemy();
            }
        }
    }

    private void DestroyEnemy() {
        Destroy(this.gameObject);
    }

    public void PlayDead() {
        state = STDIE;
        skeleton.SetActive(false);
        smokePS.Play();
        this.GetComponent<BoxCollider>().enabled = false;
        aS.Stop();
        aS.clip = die;
        aS.loop = false;
        aS.Play();
        Invoke("DestroyEnemy", 5f);
    }
}