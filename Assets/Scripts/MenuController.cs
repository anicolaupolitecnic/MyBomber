using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    private float soundVolume;
    [SerializeField] TMP_Text textSoundVolume;
    [SerializeField] Slider soundSlider;
    [SerializeField] private GameObject optsMenu; 
    [SerializeField] private GameObject aboutMenu;

    private void Start() {
        soundSlider.onValueChanged.AddListener(delegate { SetSoundVolume(); });
    }

    public void StartGame() {
        SceneManager.LoadScene(1);
        Debug.Log("Càrrega joc");
    }
    
    public void CloseMenuOptions() {
        optsMenu.SetActive(false);
    }

    public void CloseMenuAbout() {
        aboutMenu.SetActive(false);
    }

    public void OpenMenuOptions() {
        optsMenu.SetActive(true);
    }

    public void OpenMenuAbout() {
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
