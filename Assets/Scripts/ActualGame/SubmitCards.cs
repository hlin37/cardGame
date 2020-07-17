using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitCards : MonoBehaviour
{
    public GameObject button;

    private GameManagerScript gameManagerScript;

    private GameObject dropZone;

    private GameObject whiteHand;

    private GameObject redHand;

    private string _whiteHand = "WhiteHand";

    private string _redHand = "RedHand";

    private string _dropZone = "DropZone";

    private string _player = "Player";

    [SerializeField]
    private Color unSelectWhite;

    public void OnClick() {
        int index = int.Parse(button.transform.parent.name[6].ToString());
        print(index);
        if (gameManagerScript.clicked[index].Count != 2) {
            print("Not enough Cards");
        }
        else {
            dropZone = GameObject.Find(_player + index.ToString() + _dropZone);
            whiteHand = GameObject.Find(_player + index.ToString() + _whiteHand);
            foreach (GameObject card in gameManagerScript.clicked[index]) {
                for (int i = 0; i < whiteHand.transform.childCount; i++) {
                    if (whiteHand.transform.GetChild(i).GetComponent<WhiteCard>().uniqueCardNumber.Equals(card.GetComponent<WhiteCard>().uniqueCardNumber)) {
                        Transform child = whiteHand.transform.GetChild(i);
                        child.SetParent(dropZone.transform, false);
                        card.GetComponent<Image>().color = unSelectWhite;
                    }
                }
            }
            dropZone.GetComponent<DropZone>().placedCards = true;
            gameManagerScript.clicked[index].Clear();
        }
    }

    void Start() {
        GameObject gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
    }
}
