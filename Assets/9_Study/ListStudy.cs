using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ListStudy : MonoBehaviour
{
    public int matchI;
    // (�Ű�����) => { �Լ� ���� };
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
        //Debug.Log($"find {matchI}: {i}"); // ��ã���� �ڷ��� �⺻��

        //int i = numbers.FindIndex(x => x == matchI);
        //Debug.Log($"find Index {matchI}: {i}"); // ��ã���� -1

        // ����Ʈ �߰�
        //numbers.AddRange(numbers2);

        // �ݺ���
        //numbers.ForEach(v =>
        //{
        //    Debug.Log($"{v}");
        //});

        // ���������� ã�°�
        //numbers.FindLast(x => x == matchI);

        // 1�� ���ԵǾ� �ִ���
        // numbers.Contains(1);

        // numbers.IndexOf(1);

        // numbers.Insert(0, 4);

        // �׳� remove�� �ϳ��� �����, �̰� ���� �����.
        // numbers.RemoveAll(x => x == 1);

        // �ڷ��� ��ȯ
        //List<float> floats = numbers.ConvertAll(x => (float)x);
    }
}
