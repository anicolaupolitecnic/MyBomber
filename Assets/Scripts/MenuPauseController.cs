using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuPauseController : MonoBehaviour {
    private GameManager gManager;
    private GameObject pauseMenu;
    [SerializeField] private GameObject btnContinue, btnExit;

    private void Start() {
        gManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        pauseMenu = this.transform.GetChild(0).transform.gameObject;
    }

    public void Pause() {
        Time.timeScale = 0;
        gManager.isPlayerAlive = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(btnContinue);
        pauseMenu.SetActive(true);
    }

    public void PauseMenuContinue() {
        Time.timeScale = 1;
        gManager.isPlayerAlive = true;
        pauseMenu.SetActive(false);
    }

    public void PauseMenuExit() {
        gManager.state = 0;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
}
