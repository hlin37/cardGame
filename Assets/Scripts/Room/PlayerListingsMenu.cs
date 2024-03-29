﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerListingsMenu : MonoBehaviourPunCallbacks
{
     [SerializeField]
    private Transform _content;

    [SerializeField]
    private PlayerListing _playerListing;

    [SerializeField]
    private Text _readyUpText;

    private bool _ready = false;

    private List<PlayerListing> _listings = new List<PlayerListing>();

    private RoomsCanvases _roomsCanvases;
    
    public void FirstInitalize(RoomsCanvases canvases) {
        _roomsCanvases = canvases;
    }

    public override void OnEnable() {
        base.OnEnable();
        SetReadyUp(false);
        getCurrentRoomPlayers();
    }

    public override void OnDisable() {
        base.OnDisable();
        for (int i = 0; i < _listings.Count; i++) {
            Destroy(_listings[i].gameObject);
        }
        _listings.Clear();
    }

    public override void OnLeftRoom() {
        _content.DestroyChildren();
    }

    private void getCurrentRoomPlayers() {
        if (!PhotonNetwork.IsConnected) {
            return;
        }
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null) {
            return;
        }
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players) {
            addPlayerListing(playerInfo.Value);
        }
    }

    private void addPlayerListing(Player player) {
        int index = _listings.FindIndex(x => x.Player == player);
        if (index != -1) {
            _listings[index].SetPlayerInfo(player);
        }
        else {
            PlayerListing listing = Instantiate(_playerListing, _content);
            if (listing != null) {
                listing.SetPlayerInfo(player);
                _listings.Add(listing);
            }
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer) {
        addPlayerListing(newPlayer);
    }

    public void OnClick_StartGame() {
        if (PhotonNetwork.IsMasterClient) {
            // for (int i = 0; i < _listings.Count; i++) {
            //     if (_listings[i].Player != PhotonNetwork.LocalPlayer) {
            //         if (!_listings[i].Ready) {
            //             return;
            //         }
            //     }
            // }
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1);
        }
    }

    private void SetReadyUp(bool state) {
        _ready = state;
        if (_ready) {
            _readyUpText.text = "R";
        }
        else {
            _readyUpText.text = "N";
        }
    }

    public void OnClick_ReadyUp() {
        if (!PhotonNetwork.IsMasterClient) {
            SetReadyUp(!_ready);
            base.photonView.RPC("RPC_ChangeReadyState", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, _ready);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient) {
        _roomsCanvases.CurrentRoomCanvas.LeaveRoomMenu.OnClick_LeaveRoom();
    }

    [PunRPC]
    private void RPC_ChangeReadyState(Player player, bool ready) {
        int index = _listings.FindIndex(x => x.Player == player);
        if (index != -1) {
            _listings[index].Ready = ready;
        }
    }
}
