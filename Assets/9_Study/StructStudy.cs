using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Parent { }

// ����� �ȵȴ�.
public struct newStruct
{
    public int hp;
}

public class newClass
{
    public int hp;
}

public class StructStudy : MonoBehaviour
{
    int hp;
}


// �޸� ��������� ����Ǵ� ���� �ٸ�
// �ڵ念��, ������ ����, ��, ����
// Ŭ������ ��, ����ü�� ����
// ���� : ������ ���� �ʴ�. GC�� �������� �ʴ´�.
// �� : ������ ����. GC�� �����ش�.

// �� : ���� ����
// ���� : �� ����