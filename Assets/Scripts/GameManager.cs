using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {
    public bool isPlayerAlive;

    void Awake() {
        isPlayerAlive = true;
    }

    public void ReloadScene() {
        Debug.Log("2");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
