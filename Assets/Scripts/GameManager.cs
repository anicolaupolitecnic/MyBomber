using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour {
    public bool isPlayerAlive;
    public int numBombs;
    public int numBombsThrown;
    public int numFire;
    public int numLives;
    public GameObject spwanPoint;
    private GameObject player;

    [SerializeField] private TextMeshProUGUI tInfo;
    [SerializeField] private TextMeshProUGUI tBombs;
    [SerializeField] private TextMeshProUGUI tBombsThrown;
    [SerializeField] private TextMeshProUGUI tFire;
    [SerializeField] private TextMeshProUGUI tLives;

    void Awake() {
        ResetPlayerStats();
        numLives = 3;
    }

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = spwanPoint.transform.position;
    }

    public void AddLive() {
        numLives += 1;
        UpdateHUD();
    }

    public void SubstractLive() {
        numLives -= 1;
        UpdateHUD();
    }

    public void IncNumBombs() {
        numBombs += 1;
        UpdateHUD();
    }

    public void IncNumBombsThrown() {
        numBombsThrown += 1;
        UpdateHUD();
    }

    public void DecNumBombsThrown() {
        numBombsThrown -= 1;
        UpdateHUD();
    }

    public void IncNumFire() {
        numFire += 1;
        UpdateHUD();
    }

    public void ResetStats() {
        numBombs = 1;
        numFire = 1;    
        numLives = 3;
    }

    void UpdateHUD() {
        tBombs.text = ""+numBombs;
        tBombsThrown.text = ""+numBombsThrown;
        tFire.text = ""+numFire;
        tLives.text = ""+numLives;
    }

    void ResetPlayerStats() {
        isPlayerAlive = true;
        numBombs = 1;
        numBombsThrown = 0;
        numFire = 1;    
        //numLives = 3;
        tInfo.enabled = false; 
    }

    public void RespawnPlayer() {
        isPlayerAlive = false;
        SubstractLive();
        //this.gameObject.transform.position = spwanPoint.transform.position;
        if (numLives <= 0)
            tInfo.enabled = true;
        else
            StartCoroutine(ReloadSceneAfterTime(2f));
    }

    private IEnumerator ReloadSceneAfterTime(float delay) {
        Debug.Log("E1");
        yield return new WaitForSeconds(delay);
        tInfo.enabled = false;
        player.transform.position = spwanPoint.transform.position;
        ResetPlayerStats();
        Debug.Log("E2");
    } 
}
