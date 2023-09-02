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
struct MapData
{
    public string mapName;
    public Sprite img;
}

public class RoomManager : MonoBehaviourPunCallbacks
{

    #region 맵 데이터
    [SerializeField] private List<MapData> mapDatas; // 2개

    public GameObject mapPrefab;
    public Transform mapPrefabParent;
    #endregion

    public GameObject playerPerhaps_1;
    public GameObject playerPerhaps_2;
    public Transform playerListParent_1;
    public Transform playerListParent_2;

    [SerializeField]
    PhotonView view;

    [SerializeField]
    private Image selectedMapImage;

    [SerializeField]
    private GameObject mapLists;

    [SerializeField]
    private PrefabMap prefabMapScript;

    [SerializeField]
    private string selectedMap;
    private List<Player> team1List = new List<Player>();
    private List<Player> team2List = new List<Player>();

    Dictionary<Player, GameObject> playerPrefabDic = new Dictionary<Player, GameObject>();

    public Button startBtn;
    public Button mapSelectBtn;

    private void Awake()
    {
        CreateMapList();
        view = GetComponent<PhotonView>();
    }

    private void CreateMapList()
    {
        // 맵 생성 후 
        // 맵 프리팹 스크립트에다가 mapDatas[i]의 데이터를 넣어준다.
        for (int i = 0; i < mapDatas.Count; i++)
        {
            GameObject go = Instantiate(mapPrefab, mapPrefabParent);
            PrefabMap Map = go.GetComponent<PrefabMap>();
            Map.Image = mapDatas[i].img;
            Map.name = mapDatas[i].mapName;
        }
    }


    void CreatePlayerList()
    {
        //ExitGames.Client.Photon.Hashtable initialProps = new ExitGames.Client.Photon.Hashtable() { { } };//저기서 팀 분류된걸로 구분하는거 아냐?
        //PhotonNetwork.SetPlayerCustomProperties//용법이 어딨더라

        // 다른 유저들의 팀을 설정해준다.
        {
            for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
            {
                ExitGames.Client.Photon.Hashtable initialProps = PhotonNetwork.PlayerListOthers[i].CustomProperties;
                if(initialProps.ContainsKey("Team"))
                {
                    if (initialProps["Team"].ToString() == "1")
                    {
                        
                        CreatePlayer(PhotonNetwork.PlayerListOthers[i], playerPerhaps_1, playerListParent_1, team1List);
                    }
                    else if (initialProps["Team"].ToString() == "2")
                    {
                        CreatePlayer(PhotonNetwork.PlayerListOthers[i], playerPerhaps_2, playerListParent_2, team2List);
                    }
                }
            }
        }

        // 내 팀을 설정해준다.
        if (team1List.Count <= team2List.Count)
        {
            ExitGames.Client.Photon.Hashtable initialProps = PhotonNetwork.LocalPlayer.CustomProperties;
            initialProps.TryAdd("Team", "1");
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
            CreatePlayer(PhotonNetwork.LocalPlayer, playerPerhaps_1, playerListParent_1, team1List);
        }
        else
        {
            ExitGames.Client.Photon.Hashtable initialProps = PhotonNetwork.LocalPlayer.CustomProperties;
            initialProps.TryAdd("Team", "2");
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
            CreatePlayer(PhotonNetwork.LocalPlayer, playerPerhaps_2, playerListParent_2, team2List);
        }
    }

    //public override void OnPlayerEnteredRoom(Player newPlayer)
    //{
    //    base.OnPlayerEnteredRoom(newPlayer);

    //    if (team1List.Count <= team2List.Count)
    //    {
    //        CreatePlayer(newPlayer, playerPerhaps_1, playerListParent_1, team1List);
    //    }
    //    else
    //    {
    //        CreatePlayer(newPlayer, playerPerhaps_2, playerListParent_2, team2List);
    //    }
    //}

    private void CreatePlayer(Player newPlayer, GameObject playerPerhaps, Transform playerParent, List<Player> list)
    {
        DeletePlayer(newPlayer);
        GameObject go = Instantiate(playerPerhaps, playerParent);
        PrefabPlayer player = go.GetComponent<PrefabPlayer>();
        string playername = newPlayer.NickName;
        list.Add(newPlayer);
        player.SetText(playername);

        playerPrefabDic.Add(newPlayer, go);
    }

