using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCardToHand : MonoBehaviour
{
    public GameObject Hand;

    public GameObject It;

    void Awake() {
        Hand = GameObject.Find("whiteHand");
        It.transform.SetParent(Hand.transform);
        It.transform.localScale = Vector3.one;
        It.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        // It.transform.eulerAngles = new Vector3(25,0,0);
    }
}
