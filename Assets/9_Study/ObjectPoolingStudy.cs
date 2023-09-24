using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolingStudy : MonoBehaviour
{
   public GameObject cubePrefab;
    #region ����� ����
    //// stack : ��������
    //// queue : ���Լ���
    //public Queue<GameObject> bulletPools = new Queue<GameObject>();


    //public void Update()
    //{
    //    if(Input.GetMouseButtonDown(0))
    //    {
    //        CreateCube();
    //    }
    //}


    //public void CreateCube()
    //{
    //    GameObject newGo;
    //    if (bulletPools.Count == 0)
    //        newGo = Instantiate(cubePrefab);
    //    else
    //        newGo = bulletPools.Dequeue();

    //    newGo.SetActive(true);
    //    StartCoroutine(CorDeQueue(newGo));
    //}

    //IEnumerator CorDeQueue(GameObject newGo)
    //{
    //    yield return new WaitForSeconds(1);
    //    newGo.SetActive(false);
    //    bulletPools.Enqueue(newGo);
    //}
    #endregion

    #region ����Ƽ ����
    public ObjectPool<GameObject> pool;
    public void Start()
    {
        // Func<T> createFunc, Action<T> actionOnGet = null, Action<T> actionOnRelease = null, Action<T> actionOnDestroy = null
        pool = new ObjectPool<GameObject>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            pool.Get(); // Ǯ���� ������
        }
    }

    public GameObject CreateFunc()
    {
        return Instantiate(cubePrefab);
     }

    public void ActionOnGet(GameObject go)
    {
        go.SetActive(true);
        
        StartCoroutine(CorDeQueue(go));
    }

    IEnumerator CorDeQueue(GameObject newGo)
    {
        yield return new WaitForSeconds(1);
        pool.Release(newGo); // Ǯ�� �ٽ� �ֱ�
    }

    public void ActionOnRelease(GameObject go)
    {
        go.SetActive(false);
    }

    public void ActionOnDestroy(GameObject go)
    {
        Debug.Log("Destroy");
    }


    #endregion
    //private void Start()
    //{
    //    int a = 1;
    //    DebugLog(a, a);
    //    float b = 2.0f;
    //    DebugLog(b, a);
    //}

    //public void DebugLog<T, B>(T t, B b)
    //{
    //    Debug.Log(t.ToString());
    //    Debug.Log(b.ToString());
    //}   
}
