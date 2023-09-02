using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogInManager : MonoBehaviourPunCallbacks
{
    public InputField inputfieldNickName;
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnClickLogin();
        }
    }

    // Update is called once per frame
    public void OnClickLogin()
    {
        // IsNullOrWhiteSpace : String Null이거나 공백만 있을경우 "" || "  "
        if (string.IsNullOrWhiteSpace(inputfieldNickName.text))
            return;
        PhotonNetwork.NickName = inputfieldNickName.text;
        Debug.Log(PhotonNetwork.NickName);
        AsyncOperation asyinc = SceneManager.LoadSceneAsync("1_Lobby");
    }
}
