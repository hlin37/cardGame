﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRoomCanvas : MonoBehaviour
{
    [SerializeField]
    private PlayerListingsMenu _playerListingMenu;

    [SerializeField]
    private LeaveRoomMenu _leaveRoomMenu;

    public LeaveRoomMenu LeaveRoomMenu {
        get {
            return _leaveRoomMenu;
        }
    }
    private RoomsCanvases _roomsCanvases;
    public void FirstInitialize(RoomsCanvases canvases) {
        _roomsCanvases = canvases;
        _playerListingMenu.FirstInitalize(canvases);
        _leaveRoomMenu.FirstInitialize(canvases);

    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
