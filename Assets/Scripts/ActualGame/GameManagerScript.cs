using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class GameManagerScript : MonoBehaviourPunCallbacks
{
    public GameObject playerCameraPrefab;

    private GameObject gameBoard;

    public Vector3[] canvasesXYZ;

    private Queue<Player> singlePlayer = new Queue<Player>();

    private GameObject singleCanvas;

    public GameObject playedCanvasPrefab;

    private GameObject playedCanvas;

    public Dictionary<int, List<GameObject>> clicked = new Dictionary<int, List<GameObject>>();
    
    public bool selectingWhiteCards = true;

    private float timeLeft = 10.0f;

    private bool checkingCards = false;

    private void generateCameras() {
        base.photonView.RPC("createCamera", RpcTarget.All);
    }

    public Queue<Player> returnSinglePlayer() {
        return singlePlayer;
    }

    // 650 is camera size

    private Vector3[] returnCameraPosition() {
        Vector3[] list = new Vector3[gameBoard.transform.childCount];
        for (int id = 0; id < gameBoard.transform.childCount; id++) {
            Vector3 coordinates = new Vector3(gameBoard.transform.GetChild(id).position.x, gameBoard.transform.GetChild(id).position.y, -10);
            list[id] = coordinates;
        }
        return list;
    }

    private void chooseSinglePlayer() {
        Player[] listOfPlayers = shufflePlayers(PhotonNetwork.PlayerList);
        for (int i = 0; i < listOfPlayers.Length; i++) {
            singlePlayer.Enqueue(listOfPlayers[i]);
        }
        print("This should be the first player to be single" + singlePlayer.Peek().NickName);
    }

    public Player[] shufflePlayers(Player[] aList) {
        System.Random _random = new System.Random ();
        Player myGO;
        int n = aList.Length;
        for (int i = 0; i < n; i++) {
            int r = i + (int)(_random.NextDouble() * (n - i));
            myGO = aList[r];
            aList[r] = aList[i];
            aList[i] = myGO;
        }
        return aList;
    }

    public void moveCards() {
        GameObject[] dropZones = GameObject.FindGameObjectsWithTag("DropZone");
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
            if (dropZones[i].GetComponent<DropZone>().placedCards) {
                for (int j = 1; j < dropZones[i].transform.childCount; j++) {
                    GameObject card = dropZones[i].gameObject.transform.GetChild(1).gameObject;
                    card.transform.SetParent(playedCanvas.transform);
                }
            }
        }
    }

    private void generateCanvas() {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
            base.photonView.RPC("createPlayedCanvas", PhotonNetwork.PlayerList[i]);
        }
    }

    private void printActorNumber() {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
            print("This is player" + PhotonNetwork.PlayerList[i].NickName);
            print("This is my number + " + PhotonNetwork.PlayerList[i].ActorNumber);
        }
    }

    void Awake() {
        gameBoard = GameObject.Find("GameBoard");
        singleCanvas = GameObject.Find("SingleCanvas");
        canvasesXYZ = returnCameraPosition();
        chooseSinglePlayer();
        generateCameras();
    }

    void Update() {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) {
            while (!checkingCards) {
                checkingCards = true;
                generateCanvas();
                moveCards();
            }
        }
    }

    [PunRPC]
    private void createCamera() {
        GameObject playerCamera = PhotonNetwork.Instantiate(playerCameraPrefab.name, transform.position, transform.rotation);
    }

    [PunRPC]
    private void createPlayedCanvas() {
        playedCanvas = PhotonNetwork.Instantiate(playedCanvasPrefab.name, transform.position, transform.rotation);
        playedCanvas.transform.SetParent(singleCanvas.transform);
    }
    IEnumerator ExampleCoroutine()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(60);
    }   
}
