using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour {
    [SerializeField] private GameObject pauseMenu;
    private float soundVolume;
    [SerializeField] TMP_Text textSoundVolume;
    [SerializeField] Slider soundSlider;
    [SerializeField] private GameObject optsMenu, aboutMenu;
    [SerializeField] private GameObject btnPlay, btnSlideSound, btnCloseAboutMenu;
    [SerializeField] private List<GameObject> ps;
    [SerializeField] private ScrollRect myScrollRect;
    private GameManager gManager;

    private void Start() {
        gManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        soundSlider.onValueChanged.AddListener(delegate { SetSoundVolume(); });
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(btnPlay);
    }

    public void StartGame() {
        SceneManager.LoadScene(1);
        Debug.Log("Càrrega joc");
    }
    
    public void CloseMenuOptions() {
        foreach (GameObject p in ps){
            p.SetActive(true);
        }
        optsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(btnPlay);
    }

    public void CloseMenuAbout() {
        foreach (GameObject p in ps) {
            p.SetActive(true);
        }
        aboutMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(btnPlay);
    }

    public void OpenMenuOptions() {
        foreach (GameObject p in ps) {
            p.SetActive(false);
        }
        EventSystem.current.SetSelectedGameObject(btnSlideSound);
        optsMenu.SetActive(true);
    }

    public void OpenMenuAbout() {
        foreach (GameObject p in ps) {
            p.SetActive(false);
        }
        EventSystem.current.SetSelectedGameObject(btnCloseAboutMenu);
        myScrollRect.verticalNormalizedPosition = 1f;
        aboutMenu.SetActive(true);
    }

    public void ExitGame() {
        //Application.Quit();
        Debug.Log("EXIT GAME");
    }

    public void SetSoundVolume() {
        soundVolume = soundSlider.value;
        textSoundVolume.text = soundVolume.ToString();
        Debug.Log("soundVolume: " + soundVolume);
    }

}
