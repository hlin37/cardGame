using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class ScoreBoard : MonoBehaviour
{

    public Dictionary<string, int> score = new Dictionary<string, int>();

    private GameManagerScript gameManagerScript;

    void Awake() {
        GameObject gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
    }

    void Start() {

    }
}
