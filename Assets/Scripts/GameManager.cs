using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.AI.Navigation;

public class GameManager : MonoBehaviour {
    public int state;
    public int prevState;
    public bool isPlayerAlive;
    public bool isIconKey;
    public int numBombs;
    public int numBombsThrown;
    public int numFire;
    public int numLives;
    public int numDeadEnemies;
    private GameObject spwanPoint;
    private GameObject player;
    private HUDManager hud;
    private AudioSource aSFX;
    [SerializeField] private GameObject iconKeyOn;
    [SerializeField] private GameObject closedDoor;
    [SerializeField] private GameObject openDoor;
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip gameClear;
    [SerializeField] private AudioClip playerDie;
    public float volume;

    void Start() {
        state = 0;
        aSFX = GameObject.FindGameObjectWithTag("FX_AudioSource").GetComponent<AudioSource>();
    }

    public void StartGame() {
        player = GameObject.FindGameObjectWithTag("Level").GetComponent<LeveLManager>().spawnPoint;
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDManager>();

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
        hud.tBombs.text = ""+numBombs;
        hud.tBombsThrown.text = ""+numBombsThrown;
        hud.tFire.text = ""+numFire;
        hud.tLives.text = ""+numLives;
        hud.tDeadEnemies.text = "" + numDeadEnemies;
    }

    public void ResetPlayerStats() {
        isPlayerAlive = true;
        numBombs = 1;
        numBombsThrown = 0;
        numFire = 1;
        //numLives = 3;
        hud.tInfo.enabled = isIconKey = false; 
    }

    public void RespawnPlayer() {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = new Vector3(spwanPoint.transform.position.x, spwanPoint.transform.position.y, spwanPoint.transform.position.z);
        player.GetComponent<CharacterController>().enabled = true;

        aSFX.Stop();
        aSFX.clip = player.GetComponent<PlayerController>().playerSpawn;
        aSFX.loop = false;
        aSFX.Play();
    }

    private IEnumerator ReloadSceneAfterTime(float delay) {
        yield return new WaitForSeconds(delay);
        hud.tInfo.enabled = false;
        ResetPlayerStats();
        RespawnPlayer();
    }

    public void LevelClear() {
        hud.tInfo.text = "HAS GUANYAT!";
        isPlayerAlive = false;
        StartCoroutine(LoadSceneAfterTime(3f, gameClear));
    }

    public void GameOver() {
        StartCoroutine(LoadSceneAfterTime(3f, gameOver));
    }

    private IEnumerator LoadSceneAfterTime(float delay, AudioClip aC) {
        aSFX.Stop();
        aSFX.clip = aC;
        aSFX.loop = false;
        aSFX.Play();
        hud.tInfo.enabled = true;
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("MainMenu");
    }

    public void KillPlayer() { 
        isPlayerAlive = false;
        SubstractLive();
        aSFX.Stop();
        aSFX.clip = player.GetComponent<PlayerController>().playerDie;
        aSFX.loop = false;
        aSFX.Play();
        if (numLives <= 0) {
            hud.tInfo.text = "HAS MORT!";
            GameOver();
        } else
            StartCoroutine(ReloadSceneAfterTime(2f));
    }
}