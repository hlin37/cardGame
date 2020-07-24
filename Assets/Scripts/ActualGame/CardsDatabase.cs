using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CardsDatabase : MonoBehaviour
{

    public List<Card> redList = new List<Card>();

    public List<Card> whiteList = new List<Card>();

    private string path = "Assets/";

    void Start() {
        for (int typeOfCard = 0; typeOfCard < 2; typeOfCard++) {
            if (typeOfCard == 0) {
                string[] linesOfText = readAllLines("red");
                for (int index = 0; index < 15; index++) {
                    Card redCard = new Card();
                    redCard.setText(linesOfText, index);
                    redCard.setNumber(index);
                    redList.Add(redCard);
                }
            }
            else {
                string[] linesOfText = readAllLines("white");
                for (int index = 15; index < 37; index++) {
                    Card whiteCard = new Card();
                    whiteCard.setText(linesOfText, index);
                    whiteCard.setNumber(index);
                    whiteList.Add(whiteCard);
                }
            }
        }
    }

    public string[] readAllLines(string color) {
        string[] lines;
        if (color.Equals("red")) {
            lines = File.ReadAllLines(path + "red.txt");
        }
        else {
            lines = File.ReadAllLines(path + "white.txt");
        }
        return lines;
    }
}
