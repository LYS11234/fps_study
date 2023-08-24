using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


public class LogIn : MonoBehaviourPunCallbacks
{
    InputFields text;
    void Start()
    {
        text = FindObjectOfType<InputFields>();
    }

    // Update is called once per frame
    public void Login()
    {
        PhotonNetwork.NickName = text.name;
        SceneManager.LoadScene("SampleScene");
        Debug.Log("A");
    }
}
