using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitCards : MonoBehaviour
{
    // public List<Clickable> listOfCard = new List<Clickable>();

    public List<GameObject> list = new List<GameObject>();
    
    public GameObject dropZone;

    public GameObject whiteHand;

    public GameObject redHand;

    private string _whiteHand = "WhiteHand";

    private string _redHand = "RedHand";

    private string _dropZone = "DropZone";

    private string _player = "Player";

    public Dictionary<int, List<GameObject>> cards = new Dictionary<int, List<GameObject>>();

    public void OnClick() {
        getObjects();
        //dropZone = GameObject.Find("DropZone");
        //whiteHand = GameObject.Find("WhiteHand");
        //redHand = GameObject.Find("RedHand");
        if (list.Count == 2) {
            foreach (KeyValuePair<int, List<GameObject>> whiteCards in cards) {
                int key = whiteCards.Key;
                List<GameObject> car = whiteCards.Value;
                foreach (GameObject card in car) {
                    dropZone = GameObject.Find(_player + key.ToString() + _dropZone);
                    whiteHand = GameObject.Find(_player + key.ToString() + _whiteHand);
                    Transform child = whiteHand.transform.Find(card.name);
                    child.SetParent(dropZone.transform, false);
                }
            }
        }
        else if (list.Count == 1) {
            foreach (KeyValuePair<int, List<GameObject>> whiteCards in cards) {
                int key = whiteCards.Key;
                List<GameObject> car = whiteCards.Value;
                foreach (GameObject card in car) {
                    dropZone = GameObject.Find(_player + key.ToString() + _dropZone);
                    redHand = GameObject.Find(_player + key.ToString() + _redHand);
                    Transform child = whiteHand.transform.Find(card.name);
                    child.SetParent(dropZone.transform, false);
                }
            }
        }
        else {
            Debug.Log("Not Enough Cards");
        }
    }

    public void getObjects() {
        // listOfCard = Clickable.current;
        foreach (Clickable stuff in Clickable.current) {
            int index = System.Convert.ToInt32(stuff.gameObject.name[6]);
            print(stuff.gameObject.name);
            if (cards[index] == null) {
                cards.Add(index, new List<GameObject>());
            }
            cards[index].Add(stuff.gameObject);
        }
        // if (listOfCard.Count == 1) {
        //     for (int i = 0; i < 1; i++) {
        //         list.Add(listOfCard[i].gameObject);
        //     }
        // }
        // else if (listOfCard.Count == 2) {
        //     for (int i = 0; i < 2; i++) {
        //         list.Add(listOfCard[i].gameObject);
        //     }
        // }
    }
}
