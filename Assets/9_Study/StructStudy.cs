using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Parent { }

// 상속이 안된다.
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


// 메모리 저장공간에 저장되는 지역 다름
// 코드영역, 데이터 영역, 힙, 스택
// 클래스는 힙, 구조체는 스택
// 스택 : 공간이 많지 않다. GC가 지워주지 않는다.
// 힙 : 공간이 많다. GC가 지워준다.

// 힙 : 참조 변수
// 스택 : 값 변수