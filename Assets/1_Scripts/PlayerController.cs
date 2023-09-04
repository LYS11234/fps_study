using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    [Header("Photon")]
    [SerializeField]
    PhotonView view;


    [Space(20)]
    [Header("Scripts Or Objects")]
    [SerializeField]
    private Rigidbody myRigid;

    [SerializeField]
    CharacterController characterController;
    public Camera cam;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Transform myTransform;

    public Animator myAnim;
    [Space(20)]

    [Header("Variables")]
    public int applySpeed = 5;

    public float mouseSensivity = 5;

    public float currentCameraRotationX;
    
    public float currentCameraRotationY;

    

    Vector3 realPos;
    Quaternion realRot;

   

    void Awake()
    {
        myTransform = transform;
        //view = GetComponent<PhotonView>();
        //myRigid = GetComponent<Rigidbody>();
        //characterController = GetComponent<CharacterController>();

       //Material mats =  skinnedMeshRenderer.material;
       // //mats[0] = ~ ;
       // skinnedMeshRenderer.material = mats;
        if (!view.IsMine)
        {
            Destroy(cam);
        }
    }

    void Start()
    {
        myAnim.SetBool("onGround", true);
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

        // 1. 마우스 움직임을 받아온다.
        // 2. 움직임만큼의 회전값을 받는다.
        // 3. 내 회전값에 움직임만큼의 회전값을 더한다.

        // -10 ~ +10
        CameraRotate();
        CharacterRotate();
        Aim();
        Reload();
        Shoot();
    }

    #region methodes
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        if (_moveDirX == 0 && _moveDirZ == 0)
        {
                IdleAnim();
            
        }
            if (_moveDirZ > 0)
            {
                if(!myAnim.GetBool("Aiming"))
                    MoveAnim();
                else
                {
                    myAnim.SetFloat("Y", 1);
                }
                
            }

            if (_moveDirZ < 0)
            {
                myAnim.SetFloat("Y", -1);
                
            }
        if (_moveDirX > 0)
            myAnim.SetFloat("X", 1);
        if (_moveDirX < 0)
            myAnim.SetFloat("X", -1);
        if(_moveDirX == 0)
        {
            myAnim.SetFloat("X", 0);
        }
        if (_moveDirZ == 0)
        {
            myAnim.SetFloat("Y", 0);
        }
        //Vector3 _moveHorizontal = myTransform.right * _moveDirX;
        //Vector3 _moveVertical = myTransform.forward * _moveDirZ;
        //Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;
        //characterController.Move(_velocity * Time.deltaTime);
        //myRigid.MovePosition(myTransform.position + _velocity * Time.deltaTime);



    }

    private void CameraRotate()
    {
        float mouseX = -Input.GetAxis("Mouse Y");
        currentCameraRotationX += mouseX * mouseSensivity;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -45, 45);
        cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);

    }

    private void CharacterRotate()
    {
        float mouseY = Input.GetAxis("Mouse X");
        currentCameraRotationY += mouseY * mouseSensivity;
        Quaternion quat = Quaternion.Euler(new Vector3(0, currentCameraRotationY, 0));
        myTransform.rotation = quat;
    }

    // 포톤 동기화 방법

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

    private void Aim()
    {
        if (Input.GetMouseButtonDown(1))
        {
            myAnim.SetBool("Aiming", true);
        }
        else if(Input.GetMouseButtonUp(1))
        {
            myAnim.SetBool("Aiming", false);
        }
    }

    private void Reload()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            myAnim.SetBool("Reloading", true);
        }
        else
        {
            myAnim.SetBool("Reloading", false);
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

    #region Anim
    public void IdleAnim()
    {
        myAnim.SetFloat("Speed", 0);
        myAnim.SetFloat("X", 0);
        myAnim.SetFloat("Y", 0);
    }
    public void MoveAnim()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            myAnim.SetFloat("Speed", 2f);
        }
        else
        { 
            myAnim.SetFloat("Speed", 1f); 
        }

    }

    private void Shoot()
    {
        if(Input.GetMouseButton(0))
        {
            myAnim.SetTrigger("Shoot");
            
        }
        //else
        //{
        //    myAnim.ResetTrigger("Shoot");
        //}
    }
    #endregion

    #endregion
}
