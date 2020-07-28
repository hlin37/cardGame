using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private PhotonView cameraView;

    private GameManagerScript managerScript;

    public GameObject gameManager;

    private Vector3[] cameraPosition;

    private Queue<Player> singlePlayer = new Queue<Player>();

    private int stopper = 0;

    private Player player;

    void Awake() {
        gameManager = GameObject.Find("GameManager");
        managerScript = gameManager.GetComponent<GameManagerScript>();
        cameraPosition = managerScript.canvasesXYZ;
        singlePlayer = managerScript.singlePlayer;
    }

    //switched the method order
    void Start() {
        destroyCamera();
        moveCameraToPosition();
    }

    void Update() {
        if (managerScript.checkingCards && stopper == 0) {
            stopper = 1;
            moveCameraToPlayed();
        }
        // Move camera back to choose your RedCard
        else if (managerScript.choosingRedCards && stopper == 1) {
            stopper = 2;
            moveCameraToPosition();
        }
        else if (managerScript.finalSelection && stopper == 2) {
            stopper = 3;
            moveCameraToPlayed();
        }
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

    // Instead of this.cameraView.Owner;
    private void moveCameraToPosition() {
        GameObject[] listOfCameras = GameObject.FindGameObjectsWithTag("MainCamera");
        foreach (GameObject camera in listOfCameras) {
            player = camera.GetComponent<PhotonView>().Owner;
            //cameraView = camera.GetComponent<PhotonView>();
        }
        if (cameraView.IsMine) {
            if (player.NickName.Equals(singlePlayer.Peek().NickName)) {
                cameraView.gameObject.transform.position = cameraPosition[cameraPosition.Length - 1];
                cameraView.gameObject.GetComponent<Camera>().orthographicSize = 650f;
            }
            else {
                cameraView.gameObject.transform.position = cameraPosition[player.ActorNumber - 1];
                cameraView.gameObject.GetComponent<Camera>().orthographicSize = 409f;
            }
        }
    }

    private void moveCameraToPlayed() {
        cameraView.gameObject.transform.position = cameraPosition[cameraPosition.Length - 1];
        cameraView.gameObject.GetComponent<Camera>().orthographicSize = 650f;
    }
}
