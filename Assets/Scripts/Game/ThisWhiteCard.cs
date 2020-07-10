using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ThisWhiteCard : MonoBehaviour {

    public List<Card> thisWhiteCard = new List<Card>();

    public static int thisId;

    public string description;

    public Text cardText;

    public GameObject whiteHand;

    public int numberOfCardsInDeck;

    // Start is called before the first frame update
    void Start()
    {
        thisId = 0;
        thisWhiteCard[0] = CardDatabase.whiteList[thisId];
        numberOfCardsInDeck = Deck.whiteDeckSize;
    }

    // Update is called once per frame
    void Update()
    {
        whiteHand = GameObject.Find("whiteHand");
        description = thisWhiteCard[0].description;
        cardText.text = "" + description;

        if (this.tag == "Clone") {
            thisWhiteCard[0] = Deck.staticWhiteDeck[numberOfCardsInDeck - 1];
            numberOfCardsInDeck -= 1;
            Deck.whiteDeckSize -= 1;
            this.tag = "Untagged"; 
        }
    }
}
