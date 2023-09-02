using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Photon.Pun;

public class RoomCreatePopup : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField RoomTitle;
    [SerializeField] private InputField passwordcreate;
    [SerializeField] private InputField maxPlayerInput;
    public GameObject BtnCheckedoff;
    public GameObject BtnCheckedon;
    [SerializeField] private LobbyManager theLobby;
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    public void PasswordOn()
    {
        passwordcreate.interactable = true;
        BtnCheckedon.SetActive(true);
        BtnCheckedoff.SetActive(false);
    }

    public void PasswordOff()
    {
        passwordcreate.interactable = false;
        passwordcreate.text = null;
        BtnCheckedoff.SetActive(true);
        BtnCheckedon.SetActive(false);
    }

    public void CancelBtn()
    {
        RoomTitle.text = null;
        passwordcreate.text = null;
        this.gameObject.SetActive(false);
    }

    public void CreateBtn()
    {
        int dump = System.Convert.ToInt32(maxPlayerInput.text);
        if (dump <= 2)
            dump = 2;
        if (dump >= 20)
            dump = 20;
        theLobby.OnRoomCreate(RoomTitle.text, passwordcreate.text, dump);
        
    }

    public void OnValueChangedMaxPlayer()
    {

    }

    public void OnEndEditMaxPlayer(string value)
    {
        // maxPlayerInput.text = "sad";
        //int playerInput = int.Parse(maxPlayerInput.text);
        int playerInput = 0;
        bool b = int.TryParse(value, out playerInput);
        if(b)
        {
            if(playerInput > 20)
            {
                maxPlayerInput.text = "20";
            }
            else if(playerInput < 2) {
                maxPlayerInput.text = "2";
            }
        }
        else { maxPlayerInput.text = "2"; }
    }
}
