using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ListStudy : MonoBehaviour
{
    public int matchI;
    // (매개변수) => { 함수 내용 };
    // void List() { };
    [ContextMenu("`123123")]
   public void List()
    {
        List<int> numbers = new List<int>() { 1, 2, 3, 1 };
        List<int> numbers2 = new List<int>() { 1, 2, 3 };

        //numbers.Add(0);
        //numbers.Clear();
        //numbers.Remove(0);
        //numbers.RemoveAt(0);
        //Debug.Log(JsonConvert.SerializeObject(numbers));

        //int i = numbers.Find(x => x == matchI);
        //Debug.Log($"find {matchI}: {i}"); // 못찾으면 자료형 기본값

        //int i = numbers.FindIndex(x => x == matchI);
        //Debug.Log($"find Index {matchI}: {i}"); // 못찾으면 -1

        // 리스트 추가
        //numbers.AddRange(numbers2);

        // 반복문
        //numbers.ForEach(v =>
        //{
        //    Debug.Log($"{v}");
        //});

        // 마지막꺼를 찾는거
        //numbers.FindLast(x => x == matchI);

        // 1이 포함되어 있는지
        // numbers.Contains(1);

        // numbers.IndexOf(1);

        // numbers.Insert(0, 4);

        // 그냥 remove는 하나만 지우고, 이건 전부 지운다.
        // numbers.RemoveAll(x => x == 1);

        // 자료형 변환
        //List<float> floats = numbers.ConvertAll(x => (float)x);
    }
}
