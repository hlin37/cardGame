using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TestConnect : MonoBehaviourPunCallbacks
{
    private void Start() {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        print("Connected to Server");
        print(PhotonNetwork.LocalPlayer.NickName);
        if (!PhotonNetwork.InLobby) {
            PhotonNetwork.JoinLobby();
        }
    }   

    public override void OnDisconnected(DisconnectCause cause) {
        print("Disconnected " + cause.ToString());
    }
}
