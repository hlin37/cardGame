using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

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

    public void setParent(int number) {
        whiteHand = GameObject.Find(_player + number + _whiteHand);
        whiteCard.transform.SetParent(whiteHand.transform);
        whiteCard.transform.localScale = Vector3.one;
        whiteCard.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    public void setCardText() {
        Card newCard = deck.returnWhiteDeck().Dequeue();
        whiteText.text = newCard.description;
        uniqueCardNumber = newCard.num;
    }

    void Awake() {
        _deck = GameObject.Find("CardDeck");
        deck = _deck.GetComponent<CardDeck>();
        this.setCardText();
        this.setParent(deck.playerNumber);
    }
}
