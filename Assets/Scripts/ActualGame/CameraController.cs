using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CameraController : MonoBehaviour
{
    public PhotonView cameraView;

    private GameManagerScript managerScript;

    public GameObject gameManager;

    private Vector3[] cameraPosition;

    private Queue<Player> singlePlayer = new Queue<Player>();

    void Awake() {
        gameManager = GameObject.Find("GameManager");
        managerScript = gameManager.GetComponent<GameManagerScript>();
        cameraPosition = managerScript.canvasesXYZ;
        singlePlayer = managerScript.returnSinglePlayer();
    }

    void Start() {
        moveCameraToPosition();
        destroyCamera();
    }

    void Update() {
        if (managerScript.checkingCards) {
            moveCameraToPlayed();
        }
        // Move camera back to choose your RedCard
        //else if (managerScript.choosingRedCards) {
            //moveCameraToPosition();
        //}
    }

    private void destroyCamera() {
        GameObject[] listOfCameras = GameObject.FindGameObjectsWithTag("MainCamera");
        List<Player> oneCamera = new List<Player>();
        for (int i = 0; i < listOfCameras.Length; i++) {
            if (!oneCamera.Contains(listOfCameras[i].GetComponent<PhotonView>().Owner)) {
                oneCamera.Add(listOfCameras[i].GetComponent<PhotonView>().Owner);
            }
            else {
                if (listOfCameras[i].GetComponent<PhotonView>().IsMine) {
                    PhotonNetwork.Destroy(listOfCameras[i]);
                }
            }
        }
        listOfCameras = GameObject.FindGameObjectsWithTag("MainCamera");
        for (int i = 0; i < listOfCameras.Length; i++) {
            if (!listOfCameras[i].GetComponent<PhotonView>().IsMine) {
                Destroy(listOfCameras[i]);
            }
        }
    }

    private void moveCameraToPosition() {
        Player player = this.cameraView.Owner;
        if (player.NickName.Equals(singlePlayer.Peek().NickName)) {
            cameraView.gameObject.transform.position = cameraPosition[cameraPosition.Length - 1];
            cameraView.gameObject.GetComponent<Camera>().orthographicSize = 650f;
        }
        else {
            cameraView.gameObject.transform.position = cameraPosition[player.ActorNumber - 1];
            cameraView.gameObject.GetComponent<Camera>().orthographicSize = 409f;
        }
    }

    private void moveCameraToPlayed() {
        cameraView.gameObject.transform.position = cameraPosition[cameraPosition.Length - 1];
        cameraView.gameObject.GetComponent<Camera>().orthographicSize = 650f;
    }
}
