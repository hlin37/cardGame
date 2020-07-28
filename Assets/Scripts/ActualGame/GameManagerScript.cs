using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GameManagerScript : MonoBehaviourPun
{
    public GameObject playerCameraPrefab;

    private GameObject gameBoard;

    public Vector3[] canvasesXYZ;

    public Queue<Player> singlePlayer = new Queue<Player>();

    [SerializeField]
    private GameObject singleCanvas;

    public GameObject playedCanvasPrefab;

    public Dictionary<int, List<GameObject>> clicked = new Dictionary<int, List<GameObject>>();

    private float timeLeft = 200.0f;

    // Choose your White Cards;
    public bool selectingWhiteCards = true;

    // We are Checking Cards
    public bool checkingCards = false;

    // Choose your Red Cards;
    public bool choosingRedCards = false;

    public bool finalSelection = false;

    public List<int> cardsChosen = new List<int>();

    public List<int> listPlayers = new List<int>();

    public List<Player> listingsOfPlayers = new List<Player>();

    private const byte singleNumber = 1;

    private void generateCameras() {
        base.photonView.RPC("createCamera", RpcTarget.All);
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
        string[] someList = new string[listOfPlayers.Length];
        for (int i = 0; i < listOfPlayers.Length; i++) {
            listingsOfPlayers.Add(listOfPlayers[i]);
            someList[i] = listOfPlayers[i].NickName;
        }
        object[] datas = new object[] { someList };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others};
        PhotonNetwork.RaiseEvent(singleNumber, datas, raiseEventOptions, SendOptions.SendUnreliable);
    }

    private void OnEnable() {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable() {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public void OnEvent(EventData obj) {
        if (obj.Code == singleNumber) {
            object[] datas = (object[]) obj.CustomData;
            string[] list = (string[]) datas[0];
            createSinglePlayer(list);
        }
    }

    private void createSinglePlayer(string[] list) {
        Player[] listOfPlayers = PhotonNetwork.PlayerList;
        for (int i = 0; i < list.Length; i++) {
            foreach (Player player in listOfPlayers) {
                if (list[i].Equals(player.NickName)) {
                    if (!listingsOfPlayers.Contains(player)) {
                        listingsOfPlayers.Add(player);
                    }
                }
            }
        }
    }

    public void createQueue() {
        for (int i = 0; i < listingsOfPlayers.Count; i++) {
            if (i == 0) {
                print(listingsOfPlayers[i].NickName);
            }
            singlePlayer.Enqueue(listingsOfPlayers[i]);
            listPlayers.Add(listingsOfPlayers[i].ActorNumber);
        }
        print("This should be the first player to be single" + singlePlayer.Peek().NickName);
        Debug.Log("Debug: This should be the first player to be single" + singlePlayer.Peek().NickName);
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
        createQueue();
        generateCameras();
    }


    // 43 seconds per round;
    void Update() {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 30 && timeLeft > 20 && !checkingCards) {
            checkingCards = true;
            generateCanvas();
        }
        else if (timeLeft < 20 && timeLeft > 10 && !choosingRedCards) {
            choosingRedCards = true;
            selectingWhiteCards = false;
        }
        else if (timeLeft < 0) {
            choosingRedCards = false;
            finalSelection = true;
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