    private void DeletePlayer(Player newPlayer)//나중에 teamlist에서 삭제까지 추가할 것
    {
        if(playerPrefabDic.ContainsKey(newPlayer))
        {
            Destroy(playerPrefabDic[newPlayer]);
            playerPrefabDic.Remove(newPlayer);
        }
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        //playerController.name = text.name;
        
        Debug.Log("OnCreatedRoom");
        Debug.Log(PhotonNetwork.NickName);
    }

    

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        bool isExist = false;
        Debug.Log("OnPlayerPropertiesUpdate" + JsonConvert.SerializeObject(changedProps));
        for (int i = 0; i < team1List.Count; i++)
        {
            if (team1List[i] == targetPlayer)
            {
                isExist = true;

                // 기존 팀1에서 삭제하고
                // 팀 2에 재생성
                if(changedProps["Team"].ToString() == "2")
                {
                    team1List.Remove(targetPlayer);
                    team2List.Add(targetPlayer);
                    CreatePlayer(targetPlayer, playerPerhaps_2, playerListParent_2, team2List);
                }
                
            }
        }


        if(!isExist)
        {
            for (int i = 0; i < team2List.Count; i++)
            {
                if (team2List[i] == targetPlayer)
                {
                    isExist = true;

                    if (changedProps["Team"].ToString() == "1")
                    {
                        team2List.Remove(targetPlayer);
                        team1List.Add(targetPlayer);
                        CreatePlayer(targetPlayer, playerPerhaps_1, playerListParent_1, team1List);
                        
                    }
                }
            }
        }

        if (!isExist)
        {
            string tm = changedProps["Team"].ToString();

            if(tm == "1") {
                if(team2List.Contains(targetPlayer))
                    team2List.Remove(targetPlayer);
                team1List.Add(targetPlayer);
                CreatePlayer(targetPlayer, playerPerhaps_1, playerListParent_1, team1List);

            }
            else if (tm == "2")
            {
                if(team1List.Contains(targetPlayer))
                    team1List.Remove(targetPlayer);
                team2List.Add(targetPlayer);
                CreatePlayer(targetPlayer, playerPerhaps_2, playerListParent_2, team2List);

            }
        }
    }

    public void ChangeTeam(string _team)
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["Team"] = _team;
        PhotonNetwork.LocalPlayer.SetCustomProperties(cp);
        
    }

    //public void ChangeTeam2Red()
    //{
    //    Debug.Log("ChangeTeam2Red accessed");
    //    Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
    //    cp["Team"] = "1";
    //    PhotonNetwork.LocalPlayer.SetCustomProperties(cp);
    //}

    //public void ChangeTeam2Blue()
    //{
    //    Debug.Log("ChangeTeam2Blue accessed");
    //    Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
    //    cp["Team"] = "2";
    //    PhotonNetwork.LocalPlayer.SetCustomProperties(cp);
    //}//버튼 실행인데 버튼에서 매개변수

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }

    //public override void OnPlayer

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        //playerController.name = text.name;
        
        CreatePlayerList();
        ShowMapPre();
        if (!PhotonNetwork.IsMasterClient)
        {
            mapSelectBtn.interactable = false;
            startBtn.interactable = false;
        }

        //PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);

    }
    public void ShowMaps()
    {
        mapLists.gameObject.SetActive(true);
    }

    public void SelectMap(PrefabMap prefabmap)
    {
        selectedMap = prefabmap.name;
        selectedMapImage.sprite = prefabmap.Image;
        mapLists.gameObject.SetActive(false);
        Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;
        cp["MapName"] = selectedMap;
        PhotonNetwork.CurrentRoom.SetCustomProperties(cp);
        
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);

        

        if (!PhotonNetwork.IsMasterClient)
        {
            ShowMapPre();
        }

    }

    [PunRPC]
    public void ChangeScene(string _Scene)
    {
        PhotonNetwork.LoadLevel(_Scene);
    }

    private void ShowMapPre()
    {
        selectedMap = PhotonNetwork.CurrentRoom.CustomProperties["MapName"].ToString();
        MapData mapdata = mapDatas.Find(v => v.mapName == selectedMap);
        selectedMapImage.sprite = mapdata.img;//List 사용법
    }

    public void StartBtn()
    {
        view.RPC("ChangeScene", RpcTarget.All, selectedMap);
    }
}
