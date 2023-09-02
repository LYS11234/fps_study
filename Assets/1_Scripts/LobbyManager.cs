using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;
using Photon;
using UnityEngine.UIElements;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private RoomCreatePopup roomCreatePopup;
    [SerializeField]
    private JoinRoomPopup joinRoomPopup;
    [SerializeField]
    private PrefabRoom prefabRoom;
    public GameObject RoomJoinMenu;
    public GameObject roomPrefab;
    public Transform roomListParent;
    [SerializeField]
    private GameObject RoomCreateMenu;

  


    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
        //PhotonNetwork.automaticallySyncScene = true;
    }


    #region Base
    public void OnRoomCreate(string _roomTitle, string _roomPassword = "", int _maxPlayer = 20)
    {
        RoomOptions roomOptions = new RoomOptions();
        string Title = _roomTitle;
        ExitGames.Client.Photon.Hashtable initialProps = new ExitGames.Client.Photon.Hashtable() { { "password", _roomPassword }, { "MapName", "3_Map1" } };
        roomOptions.CustomRoomProperties = initialProps;
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "password", "MapName"};
        roomOptions.MaxPlayers = _maxPlayer;
        PhotonNetwork.CreateRoom(Title, roomOptions);
        PhotonNetwork.LoadLevel("2_Room");
    }
    
    public void OnRoomJoin(string _roomTitle, string _InputPassword, string _Password)
    {
        
        if (_InputPassword == _Password)
        {
           bool isSuccess =  PhotonNetwork.JoinRoom(_roomTitle);
            if(isSuccess)
            {
                
                PhotonNetwork.LoadLevel("2_Room");
            }else
            {
                // d알림창
                return;
            }
        }
        else
        {
            // 알림창
            return;
        }
    }
    #endregion

    #region photon
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

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        Debug.Log("OnRoomListUpdate" + JsonConvert.SerializeObject(roomList));

        //if (roomList.Find(e => e = prefabRoom.name))
        //{

        //}
        for (int i = 0; i < roomList.Count; i++)
        {
            GameObject go = Instantiate(roomPrefab, roomListParent);
            PrefabRoom room = go.GetComponent<PrefabRoom>();
            
            room.SetRoomInfo(roomList[i]);
            
        }
        //Debug.Log("OnRoomListUpdate" + JsonConvert.SerializeObject(roomList));
        //  List<RoomInfo> a = JsonConvert.DeserializeObject<List<RoomInfo>>("");
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

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        //playerController.name = text.name;
        Debug.Log("LobbyManager OnJoinedRoom" + PhotonNetwork.NickName);

        
        //PhotonNetwork.LoadLevel("2_Room");
    }

    //public override void OnPlayerEnteredRoom(Player newPlayer)
    //{
    //    base.OnPlayerEnteredRoom(newPlayer);
    //    Debug.Log("OnPlayerEnteredRoom " + newPlayer.NickName);
    //}

    //public override void OnPlayerLeftRoom(Player otherPlayer)
    //{
    //    base.OnPlayerLeftRoom(otherPlayer);
    //    Debug.Log("OnPlayerLeftRoom " + otherPlayer.NickName);
    //}

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

    public void CreateRoomBtn()
    {
        RoomCreateMenu.gameObject.SetActive(true);
        roomCreatePopup.BtnCheckedon.SetActive(false);
        roomCreatePopup.BtnCheckedoff.SetActive(true);
    }

    public void JoinRoom()
    {
        
    }
}