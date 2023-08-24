using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    PhotonView view;
    Rigidbody myRigid;
    public int applySpeed = 5;
    public string name;

    Vector3 realPos;
    Quaternion realRot;

    public Transform myTransform;

    void Awake()
    {
        myTransform = transform;
        view = GetComponent<PhotonView>();
        myRigid = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (view.IsMine)
        {
            Move();
            if(Input.GetKeyDown(KeyCode.Space))
            {
                view.RPC("Jump", RpcTarget.All, "abc", "def");
            }
            else if(Input.GetKeyDown(KeyCode.T))
            {
                PropertyUpdate();
            }
        }
        else
        {
            myTransform.position = Vector3.Lerp(myTransform.position, realPos, 10 * Time.deltaTime);
            myTransform.rotation = Quaternion.Lerp(myTransform.rotation, realRot, 10 * Time.deltaTime);
        }
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = myTransform.right * _moveDirX;
        Vector3 _moveVertical = myTransform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(myTransform.position + _velocity * Time.deltaTime);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(myTransform.position);
            stream.SendNext(myTransform.rotation);
        } 
        else
        {
            realPos = (Vector3)stream.ReceiveNext();
            realRot = (Quaternion)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void Jump(string a, string b)
    {
        Debug.Log($"Jump {a}, {b}, " + view.name);
    }

    public void PropertyUpdate()
    {
        ExitGames.Client.Photon.Hashtable initialProps = new ExitGames.Client.Photon.Hashtable() { { "isReady", false } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
    }
}
