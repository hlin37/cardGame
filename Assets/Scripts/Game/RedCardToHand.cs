using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCardToHand : MonoBehaviour
{
    public GameObject Hand;

    public GameObject It;
    
    void Awake() {  
        Hand = GameObject.Find("Hand");
        It.transform.SetParent(Hand.transform);
        It.transform.localScale = Vector3.one;
        It.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    public void changeParent() {
        Hand = GameObject.Find("DropZone");
        It.transform.SetParent(Hand.transform);
        It.transform.localScale = Vector3.one;
        It.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    // // Update is called once per frame
    // void Update()
    // {
    //     Hand = GameObject.Find("Hand");
    //     It.transform.SetParent(Hand.transform);
    //     It.transform.localScale = Vector3.one;
    //     It.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    //     // It.transform.eulerAngles = new Vector3(25,0,0);
    // }
}
