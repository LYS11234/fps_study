using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogInManager : MonoBehaviourPunCallbacks
{
    public InputField inputfieldNickName; //InputField 정의

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
            return;//빈 칸이면 실행안함
        PhotonNetwork.NickName = inputfieldNickName.text;//LocalPlayer의 닉네임을 입력한 텍스트로 설정
        Debug.Log(PhotonNetwork.NickName);
        AsyncOperation asyinc = SceneManager.LoadSceneAsync("1_Lobby");//Lobby로 이동
    }
}
