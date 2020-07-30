using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Clickable : MonoBehaviourPun, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
    public static List<Clickable> allSelectables = new List<Clickable>();

    [SerializeField]
    private Image img;

    [SerializeField]
    private Color selectBlue;

    [SerializeField]
    private Color unSelectRed;

    [SerializeField]
    private Color unSelectWhite;

    private GameManagerScript gameManagerScript;

    [SerializeField]
    private Color cardSelected;

    [SerializeField]
    private Color canvasSelected;

    private const byte bestDate = 2;

    // private float firstClickTime, timeBetweenClicks;

    // private bool coroutineAllowed;

    // private int clickCounter;

    public bool doubleClicked = false;

    void Awake() {
        allSelectables.Add(this);
        img = this.GetComponent<Image>();
    }

    void Start() {
        GameObject gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
        // firstClickTime = 0f;
        // timeBetweenClicks = 0.2f;
        // clickCounter = 0;
        // coroutineAllowed = true;
    }

    // void Update() {
    //     if (Input.GetMouseButtonUp(0)) {
    //         clickCounter += 1;
    //     }
    //     if (clickCounter == 1 && coroutineAllowed) {
    //         firstClickTime = Time.time;
    //         StartCoroutine(DoubleClickDetection());
    //     }
    // }

    public void OnPointerClick(PointerEventData eventData) {
        OnSelect(eventData);
    }

    public void OnSelect(BaseEventData eventData) {
        if (gameManagerScript.selectingWhiteCards) {
            if (this.gameObject.name.Contains("Red")) {
                img.color = unSelectRed;
            }
            else if (this.gameObject.name.Contains("White")) {
                int index = int.Parse(this.gameObject.transform.parent.name[6].ToString());
                if (!gameManagerScript.clicked.ContainsKey(index)) {
                    gameManagerScript.clicked.Add(index, new List<GameObject>());
                }
                if (gameManagerScript.clicked[index].Count == 2 && !gameManagerScript.clicked[index].Contains(this.gameObject)) {
                    gameManagerScript.clicked[index][0].GetComponent<Clickable>().OnDeselect(eventData);
                    gameManagerScript.clicked[index].RemoveAt(0);
                    gameManagerScript.clicked[index].Add(this.gameObject);
                    img.color = selectBlue;
                }
                else if (gameManagerScript.clicked[index].Contains(this.gameObject)) {
                    gameManagerScript.clicked[index].Remove(this.gameObject);
                    img.color = unSelectWhite;
                }
                else if (!gameManagerScript.clicked[index].Contains(this.gameObject)) {
                    gameManagerScript.clicked[index].Add(this.gameObject);
                    img.color = selectBlue;
                }
            }
        }
        else if (gameManagerScript.choosingRedCards) {
            if (this.gameObject.name.Contains("White")) {
                img.color = unSelectWhite;
            }
            else if (this.gameObject.name.Contains("Red")) {
                int index = int.Parse(this.gameObject.transform.parent.name[6].ToString());
                if (gameManagerScript.clicked[index].Count == 1 && !gameManagerScript.clicked[index].Contains(this.gameObject)) {
                    gameManagerScript.clicked[index][0].GetComponent<Clickable>().OnDeselect(eventData);
                    gameManagerScript.clicked[index].RemoveAt(0);
                    gameManagerScript.clicked[index].Add(this.gameObject);
                    img.color = selectBlue;
                }
                else if (gameManagerScript.clicked[index].Contains(this.gameObject)) {
                    gameManagerScript.clicked[index].Remove(this.gameObject);
                    img.color = unSelectRed;
                }
                else if (!gameManagerScript.clicked[index].Contains(this.gameObject)) {
                    gameManagerScript.clicked[index].Add(this.gameObject);
                    img.color = selectBlue;
                }
            }
        }
        else if (!gameManagerScript.bestDateChosen) {
            GameObject[] listOfCameras = GameObject.FindGameObjectsWithTag("MainCamera");
            int singleNumber = gameManagerScript.listPlayers[0];
            foreach (GameObject camera in listOfCameras) {
                if (camera.GetComponent<PhotonView>().Owner.ActorNumber == singleNumber) { 
                    if (this.gameObject.name.Contains("Card")) {
                        GameObject canvas = this.gameObject.transform.parent.gameObject;
                        for (int i = 0; i < canvas.transform.childCount; i++) {
                            if (canvas.transform.GetChild(i).gameObject.name.Contains("White")) {
                                canvas.transform.GetChild(i).gameObject.GetComponent<WhiteCard>().cardSelected.color = cardSelected;
                            }
                            else {
                                canvas.transform.GetChild(i).gameObject.GetComponent<RedCard>().cardSelected.color = cardSelected;
                            }
                        }
                        gameManagerScript.bestDateChosen = true;
                        string name = canvas.name;
                        object[] datas = new object[] {name, true};
                        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others};
                        PhotonNetwork.RaiseEvent(bestDate, datas, raiseEventOptions, SendOptions.SendUnreliable);
                    }
                }  
            }
        }
    }

    private void OnEnable() {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void OnDisable() {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj) {
        if (obj.Code == bestDate) {
            object[] datas = (object[]) obj.CustomData;
            string name = (string) datas[0];
            bool chosen = (bool) datas[1];
            showChosen(name, chosen);
        }
    }

    private void showChosen(string name, bool chosen) {
        GameObject canvas = GameObject.Find(name);
        for (int i = 0; i < canvas.transform.childCount; i++) {
            if (canvas.transform.GetChild(i).gameObject.name.Contains("White")) {
                canvas.transform.GetChild(i).gameObject.GetComponent<WhiteCard>().cardSelected.color = cardSelected;
            }
            else {
                canvas.transform.GetChild(i).gameObject.GetComponent<RedCard>().cardSelected.color = cardSelected;
            }
        }
        gameManagerScript.bestDateChosen = chosen;
    }

    public void OnDeselect(BaseEventData eventData) {
        if (this.gameObject.name.Contains("White")) {
            img.color = unSelectWhite;
        }
        else {
            img.color = unSelectRed;
        }
    }

    // private IEnumerator DoubleClickDetection() {
    //     coroutineAllowed = false;
    //     while (Time.time < firstClickTime + timeBetweenClicks) {
    //         if (clickCounter == 2) {
    //             doubleClicked = true;
    //             break;
    //         }
    //         yield return new WaitForEndOfFrame();
    //     }
    //     clickCounter = 0;
    //     firstClickTime = 0f;
    //     coroutineAllowed = true;
    // }
}   
