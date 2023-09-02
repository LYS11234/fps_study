using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class PrefabRoom : MonoBehaviour
{
    // crtl + .
    public RoomInfo roomInfo;
    public Text roomName;
    public Text playerCount;

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        this.roomInfo = roomInfo;

        string roomName = roomInfo.Name;
        string playerCount = $"{roomInfo.PlayerCount} / {roomInfo.MaxPlayers}";

        this.roomName.text = roomName;
        this.playerCount.text = playerCount;
    }

    public void OnClick()
    {
        CanvasManager.Instance.joinRoomPopup.Open(this);
    }
}
