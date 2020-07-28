using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class SubmitCards : MonoBehaviourPun
{
    public GameObject button;

    private GameManagerScript gameManagerScript;

    private GameObject dropZone;

    private GameObject whiteHand;

    private GameObject redHand;

    private string _whiteHand = "WhiteHand";

    private string _redHand = "RedHand";

    private string _dropZone = "DropZone";

    private string _player = "Player";

    [SerializeField]
    private Color unSelectWhite;

    [SerializeField]
    private Color unSelectRed;

    [SerializeField]
    private PhotonView view;

    private const byte getNumbers = 0;

    public void OnClick() {
        if (gameManagerScript.selectingWhiteCards) {
            int[] listOfNumbers = new int[2];
            int index = int.Parse(button.transform.parent.name[6].ToString());
            int num = 0;
            if (gameManagerScript.clicked[index].Count != 2) {
                print("Not enough Cards");
            }
            else {
                dropZone = GameObject.Find(_player + index.ToString() + _dropZone);
                whiteHand = GameObject.Find(_player + index.ToString() + _whiteHand);
                foreach (GameObject card in gameManagerScript.clicked[index]) {
                    for (int i = 0; i < whiteHand.transform.childCount; i++) {
                        if (whiteHand.transform.GetChild(i).GetComponent<WhiteCard>().uniqueCardNumber.Equals(card.GetComponent<WhiteCard>().uniqueCardNumber)) {
                            Transform child = whiteHand.transform.GetChild(i);
                            child.SetParent(dropZone.transform, false);
                            listOfNumbers[num] = card.GetComponent<WhiteCard>().uniqueCardNumber;
                            card.GetComponent<Image>().color = unSelectWhite;
                        }
                    }
                    num++;
                }
                dropZone.GetComponent<DropZone>().placedCards = true;
                gameManagerScript.clicked[index].Clear();
            }
            addNumbers(listOfNumbers);
            object[] datas = new object[] {listOfNumbers};
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others};
            PhotonNetwork.RaiseEvent(getNumbers, datas, raiseEventOptions, SendOptions.SendUnreliable);
        }
        else {
            int[] listOfNumbers = new int[1];
            int index = int.Parse(button.transform.parent.name[6].ToString());
            int num = 0;
            if (gameManagerScript.clicked[index].Count != 1) {
                print("Not enough Cards");
            }
            else {
                dropZone = GameObject.Find(_player + index.ToString() + _dropZone);
                redHand = GameObject.Find(_player + index.ToString() + _redHand);
                foreach (GameObject card in gameManagerScript.clicked[index]) {
                    for (int i = 0; i < redHand.transform.childCount; i++) {
                        if (redHand.transform.GetChild(i).GetComponent<RedCard>().uniqueCardNumber.Equals(card.GetComponent<RedCard>().uniqueCardNumber)) {
                            Transform child = redHand.transform.GetChild(i);
                            child.SetParent(dropZone.transform, false);
                            listOfNumbers[num] = card.GetComponent<RedCard>().uniqueCardNumber;
                            card.GetComponent<Image>().color = unSelectRed;
                        }
                    }
                    num++;
                }
                dropZone.GetComponent<DropZone>().placedCards = true;
                gameManagerScript.clicked[index].Clear();
            }
            addNumbers(listOfNumbers);
            object[] datas = new object[] {listOfNumbers};
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others};
            PhotonNetwork.RaiseEvent(getNumbers, datas, raiseEventOptions, SendOptions.SendUnreliable);
        }
    }

    private void OnEnable() {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void OnDisable() {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj) {
        if (obj.Code == getNumbers) {
            object[] datas = (object[]) obj.CustomData;
            int[] list = (int[]) datas[0];
            addNumbers(list);
        }
    }
    private void addNumbers(int[] list) {
        foreach (int i in list) {
            if (!gameManagerScript.cardsChosen.Contains(i)) {
                gameManagerScript.cardsChosen.Add(i);
            }
        }
    }

    void Start() {
        GameObject gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
    }
}
