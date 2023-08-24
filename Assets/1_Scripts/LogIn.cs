using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


public class LogIn : MonoBehaviourPunCallbacks
{
    InputFields text;
    public string Lobby;
    ManagerLobby managerLobby;
    void Start()
    {
        text = GetComponent<InputFields>();
        managerLobby = GetComponent<ManagerLobby>();
    }

    // Update is called once per frame
    private void Login()
    {
        PhotonNetwork.NickName = text.name;
        SceneManager.LoadScene("SampleScene");
    }
}
