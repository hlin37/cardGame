using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ThisRedCard : MonoBehaviour {

    public List<Card> thisRedCard = new List<Card>();

    public static int thisId;

    public string description;

    public Text cardText;

    public GameObject hand;
    public int numberOfCardsInDeck;


    // Start is called before the first frame update
    void Start()
    {
        thisId = 0;
        thisRedCard[0] = CardDatabase.redList[thisId];
        numberOfCardsInDeck = Deck.redDeckSize;

    }

    // Update is called once per frame
    void Update()
    {
        hand = GameObject.Find("Hand");
        description = thisRedCard[0].description;
        cardText.text = "" + description;
        
        if (this.tag == "Clone") {
            thisRedCard[0] = Deck.staticRedDeck[numberOfCardsInDeck - 1];
            numberOfCardsInDeck -= 1;
            Deck.redDeckSize -= 1;
            this.tag = "Untagged";
            thisId++;
        }
    }
}
