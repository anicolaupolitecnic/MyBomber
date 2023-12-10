using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.AI.Navigation;

public class GameManager : MonoBehaviour {
    [SerializeField] private GameObject pauseMenu;
    public bool isPlayerAlive;
    public bool isIconKey;
    public int numBombs;
    public int numBombsThrown;
    public int numFire;
    public int numLives;
    public int numDeadEnemies;
    public GameObject spwanPoint;
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI tInfo;
    [SerializeField] private TextMeshProUGUI tBombs;
    [SerializeField] private TextMeshProUGUI tBombsThrown;
    [SerializeField] private TextMeshProUGUI tFire;
    [SerializeField] private TextMeshProUGUI tLives;
    [SerializeField] private TextMeshProUGUI tDeadEnemies;
    [SerializeField] private GameObject iconKeyOn;
    [SerializeField] private GameObject closedDoor;
    [SerializeField] private GameObject openDoor;
    [SerializeField] private AudioSource aS;
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip gameClear;
    [SerializeField] private AudioClip playerDie;
    [SerializeField] private AudioClip playerSpawn;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        ResetPlayerStats();
        numLives = 3;
        RespawnPlayer();
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

    public void IncNumDeadEnemies() {
        numDeadEnemies += 1;
        UpdateHUD();
    }

    public void ResetStats() {
        numBombs = 1;
        numFire = 1;    
        numLives = 3;
    }

    public void EnableDoor() {
        isIconKey = true;
        closedDoor.SetActive(false);
        openDoor.SetActive(true);
    }

    void UpdateHUD() {
        tBombs.text = ""+numBombs;
        tBombsThrown.text = ""+numBombsThrown;
        tFire.text = ""+numFire;
        tLives.text = ""+numLives;
        tDeadEnemies.text = "" + numDeadEnemies;
    }

    public void ResetPlayerStats() {
        isPlayerAlive = true;
        numBombs = 1;
        numBombsThrown = 0;
        numFire = 1;    
        //numLives = 3;
        tInfo.enabled = isIconKey = false; 
    }

    public void RespawnPlayer() {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = new Vector3(spwanPoint.transform.position.x, spwanPoint.transform.position.y, spwanPoint.transform.position.z);
        player.GetComponent<CharacterController>().enabled = true;

        aS.Stop();
        aS.clip = playerSpawn;
        aS.loop = false;
        aS.Play();
    }

    private IEnumerator ReloadSceneAfterTime(float delay) {
        yield return new WaitForSeconds(delay);
        tInfo.enabled = false;
        ResetPlayerStats();
        RespawnPlayer();
    }

    public void LevelClear() {
        tInfo.text = "HAS GUANYAT!";
        isPlayerAlive = false;
        StartCoroutine(LoadSceneAfterTime(3f, gameClear));
    }

    public void GameOver() {
        StartCoroutine(LoadSceneAfterTime(3f, gameOver));
    }

    private IEnumerator LoadSceneAfterTime(float delay, AudioClip aC) {
        aS.Stop();
        aS.clip = aC;
        aS.loop = false;
        aS.Play();
        tInfo.enabled = true;
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("MainMenu");
    }

    public void PauseMenu() {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void PauseMenuContinue() {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void PauseMenuExit() {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void KillPlayer() { 
        isPlayerAlive = false;
        SubstractLive();
        aS.Stop();
        aS.clip = playerDie;
        aS.loop = false;
        aS.Play();
        if (numLives <= 0) {
            tInfo.text = "HAS MORT!";
            GameOver();
        } else
            StartCoroutine(ReloadSceneAfterTime(2f));
    }
}