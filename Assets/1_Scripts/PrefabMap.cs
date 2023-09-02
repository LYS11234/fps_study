using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabMap : MonoBehaviour
{
    public string name;
    public Sprite Image;

    public Image image;
    public Text text;

    public RoomInfo roomInfo;


    private void Start()
    {
        text.text = name;
        image.sprite = Image;
    }


    public void OnClick()
    {
        Database.Instance.roomManager.SelectMap(this);
    }
}
