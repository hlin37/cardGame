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

    public List<string> singlePlayer = new List<string>();

    [SerializeField]
    private GameObject singleCanvas;

    public GameObject playedCanvasPrefab;

    public Dictionary<int, List<GameObject>> clicked = new Dictionary<int, List<GameObject>>();

    private float timeLeft = 43.0f;

    // Choose your White Cards;
    public bool selectingWhiteCards = true;

    // We are Checking Cards
    public bool checkingCards = false;

    // Choose your Red Cards;
    public bool choosingRedCards = false;

    public bool createdCameras = false;

    public bool finalSelection = false;

    public bool bestDateChosen = false;

    public List<int> cardsChosen = new List<int>();

    public List<int> listPlayers = new List<int>();

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
        string[] players = new string[PhotonNetwork.PlayerList.Length];
        int[] numbers = new int[PhotonNetwork.PlayerList.Length];
        if (PhotonNetwork.IsMasterClient) {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
                players[i] = PhotonNetwork.PlayerList[i].NickName;
            }
            players = shufflePlayers(players);
            numbers = createNumbers(players);
            addToQueue(players, numbers);
            createdCameras = true;
            object[] datas = new object[] { players, numbers };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others};
            PhotonNetwork.RaiseEvent(singleNumber, datas, raiseEventOptions, SendOptions.SendUnreliable);
        }
    }

    private int[] createNumbers(string[] players) {
        int[] numbers = new int[PhotonNetwork.PlayerList.Length];
        for (int i = 0; i < players.Length; i++) {
            foreach (Player player in PhotonNetwork.PlayerList) {
                if (players[i].Equals(player.NickName)) {
                    listPlayers.Add(player.ActorNumber);
                    numbers[i] = player.ActorNumber;
                }
            }
        }
        return numbers;
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
            int[] numbers = (int[]) datas[1];
            addToQueue(list, numbers);
        }
    }

    private void addToQueue(string[] list, int[] numbers) {
        for (int i = 0; i < list.Length; i++) {
            singlePlayer.Add(list[i]);
            if (!listPlayers.Contains(numbers[i])) {
                listPlayers.Add(numbers[i]);
            }
        }
        createdCameras = true;
        Debug.Log("Debug: This should be the first player to be single" + singlePlayer[0]);
    }

    public string[] shufflePlayers(string[] aList) {
        System.Random _random = new System.Random();
        string myGO;
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
