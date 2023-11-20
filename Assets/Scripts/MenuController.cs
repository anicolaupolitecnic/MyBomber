using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {


    [SerializeField] private GameObject optsMenu; 
    [SerializeField] private GameObject aboutMenu; 

    
    public void StartGame() {
        //SceneManager.LoadScene(4);
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
}
