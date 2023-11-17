using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour {
    public bool isPlayerAlive;
    public int numBombs;
    public int numFire;
    public int numLives;
    [SerializeField] private TextMeshProUGUI tInfo;
    [SerializeField] private TextMeshProUGUI tBombs;
    [SerializeField] private TextMeshProUGUI tFire;
    [SerializeField] private TextMeshProUGUI tLives;

    void Awake() {
        isPlayerAlive = true;
        numBombs = 1;
        numFire = 1;    
        numLives = 3;
        tInfo.enabled = false; 
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
        tFire.text = ""+numFire;
        tLives.text = ""+numLives;
    }

    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RespawnPlayer() {
        isPlayerAlive = false;
        SubstractLive();
        tInfo.enabled = true;
        ReloadSceneAfterTime(2f);
        //Invoke("ReloadScene",2f);
    }

    private IEnumerator ReloadSceneAfterTime(float delay) {
        yield return new WaitForSeconds(delay);
        ReloadScene();
    } 
}
