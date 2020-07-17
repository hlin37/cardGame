using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SingleCanvas : MonoBehaviour 
{
    private GameObject gameBoard;
    
    private GameObject dropZone;

    private List<GameObject> listOfDropZones;

    public GameObject playedCanvas;

    private GameObject _playedCanvas;

    private GameObject singleCanvas;

    public void dropZonetoCanvas() {
        for (int id = 0; id < gameBoard.transform.childCount; id++) {
            dropZone = gameBoard.transform.GetChild(id).gameObject.transform.GetChild(1).gameObject;
            listOfDropZones.Add(dropZone);
        }
    }

    void Awake() {
        gameBoard = GameObject.Find("GameBoard");
        singleCanvas = GameObject.Find("SingleCanvas");
        //dropZonetoCanvas();
    }

    public void moveToCanvas() {
        foreach (GameObject drop in listOfDropZones) {
            if (!dropZone.GetComponent<DropZone>().placedCards) {
                return;
                
            }
            else {
                moveCardstoPlayedCanvas(drop);
            }
        }
    }

    // playedCanvas is the small thing that holds the cards
    public void moveCardstoPlayedCanvas(GameObject zone) {
        _playedCanvas = Instantiate(playedCanvas, transform.position, transform.rotation);
        movePlayedCanvasToSingleCanvas();
        for (int i = 0; i < 2; i++) {
            Transform child = zone.transform.GetChild(i + 1).gameObject.transform;
            child.SetParent(_playedCanvas.transform);
            child.gameObject.GetComponent<CardFlipper>().flipToBack();
        }
    }

    public void movePlayedCanvasToSingleCanvas() {
        _playedCanvas.transform.SetParent(singleCanvas.transform);
        _playedCanvas.transform.localScale = Vector3.one;
        _playedCanvas.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
