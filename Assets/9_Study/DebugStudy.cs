using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStudy : MonoBehaviour
{
    /*
     * F5 : ���
     * F10 : �����ٷ�
     * F11 : ���� ��ŷ�Ǿ��ִ°��� �Լ���� �Լ� ���η� �̵� �ƴ϶�� �����ٷ�
     * F9 : �ߴ��� ����/����
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