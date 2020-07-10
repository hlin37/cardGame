using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public List<GameObject> listOfCards = new List<GameObject>();

    void Start() {
    }

    public void addCard(GameObject child) {
        if (!listOfCards.Contains(child)) {
            listOfCards.Add(child);
        }
    }

    public void removeCard(GameObject child) {
        if (listOfCards.Contains(child)) {
            listOfCards.Remove(child);
        }
    }

    public int returnSize() {
        return listOfCards.Count;
    }
}
