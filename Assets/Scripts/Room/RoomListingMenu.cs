using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;

    [SerializeField]
    private RoomListing _roomListing;

    private RoomsCanvases _roomsCanvases;
    public void FirstInitialize(RoomsCanvases canvases) {
        _roomsCanvases = canvases;
    }

    private List<RoomListing> _listings = new List<RoomListing>();

    public override void OnJoinedRoom() {
        _roomsCanvases.CurrentRoomCanvas.Show();
        _content.DestroyChildren();
        _listings.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        foreach (RoomInfo info in roomList) {
            // Removed from room list
            if (info.RemovedFromList) {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index != -1) {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
            // Added to room list
            else {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index == -1) {
                    RoomListing listing = Instantiate(_roomListing, _content);
                    if (listing != null) {
                        listing.SetRoomInfo(info);
                        _listings.Add(listing);
                    }
                }
            }
        }
    }
}
