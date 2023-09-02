using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Newtonsoft.Json;

public class ManagerLobby : MonoBehaviourPunCallbacks
{
    //InputFields text;
    PlayerController playerController;
    public GameObject roomPrefab;
    public Transform roomListParent;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnRoomCreate()
    {
        RoomOptions roomOptions = new RoomOptions();
        ExitGames.Client.Photon.Hashtable initialProps = new ExitGames.Client.Photon.Hashtable() { { "isReady", false }, { "passworld", "" } };
        roomOptions.CustomRoomProperties =  initialProps;
        PhotonNetwork.CreateRoom("abc", roomOptions);
        
    }

    public void OnRoomJoin()
    {
        PhotonNetwork.JoinRoom("abc");
    }

    #region Photon
    public override void OnConnected()
    {
        base.OnConnected();
        Debug.Log("OnConnected");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("OnLeftRoom");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        Debug.Log("newMasterClient : " + newMasterClient.NickName);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        //playerController.name = text.name;
        Debug.Log("OnCreatedRoom");
        Debug.Log(PhotonNetwork.NickName);
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("OnJoinedLobby");
    }

    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        Debug.Log("OnLeftLobby");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("OnDisconnected" + cause);

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        Debug.Log("OnRoomListUpdate" + JsonConvert.SerializeObject(roomList));

        for (int i = 0; i < roomList.Count; i++)
        {
            GameObject go = Instantiate(roomPrefab, roomListParent);
            PrefabRoom room = go.GetComponent<PrefabRoom>();
            room.SetRoomInfo(roomList[i]);
        }
        //Debug.Log("OnRoomListUpdate" + JsonConvert.SerializeObject(roomList));
        //  List<RoomInfo> a = JsonConvert.DeserializeObject<List<RoomInfo>>("");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        //playerController.name = text.name;
        Debug.Log("OnJoinedRoom" + PhotonNetwork.NickName);
        
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("OnPlayerEnteredRoom " + newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log("OnPlayerLeftRoom " + otherPlayer.NickName);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("OnConnectedToMaster");

        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        Debug.Log("OnRoomPropertiesUpdate: " + JsonConvert.SerializeObject(propertiesThatChanged));
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        Debug.Log("OnPlayerPropertiesUpdate: " + targetPlayer.NickName + " : " + JsonConvert.SerializeObject(changedProps));
    }
    #endregion
}