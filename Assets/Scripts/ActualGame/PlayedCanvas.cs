using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayedCanvas : MonoBehaviour
{

    private GameObject singleCanvas;

    [SerializeField]
    private GameObject playedCanvas;

    [SerializeField]
    private PhotonView canvasOwner;

    private GameManagerScript gameManagerScript;

    private GameObject playingCanvas;

    private int stopper = 0;

    public void moveYourCards() {
        GameObject[] dropZones = GameObject.FindGameObjectsWithTag("DropZone");
        GameObject[] playedCanvases = GameObject.FindGameObjectsWithTag("PlayedCanvas");
        GameObject[] listOfCameras = GameObject.FindGameObjectsWithTag("MainCamera");
        int number = 0;
        foreach (GameObject camera in listOfCameras) {
            number = camera.GetComponent<PhotonView>().Owner.ActorNumber;
        }
        foreach (GameObject canvas in playedCanvases) {
            if (canvas.name.Contains(number.ToString())) {
                playingCanvas = canvas;
            }
        }
        while (dropZones[number - 1].transform.childCount > 1) {
            GameObject card = dropZones[number - 1].gameObject.transform.GetChild(1).gameObject;
            card.transform.SetParent(playingCanvas.transform);
        }
    }

    public void moveOtherCards() {
        GameObject[] whiteCards = GameObject.FindGameObjectsWithTag("WhiteCard");
        GameObject[] canvases = GameObject.FindGameObjectsWithTag("PlayedCanvas");
        for (int i = 0; i < gameManagerScript.cardsChosen.Count; i++) {
            foreach (GameObject card in whiteCards) {
                if (card.GetComponent<WhiteCard>().uniqueCardNumber == gameManagerScript.cardsChosen[i]) {
                    if (!card.transform.parent.name.Contains("Played")) {
                        int index = int.Parse(card.transform.parent.name[6].ToString());
                        foreach (GameObject canvas in canvases) {
                            int anotherIndex = int.Parse(canvas.name[6].ToString());
                            if (index == anotherIndex) {
                                card.transform.SetParent(canvas.transform);
                            }
                        }
                    }
                }
            }
        }
    }

    public void moveAllCards() {
        List<int> list = gameManagerScript.listPlayers;
        GameObject[] playedCanvases = GameObject.FindGameObjectsWithTag("PlayedCanvas");
        GameObject[] listOfCameras = GameObject.FindGameObjectsWithTag("MainCamera");
        int number = 0;
        foreach (GameObject camera in listOfCameras) {
            number = camera.GetComponent<PhotonView>().Owner.ActorNumber;
        }
        int attackingNumber = 0;
        for (int i = 1; i < list.Count; i++) {
            if (number == list[i]) {
                if (i == list.Count - 1) {
                    attackingNumber = list[1];
                }
                else {
                    attackingNumber = list[i + 1];
                }
            }
        }
        foreach (GameObject canvas in playedCanvases) {
            if (canvas.name.Contains(attackingNumber.ToString())) {
                playingCanvas = canvas;
            }
        }
        GameObject dropZone = findDropZone(number);
        while (dropZone.transform.childCount > 0) {
            GameObject card = dropZone.transform.GetChild(0).gameObject;
            card.transform.SetParent(playingCanvas.transform);
        }
    }

    private void destroyCanvas() {
        GameObject[] listOfCanvases= GameObject.FindGameObjectsWithTag("PlayedCanvas");
        List<Player> oneCanvas = new List<Player>();
        for (int i = 0; i < listOfCanvases.Length; i++) {
            if (!oneCanvas.Contains(listOfCanvases[i].GetComponent<PhotonView>().Owner)) {
                oneCanvas.Add(listOfCanvases[i].GetComponent<PhotonView>().Owner);
            }
            else {
                if (listOfCanvases[i].GetComponent<PhotonView>().IsMine) {
                    PhotonNetwork.Destroy(listOfCanvases[i]);
                }
            }
        }
    }

    private void nameCanvas() {
        Player player = this.canvasOwner.Owner;
        playedCanvas.name = "Player" + player.ActorNumber + "Played Canvas";
        playedCanvas.tag = "PlayedCanvas";
    }

    private void moveCanvas() {
        Vector3 position = playedCanvas.transform.position;
        Vector3 newPosition = new Vector3(position.x, position.y, 0);
        playedCanvas.transform.position = newPosition;
    }

    // Move attacked white cards to your canvas and choose your red card.
    private void chooseRedCards() {
        List<int> list = gameManagerScript.listPlayers;
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        int number = 0;
        foreach (GameObject camera in cameras) {
            number = camera.GetComponent<PhotonView>().Owner.ActorNumber;
        }
        int attackingNumber = 0;
        if (number != list[0]) {
            attackingNumber = list.IndexOf(number);
            if (attackingNumber == list.Count - 1) {
                attackingNumber = list[1];
            }
            else {
                attackingNumber = list[attackingNumber + 1];
            }
        }
        GameObject[] canvases = GameObject.FindGameObjectsWithTag("PlayedCanvas");
        GameObject dropZone = findDropZone(number);
        //Debug.Log("This is my attacking number" + attackingNumber + "This should be my dropZone " + number + "name: " + dropZone.name);
        foreach (GameObject canvas in canvases) {
            if (canvas.name.Contains(attackingNumber.ToString())) {
                Debug.Log(canvas.name);
                while (canvas.transform.childCount > 0) {
                    GameObject card = canvas.transform.GetChild(0).gameObject;
                    card.transform.SetParent(dropZone.transform);
                }
            }
        }
    }

    private GameObject findDropZone(int number) {
        GameObject[] dropZones = GameObject.FindGameObjectsWithTag("DropZone");
        int holder = 0;
        for (int i = 0; i < dropZones.Length; i++) {
            if (dropZones[i].name.Contains(number.ToString())) {
                holder = i;
            }
        }
        return dropZones[holder];
    }

    void Awake() {
        singleCanvas = GameObject.Find("SingleCanvas");
        GameObject gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
        nameCanvas();
        playedCanvas.transform.SetParent(singleCanvas.transform);
        moveCanvas();
    }

    void Start() {
        destroyCanvas();
        moveYourCards();
        moveOtherCards();
    }

    void Update() {
        if (gameManagerScript.choosingRedCards && stopper == 0) {
            stopper = 1;
            chooseRedCards();
        }
        else if (gameManagerScript.finalSelection && stopper == 1) {
            stopper = 2;
            moveAllCards();
            //moveOtherCards();
        }
    }
}
