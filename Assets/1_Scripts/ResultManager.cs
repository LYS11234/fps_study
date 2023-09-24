using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviourPunCallbacks
{
    #region Scripts & Objects
    [Header("Scripts & Objects")]
    public Text winText;
    public Text loseText;
    public Text drawText;
    #endregion
    #region Variables
    [Header("Variables")]
    public string playerTeam;
    public int team1Score;
    public int team2Score;
    #endregion

    #region Codes
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        ExitGames.Client.Photon.Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;
        team1Score = System.Convert.ToInt32(cp["Team1 Score"]);
        team2Score = System.Convert.ToInt32(cp["Team2 Score"]);
        GetWinTeam();
        cp["isPlayed"] = "true";
        PhotonNetwork.CurrentRoom.SetCustomProperties(cp);
    }
    private void GetWinTeam()
    {
        ExitGames.Client.Photon.Hashtable initialProps = PhotonNetwork.LocalPlayer.CustomProperties;
        playerTeam = initialProps["Team"].ToString();
        if(team1Score > team2Score)
        {
            if (playerTeam == "1")
            {
                winText.gameObject.SetActive(true);
                loseText.gameObject.SetActive(false);
                drawText.gameObject.SetActive(false);
            }
            else
            {
                winText.gameObject.SetActive(false);
                loseText.gameObject.SetActive(true);
                drawText.gameObject.SetActive(false);
            }
        }
        else if (team1Score < team2Score)
        {
            if (playerTeam == "1")
            {
                winText.gameObject.SetActive(false);
                loseText.gameObject.SetActive(true);
                drawText.gameObject.SetActive(false);
            }
            else
            {
                winText.gameObject.SetActive(true);
                loseText.gameObject.SetActive(false);
                drawText.gameObject.SetActive(false);
            }
        }
        else
        {
            winText.gameObject.SetActive(false);
            loseText.gameObject.SetActive(false);
            drawText.gameObject.SetActive(true);
        }
    }

    
    public void GoToRoom()
    {
        PhotonNetwork.LoadLevel("2_Room"); 
    }

    public void GoToLobby()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (PhotonNetwork.PlayerList.Length > 1)
                PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerList[1]);
        }
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("1_Lobby");
    }
    #endregion

    #region Photon
    
    #endregion
}
