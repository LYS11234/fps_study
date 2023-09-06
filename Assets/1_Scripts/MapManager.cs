using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using ExitGames.Client.Photon;
using Unity.VisualScripting;

[System.Serializable]
public struct TeamMaterial
{
    public Material head;
    public Material legs;
    public Material torso;
}

public class MapManager : MonoBehaviourPunCallbacks
{
    public static MapManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(instance);
        }
    }

    #region Scripts & Objects
    [Header("Scrips & Objects")]
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private DatabaseManager databaseManager;
    [SerializeField]
    private Transform team1Transform;
    [SerializeField]
    private Transform team2Transform;
    [SerializeField]
    private PlayerController playerController;
    #endregion

    #region Player Materials
    public TeamMaterial aTeam;
    public TeamMaterial bTeam;
    #endregion

    #region Codes
    private void Start()
    {
        Transform tf = transform.transform;
        if (PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString() == "1")
        {
            tf = team1Transform.transform;
        }
        if (PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString() == "2")
        {
            tf = team2Transform.transform;
        }
        GameObject my =  PhotonNetwork.Instantiate("player", tf.transform.position, Quaternion.identity);
        playerController = my.GetComponent<PlayerController>();
        Transform tfCam = Camera.main.transform;
        tfCam.SetParent(my.transform);
        tfCam.localPosition = new Vector3(0, 2, 0);
        playerController.cam = tfCam;

        if (PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString() == "1")
        {
            playerController.ChangeMaterial(aTeam.head, aTeam.legs, aTeam.torso);
        }
        if (PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString() == "2")
        {
            // shift + art + . : 선택한 텍스트 다음도 같이 선택
            // shift + art + ; : 선택한 텍스트 동일한거 전부 같이 선택
            playerController.ChangeMaterial(bTeam.head, bTeam.legs, bTeam.torso);

        }

    }
    #endregion
    #region Photon


    #endregion
}
