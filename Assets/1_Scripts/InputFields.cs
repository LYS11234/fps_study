using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 지워버림
public class InputFields : MonoBehaviour
{
    private Text text;
    public string name;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            name = text.text;
        }
    }
}
