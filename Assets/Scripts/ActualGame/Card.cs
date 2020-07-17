using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Random=System.Random;
[System.Serializable]

public class Card {
    
    public string description;

    public int num;

    public Card() {
    }

    public void setText(string[] list, int index) {
        description = list[index];
    }

    public void setNumber(int number) {
        num = number;
    }
}
