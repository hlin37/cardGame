using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
    public static List<Clickable> allSelectables = new List<Clickable>();

    [SerializeField]
    private Image img;

    [SerializeField]
    private Color selectBlue;

    [SerializeField]
    private Color unSelectRed;

    [SerializeField]
    private Color unSelectWhite;

    private GameManagerScript gameManagerScript;

    void Awake() {
        allSelectables.Add(this);
        img = this.GetComponent<Image>();
    }

    void Start() {
        GameObject gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();

    }

    public void OnPointerClick(PointerEventData eventData) {
        OnSelect(eventData);
    }

    public void OnSelect(BaseEventData eventData) {
        if (gameManagerScript.selectingWhiteCards) {
            if (this.gameObject.name.Contains("Red")) {
                img.color = unSelectRed;
            }
            else if (this.gameObject.name.Contains("White")) {
                int index = int.Parse(this.gameObject.transform.parent.name[6].ToString());
                if (!gameManagerScript.clicked.ContainsKey(index)) {
                    gameManagerScript.clicked.Add(index, new List<GameObject>());
                }
                if (gameManagerScript.clicked[index].Count == 2 && !gameManagerScript.clicked[index].Contains(this.gameObject)) {
                    gameManagerScript.clicked[index][0].GetComponent<Clickable>().OnDeselect(eventData);
                    gameManagerScript.clicked[index].RemoveAt(0);
                    gameManagerScript.clicked[index].Add(this.gameObject);
                    img.color = selectBlue;
                }
                else if (gameManagerScript.clicked[index].Contains(this.gameObject)) {
                    gameManagerScript.clicked[index].Remove(this.gameObject);
                    img.color = unSelectWhite;
                }
                else if (!gameManagerScript.clicked[index].Contains(this.gameObject)) {
                    gameManagerScript.clicked[index].Add(this.gameObject);
                    img.color = selectBlue;
                }
            }
        }
        else {
            if (this.gameObject.name.Contains("White")) {
                img.color = unSelectWhite;
            }
            else if (this.gameObject.name.Contains("Red")) {
                int index = int.Parse(this.gameObject.transform.parent.name[6].ToString());
                if (gameManagerScript.clicked[index].Count == 1 && !gameManagerScript.clicked[index].Contains(this.gameObject)) {
                    gameManagerScript.clicked[index][0].GetComponent<Clickable>().OnDeselect(eventData);
                    gameManagerScript.clicked[index].RemoveAt(0);
                    gameManagerScript.clicked[index].Add(this.gameObject);
                    img.color = selectBlue;
                }
                else if (gameManagerScript.clicked[index].Contains(this.gameObject)) {
                    gameManagerScript.clicked[index].Remove(this.gameObject);
                    img.color = unSelectRed;
                }
                else if (!gameManagerScript.clicked[index].Contains(this.gameObject)) {
                    gameManagerScript.clicked[index].Add(this.gameObject);
                    img.color = selectBlue;
                }
            }
        }
    }

    public void OnDeselect(BaseEventData eventData) {
        if (this.gameObject.name.Contains("White")) {
            img.color = unSelectWhite;
        }
        else {
            img.color = unSelectRed;
        }
    }

    public void returntoWhite() {
        img.color = unSelectWhite;
    }
}
