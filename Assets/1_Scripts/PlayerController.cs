using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using System.IO;
using Photon.Realtime;
using UnityEngine.Pool;

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

    public SkinnedMeshRenderer skinnedMeshRendererHead;
    public SkinnedMeshRenderer skinnedMeshRendererLegs;
    public SkinnedMeshRenderer skinnedMeshRendererTorso;

    public Transform myTransform;

    public Transform camTransform;
    public Camera cam;

    public Animator myAnim;

    public ParticleSystem fireEffect;

    

    [Space(20)]

    [Header("Variables")]

    public int applySpeed = 5;

    public float mouseSensivity = 5;

    public float currentCameraRotationX;

    public float currentCameraRotationY;

    public float HP;

    private bool canMove = true;

    private int kills;

    private int deaths;

    bool dump = true;

    public bool isDead = false;

    private WaitForSeconds waitTime = new WaitForSeconds(0.2f);
    private WaitForSeconds reviveTime = new WaitForSeconds(2f);
    private WaitForSeconds destroyTime = new WaitForSeconds(3f);
    [SerializeField]
    private int force;
    [SerializeField]
    private int damage = 10;

    public int jumpForce = 100;

    Vector3 realPos;

    Quaternion realRot;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        myTransform = transform;
        //view = GetComponent<PhotonView>();
        //myRigid = GetComponent<Rigidbody>();
        //characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        myAnim.SetBool("onGround", true);
        if (view.Owner.CustomProperties["Team"].ToString() == "1")
        {
            this.ChangeMaterial(MapManager.instance.aTeam.head, MapManager.instance.aTeam.legs, MapManager.instance.aTeam.torso);
        }
        if (view.Owner.CustomProperties["Team"].ToString() == "2")
        {
            this.ChangeMaterial(MapManager.instance.bTeam.head, MapManager.instance.bTeam.legs, MapManager.instance.bTeam.torso);
        }

        HP = 20;
    }

    private void Update()
    {
        if (view.IsMine)
        {
            if (canMove)
            {
                Move();
                CameraRotate();
                CharacterRotate();
                Aim();
                Reload();
                //Jump();
                Shoot();
            }
            
            else if (Input.GetKeyDown(KeyCode.T))
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

    }

    #region methodes
    public void ChangeMaterial(Material teamHead, Material teamLegs, Material teamTorso)
    {
        skinnedMeshRendererHead.material = teamHead;
        skinnedMeshRendererLegs.material = teamLegs;
        skinnedMeshRendererTorso.material = teamTorso;
    }

    //private void Jump()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        //myRigid.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
    //        myAnim.SetTrigger("Jump");
    //    }
    //}

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");//A or D or ← or → 버튼 입력 감지
        float _moveDirZ = Input.GetAxisRaw("Vertical");//W or S or ↑ or ↓ 버튼 입력 감지

        myAnim.SetFloat("Horizontal", _moveDirX);
        myAnim.SetFloat("Vertical", _moveDirZ);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            myAnim.SetBool("Run", true);
        }
        else
            myAnim.SetBool("Run", false);

        //if (_moveDirX == 0 && _moveDirZ == 0)
        //{
        //    IdleAnim();//버튼 입력 없으면 가만히 있는 애니메이션 실행

        //}
        //if (_moveDirZ > 0)
        //{
        //    if (!myAnim.GetBool("Aiming"))//W or S or ↑ or ↓ 버튼 입력 감지되고, Aiming이 false면 이동 애니메이션 실행
        //        MoveAnim();
        //    else
        //    {
        //        myAnim.SetFloat("Vertical", 1);//W or S or ↑ or ↓ 버튼 입력 감지되고, Aiming이 ture면 조준 상태 이동 애니메이션 실행
        //    }

        //}

        //if (_moveDirZ < 0)
        //{
        //    myAnim.SetFloat("Vertical", -1);

        //}
        //if (_moveDirX > 0)
        //    myAnim.SetFloat("Horizontal", 1);
        //if (_moveDirX < 0)
        //    myAnim.SetFloat("Horizontal", -1);
        //if (_moveDirX == 0)
        //{
        //    myAnim.SetFloat("Horizontal", 0);
        //}
        //if (_moveDirZ == 0)
        //{
        //    myAnim.SetFloat("Vertical", 0);
        //}
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
        camTransform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);

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
        if (stream.IsWriting)
        {
            stream.SendNext(myTransform.position);
            stream.SendNext(myTransform.rotation);
            stream.SendNext(myAnim.GetFloat("X"));
            stream.SendNext(myAnim.GetFloat("Y"));
            stream.SendNext(myAnim.GetFloat("Speed"));
            stream.SendNext(myAnim.GetBool("Aiming"));
        }
        else
        {
            realPos = (Vector3)stream.ReceiveNext();
            realRot = (Quaternion)stream.ReceiveNext();
            myAnim.SetFloat("X", (float)stream.ReceiveNext());
            myAnim.SetFloat("Y", (float)stream.ReceiveNext());
            myAnim.SetFloat("Speed", (float)stream.ReceiveNext());
            myAnim.SetBool("Aiming", (bool)stream.ReceiveNext());
        }
    }

    private void Aim()
    {
        if (Input.GetMouseButtonDown(1))
        {
            myAnim.SetBool("Aiming", true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            myAnim.SetBool("Aiming", false);
        }
    }

    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
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

    public void Damage(int dmg)
    {
        if (isDead) return;
        HP -= dmg;
        view.RPC("DamageSync", RpcTarget.Others, HP);
        if (HP <= 0)
        {
            deaths++;
            PhotonNetwork.LocalPlayer.CustomProperties["Death"] = deaths.ToString();
            isDead = true;

            // 내킬수 증가

            view.RPC("Dead", RpcTarget.All);
            //StartCoroutine(Died());
        }
    }

    [PunRPC]
    public void DamageSync(float hp)
    {
        HP = hp;
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

    public void Shoot()
    {
        // 나한테만 실행시킬것
        if (Input.GetMouseButton(0) && dump)
        {
            dump = false;
            StartCoroutine(CorDelay());
            myAnim.SetTrigger("Shoot");
            fireEffect.Play(true);

            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width >> 1, Screen.height >> 1, 0));
            Bullet(ray.origin, ray.direction, true);
            view.RPC("Bullet", RpcTarget.Others, ray.origin, ray.direction, false);


        }



    }

    [PunRPC]
    private void Bullet(Vector3 origin, Vector3 dir, bool isDamaged)
    {
        //PrefabBullet bullet = Instantiate(bulletPrefab, origin + myTransform.forward, Quaternion.identity, null).GetComponent<PrefabBullet>();
        PrefabBullet bullet = MapManager.instance.bulletPool.Get().GetComponent<PrefabBullet>();
        bullet.transform.position = origin + myTransform.forward;
        bullet.damage = damage;
        bullet.isDamaged = isDamaged;
        bullet.force = force;
        bullet.dir = dir;

        bullet.Shoot();
    }

    IEnumerator CorDelay()
    {
        yield return waitTime;
        dump = true;
    }

    [PunRPC]
    private void Dead()
    {
        myAnim.SetBool("Dead", true);
        canMove = false;

        if(view.IsMine)
        {
            // 내 데드 수 증가

            StartCoroutine(Destory());
            MapManager.instance.Respawn();
        }
    }

    IEnumerator Destory() {
        yield return destroyTime;
        PhotonNetwork.Destroy(gameObject);
    }
    #endregion

    #endregion
}
