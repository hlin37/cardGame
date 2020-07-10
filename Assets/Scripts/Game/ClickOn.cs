using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class ClickOn : MonoBehaviour
{
    [SerializeField]
    private Color red;
    [SerializeField]

    private Color blue;

    public GameObject card;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ClickMe() {
        Debug.Log("hello");
    }
}
