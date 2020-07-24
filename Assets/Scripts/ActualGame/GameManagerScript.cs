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

    [SerializeField]
    private GameObject singleCanvas;

    public GameObject playedCanvasPrefab;

    public Dictionary<int, List<GameObject>> clicked = new Dictionary<int, List<GameObject>>();

    private float timeLeft = 10.0f;

    // Choose your White Cards;
    public bool selectingWhiteCards = true;

    // We are Checking Cards
    public bool checkingCards = false;

    // Choose your Red Cards;
    public bool choosingRedCards = false;

    public List<int> cardsChosen = new List<int>();

    public List<int> listPlayers = new List<int>();

    private void generateCameras() {
        base.photonView.RPC("createCamera", RpcTarget.All);
    }

    public Queue<Player> returnSinglePlayer() {
        return singlePlayer;
    }

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
            listPlayers.Add(listOfPlayers[i].ActorNumber);
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

    private void generateCanvas() {
        base.photonView.RPC("createPlayedCanvas", RpcTarget.All);
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
        if (timeLeft < 0 && !checkingCards) {
            checkingCards = true;
            generateCanvas();
            //choosingRedCards = true;
        }
    }

    [PunRPC]
    private void createCamera() {
        GameObject playerCamera = PhotonNetwork.Instantiate(playerCameraPrefab.name, transform.position, transform.rotation);
    }

    [PunRPC]
    private void createPlayedCanvas() {
        GameObject playedCanvas = PhotonNetwork.Instantiate(playedCanvasPrefab.name, transform.position, transform.rotation);
    }
}
