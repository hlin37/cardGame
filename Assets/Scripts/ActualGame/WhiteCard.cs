using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class WhiteCard : MonoBehaviourPun
{
    public GameObject whiteHand;

    public GameObject whiteCard;

    private CardDeck deck;

    public GameObject _deck;

    public Text whiteText;

    private string _whiteHand = "WhiteHand";

    private string _player = "Player";

    public int uniqueCardNumber;

    [SerializeField]
    private Text customInput;

    public GameObject field;

    public Image cardSelected;

    public bool customCard = false;

    public bool customWritten = false;

    private const byte custom = 3;

    public bool recievingEnd = false;

    public string receivingText;

    public string global;

    public void setParent(int number) {
        whiteHand = GameObject.Find(_player + number + _whiteHand);
        whiteCard.transform.SetParent(whiteHand.transform);
        whiteCard.transform.localScale = Vector3.one;
        whiteCard.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    public void setCardText() {
        Card newCard = deck.returnWhiteDeck().Dequeue();
        uniqueCardNumber = newCard.num;
        if (newCard.description.Contains("Custom Card")) {
            whiteText.text = newCard.description;
            customCard = true;
            activateInputField(uniqueCardNumber);
        }
        else {
            whiteText.text = newCard.description;
        }
    }

    private void activateInputField(int number) {
        Debug.Log(number);
        GameObject[] whiteCards = GameObject.FindGameObjectsWithTag("WhiteCard");
        foreach (GameObject card in whiteCards) {
            if (card.GetComponent<WhiteCard>().uniqueCardNumber == number) {
                card.GetComponent<WhiteCard>().field.SetActive(true);
            }
        }
    }

    private void disableInputField(int number) {
        GameObject[] whiteCards = GameObject.FindGameObjectsWithTag("WhiteCard");
        foreach (GameObject card in whiteCards) {
            if (card.GetComponent<WhiteCard>().uniqueCardNumber == number) {
                card.GetComponent<WhiteCard>().field.SetActive(false);
            }
        }
    }

    private void customText() {
        string input = customInput.text.ToString();
        global = input;
        whiteText.text = customInput.text;
        customWritten = true;
        object[] datas = new object[] { input, uniqueCardNumber};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others};
        PhotonNetwork.RaiseEvent(custom, datas, raiseEventOptions, SendOptions.SendUnreliable);
    }

    private void OnEnable() {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable() {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public void OnEvent(EventData obj) {
        if (obj.Code == custom) {
            object[] datas = (object[]) obj.CustomData;
            string list = (string) datas[0];
            int number = (int) datas[1];
            updateText(list, number);
        }
    }

    private void updateText(string input, int cardNumber) {
        GameObject[] whiteCards = GameObject.FindGameObjectsWithTag("WhiteCard");
        foreach (GameObject card in whiteCards) {
            if (card.GetComponent<WhiteCard>().uniqueCardNumber == cardNumber) {
                card.GetComponent<WhiteCard>().field.SetActive(false);
                card.GetComponent<WhiteCard>().receivingText = input;
                card.GetComponent<WhiteCard>().recievingEnd = true;
            }
        }
    }

    private void setText(string text) {
        whiteText.text = text;
    }

    void Awake() {
        _deck = GameObject.Find("CardDeck");
        deck = _deck.GetComponent<CardDeck>();
        this.setCardText();
        this.setParent(deck.playerNumber);
    }

    void Update() {
        if (whiteText.text.ToString().Equals("Custom Card") && field.GetComponent<InputField>().text.ToString().Length > 0 && Input.GetKeyDown(KeyCode.Return) && !customWritten) {
            customText();
            int number = findCard(global);
            disableInputField(number);
        }
        if (recievingEnd) {
            setText(receivingText);
            recievingEnd = false;
        }
    }

    private int findCard(string text) {
        int number = -2;
        GameObject[] whiteCards = GameObject.FindGameObjectsWithTag("WhiteCard");
        foreach (GameObject card in whiteCards) {
            if (card.GetComponent<WhiteCard>().whiteText.text.ToString().Equals(text)) {
                number = card.GetComponent<WhiteCard>().uniqueCardNumber;
            }
        }
        return number;
    }
}
