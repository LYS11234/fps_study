using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PrefabBullet : MonoBehaviourPun
{
    public Rigidbody myRigid;

    public bool mine;
    public int damage;
    public int force;
    public Vector3 dir;
    public WaitForSeconds liveTime = new WaitForSeconds(1f);
    public bool isDamaged = false;

    public void Shoot()
    {
        myRigid.AddForce(dir * force);
        StartCoroutine(LiveTimeCor());
    }

    IEnumerator LiveTimeCor()
    {
        yield return liveTime;
        if(this.gameObject.activeSelf)
             MapManager.instance.bulletPool.Release(this.gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision Name = {collision.gameObject.name}", collision.gameObject);
        // 데미지
        if(isDamaged && collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.Damage(damage);
            Debug.Log("Shoot_Player Actived");
        }

        StopCoroutine(LiveTimeCor());

        if(this.gameObject.activeSelf)
            MapManager.instance.bulletPool.Release(this.gameObject);
        // 삭제

    }
}
