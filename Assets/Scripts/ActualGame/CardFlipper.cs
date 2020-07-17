using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipper : MonoBehaviour
{
    public Sprite whiteCardBack;

    public Sprite redCardBack;
    public Image image;

    public Text text;

    public Color colorWhite;

    public Color colorRed;

    public void flipToFront() {
        image.sprite = null;
        if (this.gameObject.name.Contains("Red")) {
            image.color = colorRed;
        }
        text.enabled = true;
    }

    public void flipToBack() {
        if (this.gameObject.name.Contains("White")) {
            image.sprite = whiteCardBack;
        }
        else {
            image.sprite = redCardBack;
        }
        text.enabled = false;
        image.color = colorWhite;
    }

    void Start() {
        image = GetComponent<Image>();
        text = this.gameObject.transform.GetChild(0).GetComponent<Text>();
    }
}
