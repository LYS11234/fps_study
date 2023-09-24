using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using ExitGames.Client.Photon;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using System.Collections;
using System;
using UnityEngine.Pool;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;

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
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        if (instance != null)
            DestroyImmediate(instance);
        instance = this;
    }

    #region Const
    public const float playTotalTime = 1 * 60 * 1000;
    #endregion

    #region Scripts & Objects
    [Header("Scrips & Objects")]
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private DatabaseManager databaseManager;
    [SerializeField]
    public Transform team1Transform;
    [SerializeField]
    public Transform team2Transform;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private PhotonView view;
    public Text time;
    public Text team1_Score;
    public Text team2_Score;
    public Transform team1Parent;
    public Transform team2Parent;
    public GameObject team1Players;
    public GameObject team2Players;


    [SerializeField]
    private GameObject gamePanel;

    public ObjectPool<GameObject> bulletPool;
    public GameObject bulletPrefab;

    public DateTime endTime;
    
    #endregion

    #region Variables
    [Header("Variables")]
    private WaitForSeconds reviveTime = new WaitForSeconds(5f);
    public float syncEndTime = 5;
    public float remainMilliSeconds;
    private int team1Score = 0;
    private int team2Score = 0;

    #endregion

    #region Player Materials
    public TeamMaterial aTeam;
    public TeamMaterial bTeam;
    #endregion

    #region Codes
    private void Start()
    {
        Debug.Log("MapManager Start");
        endTime = DateTime.Now.AddMilliseconds(playTotalTime);
        SpawnSetting();
        ExitGames.Client.Photon.Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;
        team1Score = 0;
        team2Score = 0;
        cp["Team1 Score"] = team1Score.ToString();
        cp["Team2 Score"] = team2Score.ToString();

        Debug.Log("cp[\"Dead\"] : " + cp["Dead"]);
        ExitGames.Client.Photon.Hashtable cp2 = PhotonNetwork.LocalPlayer.CustomProperties;
        if (!cp2.ContainsKey("Dead"))
            cp2.Add("Dead", "false");
        else
            cp2["Dead"] = "false";

        if (!cp2.ContainsKey("Kill"))
        {
            cp2.Add("Kill", "0");
            cp2.Add("Death", "0");
        }
        else
        {
            cp2["Kill"] = "0";
            cp2["Death"] = "0";
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(cp2);
        bulletPool = new ObjectPool<GameObject>(CreatBullet, GetBullet, ReleaseBullet);
        CreatePlayerList();  
    }

    public GameObject CreatBullet()
    {
        return Instantiate(bulletPrefab);
    }

    public void GetBullet(GameObject go)
    {
        go.SetActive(true);
    }


    public void ReleaseBullet(GameObject go)
    {
        go.SetActive(false);
    }


    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
            TimerMaster();
        else
            TimerOther();

        if(Input.GetKeyDown(KeyCode.Tab))
            gamePanel.SetActive(true);
        if (Input.GetKeyUp(KeyCode.Tab))
            gamePanel.SetActive(false);
        //view.RPC("Timer", RpcTarget.All);
    }


    private void CreatePlayer(Player newPlayer, GameObject playerPerhaps, Transform playerParent)
    {
        GameObject go = Instantiate(playerPerhaps, playerParent);
        PrefabPlayer player = go.GetComponent<PrefabPlayer>();
        string playername = newPlayer.NickName;
        player.SetText(playername);
    }

    void CreatePlayerList()
    {
        //ExitGames.Client.Photon.Hashtable initialProps = new ExitGames.Client.Photon.Hashtable() { { } };//저기서 팀 분류된걸로 구분하는거 아냐?
        //PhotonNetwork.SetPlayerCustomProperties//용법이 어딨더라

        // 다른 유저들의 팀을 설정해준다.
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                ExitGames.Client.Photon.Hashtable initialProps = PhotonNetwork.PlayerList[i].CustomProperties;
                if (initialProps.ContainsKey("Team"))
                {
                    if (initialProps["Team"].ToString() == "1")
                    {
                        Debug.Log("Team: 1");
                        CreatePlayer(PhotonNetwork.PlayerList[i], team1Players, team1Parent);
                    }
                    else if (initialProps["Team"].ToString() == "2")
                    {
                        Debug.Log("Team: 2");
                        CreatePlayer(PhotonNetwork.PlayerList[i], team2Players, team2Parent);
                    }
                }
            }
        }
    }
    #endregion

    #region PlayerProps
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);

        if(propertiesThatChanged.ContainsKey("Team1 Score"))
            team1_Score.text = propertiesThatChanged["Team1 Score"].ToString();
        if (propertiesThatChanged.ContainsKey("Team2 Score"))
            team2_Score.text = propertiesThatChanged["Team2 Score"].ToString();
    }

    public void Respawn()
    {
        CameraSetNull();
        StartCoroutine(CorRespawn());
    }

    IEnumerator CorRespawn()
    {
        Dead();
        yield return reviveTime;
        Revive();
    }

    public void Dead()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString() == "1")
        {
            ExitGames.Client.Photon.Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;
            team2Score++;
            cp["Team2 Score"] = team2Score.ToString();
            PhotonNetwork.CurrentRoom.SetCustomProperties(cp);
        }
        else
        {
            ExitGames.Client.Photon.Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;
            team1Score++;
            cp["Team1 Score"] = team1Score.ToString();
            PhotonNetwork.CurrentRoom.SetCustomProperties(cp);
        }
    }

    #endregion
    #region Photon
    public void CameraSetNull()
    {
        Transform tfCam = Camera.main.transform;
        tfCam.SetParent(null);
    }
    public void Revive()
    {
        Debug.Log("Revive Actived");
        SpawnSetting();
    }

    private void SpawnSetting()
    {
        Transform tf = null;
        if (PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString() == "1")
        {
            tf = team1Transform;
        }
        if (PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString() == "2")
        {
            tf = team2Transform;
        }
        GameObject my = PhotonNetwork.Instantiate("player", tf.transform.position, Quaternion.identity);
        playerController = my.GetComponent<PlayerController>();
        Transform tfCam = Camera.main.transform;
        tfCam.SetParent(my.transform);
        tfCam.localPosition = new Vector3(0, 2, 0);
        playerController.camTransform = tfCam;
        playerController.cam = Camera.main;

        if (PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString() == "1")
        {
            playerController.ChangeMaterial(aTeam.head, aTeam.legs, aTeam.torso);
        }
        if (PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString() == "2")
        {
            // shift + alt + . : 선택한 텍스트 다음도 같이 선택
            // shift + alt + ; : 선택한 텍스트 동일한거 전부 같이 선택
            playerController.ChangeMaterial(bTeam.head, bTeam.legs, bTeam.torso);
        }
    }

    [PunRPC]
    private void GoResult()
    {
        bulletPool.Clear();
        PhotonNetwork.LoadLevel("5_Result");
    }

    private void TimerMaster()
    {
        DateTime now = DateTime.Now;
        TimeSpan timeSpan = endTime - now;
        if(timeSpan.TotalMilliseconds <= 0)
        {
            view.RPC("GoResult", RpcTarget.All);
        }
        else
        {
            // 1시간 30분 20초
            // .minute 30
            // .totalMinute 90
            time.text = $"{timeSpan.Minutes.ToString("00")}:{timeSpan.Seconds.ToString("00")}";
        }

        if(syncEndTime >= 5)
        {
            Debug.Log("timeSpan.TotalMilliseconds : " + timeSpan.TotalMilliseconds);
            view.RPC("SyncTimer", RpcTarget.Others, timeSpan.TotalMilliseconds);
            syncEndTime = 0;
        }

        syncEndTime += Time.deltaTime;

        //if (min == 0 && sec == 0f)
        //{
        //    PhotonNetwork.LoadLevel("2_Room");
        //}
        //else
        //{
        //    if (sec < 0f)
        //    {
        //        min--;
        //        sec = 59f;
        //    }
        //    else
        //    {
        //        sec -= Time.deltaTime;
        //    }
        //    time.text = $"{min.ToString()}:{sec.ToString(fmt)}";
        //}
    }

    public void TimerOther()
    {
        remainMilliSeconds -= (Time.deltaTime * 1000);
        int totalSeconds = (int)(remainMilliSeconds / 1000); // 밀리초를 초로 변환
        int minutes = totalSeconds / 60; // 초를 분으로 변환
        int seconds = totalSeconds % 60; // 나머지 초

        time.text = $"{minutes.ToString("00")}:{seconds.ToString("00")}";
    }

    [PunRPC]
    public void SyncTimer(double totalMilliseconds)
    {
        remainMilliSeconds = (float)totalMilliseconds;
    }
    #endregion


}
