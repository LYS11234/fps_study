using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon;
using Unity.VisualScripting;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class JoinRoomPopup : MonoBehaviour
{
    [SerializeField]
    private LobbyManager theLobby;
    [SerializeField]
    private Text Title;
    [SerializeField]
    private InputField InputPassword;


    [SerializeField]
    public PrefabRoom prefabRoom;


    void Start()
    {
        Title.text = prefabRoom.roomName.text;
    }

    public void Open(PrefabRoom prefabRoom)
    {
        this.prefabRoom = prefabRoom;
        gameObject.SetActive(true);
    }

    public void JoinRoom()
    {
        RoomInfo roomInfo = this.prefabRoom.roomInfo;
        Hashtable hashtable = this.prefabRoom.roomInfo.CustomProperties;
        string pw = hashtable["password"].ToString();
        theLobby.OnRoomJoin(Title.text, InputPassword.text, pw);
        
        
    }

    
}
