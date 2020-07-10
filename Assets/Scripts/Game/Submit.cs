using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submit : MonoBehaviour
{
    public List<Draggable> listOfCard = new List<Draggable>();

    public List<GameObject> list = new List<GameObject>();
    
    public GameObject dropZone;

    public GameObject whiteHand;

    public GameObject hand;

    private GameObject card1;

    private GameObject card2;



    public void OnClick() {
        getObjects();
        dropZone = GameObject.Find("DropZone");
        whiteHand = GameObject.Find("whiteHand");
        hand = GameObject.Find("Hand");
        if (list.Count == 1) {
            for (int i = 0; i < 1; i++) {
                Transform child = hand.transform.Find(list[i].name);
                child.SetParent(dropZone.transform, false);
            }
        }
        else if (list.Count == 2) {
            for (int i = 0; i < 2; i++) {
                Transform child = whiteHand.transform.Find(list[i].name);
                child.SetParent(dropZone.transform, false);
            }
        }
        else {
            Debug.Log("Not Enough Cards");
        }
    }

    public void getObjects() {
        listOfCard = Draggable.current;
        if (listOfCard.Count == 1) {
            for (int i = 0; i < 1; i++) {
                list.Add(listOfCard[i].gameObject);
            }
        }
        else if (listOfCard.Count == 2) {
            for (int i = 0; i < 2; i++) {
                list.Add(listOfCard[i].gameObject);
            }
        }
    }
}
