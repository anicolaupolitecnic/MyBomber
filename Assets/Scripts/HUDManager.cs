using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour {
    GameManager gManager;

    [SerializeField] public TextMeshProUGUI tInfo;
    [SerializeField] public TextMeshProUGUI tBombs;
    [SerializeField] public TextMeshProUGUI tBombsThrown;
    [SerializeField] public TextMeshProUGUI tFire;
    [SerializeField] public TextMeshProUGUI tLives;
    [SerializeField] public TextMeshProUGUI tDeadEnemies;

    void Start () {
        gManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void UpdateHUD() {
        tBombs.text = ""+gManager.numBombs;
        tBombsThrown.text = ""+ gManager.numBombsThrown;
        tFire.text = ""+ gManager.numFire;
        tLives.text = ""+ gManager.numLives;
        tDeadEnemies.text = "" + gManager.numDeadEnemies;
    }
}
