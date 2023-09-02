using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStudy : MonoBehaviour
{
    /*
     * F5 : 계속
     * F10 : 다음줄로
     * F11 : 현재 마킹되어있는곳이 함수라면 함수 내부로 이동 아니라면 다음줄로
     * F9 : 중단점 설정/해제
     */

    [ContextMenu("DebugTest")]
    public void DebugTest()
    {
        DebugTest2();
        
    }

    public void DebugTest2()
    {
        int a = 0;
        Debug.Log(a);
        Console.WriteLine(a);
    }
}