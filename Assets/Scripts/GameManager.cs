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
        Debug.Log("RELOAD");
        //isPlayerAlive = true;
        //GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(0, 3.5f, 3.5f);
        //GameObject.FindGameObjectWithTag("Player").transform.rotation = Quaternion.Euler(Vector3.zero);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
