using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogInManager : MonoBehaviourPunCallbacks
{
    public InputField inputfieldNickName; //InputField ����

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
        // IsNullOrWhiteSpace : String Null�̰ų� ���鸸 ������� "" || "  "
        if (string.IsNullOrWhiteSpace(inputfieldNickName.text))
            return;//�� ĭ�̸� �������
        PhotonNetwork.NickName = inputfieldNickName.text;//LocalPlayer�� �г����� �Է��� �ؽ�Ʈ�� ����
        Debug.Log(PhotonNetwork.NickName);
        AsyncOperation asyinc = SceneManager.LoadSceneAsync("1_Lobby");//Lobby�� �̵�
    }
}
