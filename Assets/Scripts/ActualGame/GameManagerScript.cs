using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class GameManagerScript : MonoBehaviour
{
    public Camera playerCamera;

    private GameObject gameBoard;

    public Vector3[] canvasesXYZ;

    private PhotonView view;

    private void createCamera() {
        GameObject manager = GameObject.Find("GameManager");
        view = manager.GetComponent<PhotonView>();
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
            view.RPC("localCamera", PhotonNetwork.PlayerList[i], i);
        }
    }
    //destroy audiolistener

    private Vector3[] returnCanvasPosition() {
        Vector3[] list = new Vector3[gameBoard.transform.childCount];
        for (int id = 0; id < gameBoard.transform.childCount; id++) {
            Vector3 coordinates = new Vector3(gameBoard.transform.GetChild(id).position.x, gameBoard.transform.GetChild(id).position.y, -10);
            list[id] = coordinates;
        }
        return list;
    }

    void Awake() {
        gameBoard = GameObject.Find("GameBoard");
        canvasesXYZ = returnCanvasPosition();
        createCamera();
    }

    [PunRPC]
    private void localCamera(int number) {
        playerCamera = Instantiate(playerCamera, canvasesXYZ[number], new Quaternion(0,0,0,0));
    }
}

