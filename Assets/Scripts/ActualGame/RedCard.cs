using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RedCard : MonoBehaviour
{
    private GameObject redHand;

    public GameObject It;

    private CardDeck deck;

    public GameObject _deck;

    public Text redText;

    private string _redHand = "RedHand";

    private string _player = "Player";

    public int uniqueCardNumber;

    public Image cardSelected;

    public void setParent(int number) {
        redHand = GameObject.Find(_player + number + _redHand);
        It.transform.SetParent(redHand.transform);
        It.transform.localScale = Vector3.one;
        It.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    public void setCardText() {
        Card newCard = deck.returnRedDeck().Dequeue();
        redText.text = newCard.description;
        uniqueCardNumber = newCard.num;
        
    }

    void Awake() {
        _deck = GameObject.Find("CardDeck");
        deck = _deck.GetComponent<CardDeck>();
        this.setCardText();
        this.setParent(deck.playerNumber);
    }
}
