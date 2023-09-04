using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabPlayer : MonoBehaviour
{
    public Text playerNametext;
    public Text readyText;



    public void SetText(string _playername)
    {
        playerNametext.text = _playername;
    }

    public void SetReady(string _isReady)
    {
        if(_isReady == "true")
        {
            readyText.text = "Ready!";
        }
        else
        {
            readyText.text = "";
        }
    }
    
}
