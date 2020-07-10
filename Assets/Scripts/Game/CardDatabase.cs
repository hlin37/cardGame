using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CardDatabase : MonoBehaviour {

    public static List<Card> redList = new List<Card>();

    public static List<Card> whiteList = new List<Card>();

    void Awake() {
        for (int typeOfCard = 0; typeOfCard < 2; typeOfCard++) {
            if (typeOfCard == 0) {
                string[] linesOfText = readAllLines("red");
                for (int index = 0; index < 10; index++) {
                    Card redCard = new Card();
                    redCard.setText(linesOfText, index);
                    redList.Add(redCard);
                }
            }
            else {
                string[] linesOfText = readAllLines("white");
                for (int index = 0; index < 20; index++) {
                    Card whiteCard = new Card();
                    whiteCard.setText(linesOfText, index);
                    whiteList.Add(whiteCard);
                }
            }
        }
    }

    public string[] readAllLines(string color) {
        string[] lines;
        if (color.Equals("red")) {
            lines = File.ReadAllLines("red.txt");
        }
        else {
            lines = File.ReadAllLines("white.txt");
        }
        return lines;
    }
}
