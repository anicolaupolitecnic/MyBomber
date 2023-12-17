using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.AI.Navigation;

public class GameManager : MonoBehaviour {
    public bool isPlayerAlive;
    public bool isIconKey;
    public int numBombs;
    public int numBombsThrown;
    public int numFire;
    public int numLives;
    public int numDeadEnemies;
    [SerializeField] private GameObject spwanPoint;
    [SerializeField] private GameObject player;
    [SerializeField] private HUDManager hud;
    private MusicManager musicManager;
    [SerializeField] private GameObject iconKeyOn;
    [SerializeField] private GameObject closedDoor;
    [SerializeField] private GameObject openDoor;

    public float volume;

    void Awake() {
        musicManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<MusicManager>();
        musicManager.GameMusic();
    }

    void Start() {
        ResetPlayerStats();
        numLives = 3;
        RespawnPlayer();
    }

    public void AddLive() {
        numLives += 1;
        hud.UpdateHUD();
    }

    public void SubstractLive() {
        numLives -= 1;
        hud.UpdateHUD();
    }

    public void IncNumBombs() {
        numBombs += 1;
        hud.UpdateHUD();
    }

    public void IncNumBombsThrown() {
        numBombsThrown += 1;
        hud.UpdateHUD();
    }

    public void DecNumBombsThrown() {
        numBombsThrown -= 1;
        hud.UpdateHUD();
    }

    public void IncNumFire() {
        numFire += 1;
        hud.UpdateHUD();
    }

    public void IncNumDeadEnemies() {
        numDeadEnemies += 1;
        hud.UpdateHUD();
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
        StartCoroutine(LoadSceneAfterTime(3f, musicManager.gameClear));
    }

    public void GameOver() {
        StartCoroutine(LoadSceneAfterTime(3f, musicManager.gameOver));
    }

    private IEnumerator LoadSceneAfterTime(float delay, AudioClip aC) {
        musicManager.PlayFX(aC, false);
        hud.tInfo.enabled = true;
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("MainMenu");
    }

    public void KillPlayer() { 
        isPlayerAlive = false;
        SubstractLive();
        musicManager.PlayFX(musicManager.playerDie, false);

        if (numLives <= 0) {
            hud.tInfo.text = "HAS MORT!";
            GameOver();
        } else
            StartCoroutine(ReloadSceneAfterTime(2f));
    }
}