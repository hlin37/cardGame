using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

    public List<Card> redDeck = new List<Card>();

    public List<Card> whiteDeck = new List<Card>();

    public static List<Card> staticRedDeck = new List<Card>();

    public static List<Card> staticWhiteDeck = new List<Card>();

    public static int redDeckSize;

    public static int whiteDeckSize;

    public int x;

    public GameObject hand;

    public GameObject whiteHand;

    public GameObject someDeck;

    public GameObject redCardToHand;

    public GameObject whiteCardToHand;


    // Start is called before the first frame update
    void Start()
    {
        x = 0;
        redDeckSize = 5;
        whiteDeckSize = 10;
        for (int i = 0; i < 15; i++) {
            if (i < 5) {
                redDeck[i] = CardDatabase.redList[i];
            }
            else {
                whiteDeck[i-5] = CardDatabase.whiteList[i-5];
            }   
        }
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        staticRedDeck = redDeck;
        staticWhiteDeck = whiteDeck;
    }

    IEnumerator StartGame() {
        for (int i = 0; i < 3; i++) {
            yield return new WaitForSeconds(1);
            GameObject card = Instantiate(redCardToHand, transform.position, transform.rotation);
            card.name = "redCard " + i;
        }

        for (int i = 0; i < 4; i++) {
          yield return new WaitForSeconds(1);
           GameObject card = Instantiate(whiteCardToHand, transform.position, transform.rotation);
           card.name = "whiteCard " + i;
        }
    }
}
