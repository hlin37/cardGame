using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CardDeck : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Queue<Card> redDeck = new Queue<Card>();
    private Queue<Card> whiteDeck = new Queue<Card>();
    public GameObject redCardPrefab;

    public GameObject whiteCardPrefab;

    private CardsDatabase database;

    public GameObject _cardsDatabase;
    
    public int playerNumber;

    public void distributeCards() {
        for (int index = 1; index < 3; index++) {
            playerNumber = index;
            for (int i = 0; i < 3; i++) {
                GameObject redCard = Instantiate(redCardPrefab, transform.position, transform.rotation);
            }
            for (int i = 0; i < 4; i++) {
                GameObject whiteCard = Instantiate(whiteCardPrefab, transform.position, transform.rotation);
            }
        }
    }

    public void createDeck() {
        createRedDeck();
        createWhiteDeck();
    }

    private void createRedDeck() {
        for (int i = 0; i < 12; i++) {
            redDeck.Enqueue(database.redList[i]);
        }
    }

    private void createWhiteDeck() {
        for (int i = 0; i < 10; i++) {
            whiteDeck.Enqueue(database.whiteList[i]);
        }
    }

    public Queue<Card> returnWhiteDeck() {
        return whiteDeck;
    }

    public Queue<Card> returnRedDeck() {
        return redDeck;
    }

    void Start() {
        _cardsDatabase = GameObject.Find("CardDatabase");
        database = _cardsDatabase.GetComponent<CardsDatabase>();
        this.createDeck();
        this.distributeCards();
    }
}
