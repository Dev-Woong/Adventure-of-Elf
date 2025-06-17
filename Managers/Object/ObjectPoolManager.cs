using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    private class ObjectInfo
    {
        // 오브젝트 이름
        public string objectName;
        // 오브젝트 풀에서 관리할 오브젝트
        public GameObject perfab;
        // 몇개를 미리 생성 해놓을건지
        public int count;
    }


    public static ObjectPoolManager instance;

    // 오브젝트풀 매니저 준비 완료표시
    public bool IsReady { get; private set; }

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    // 생성할 오브젝트의 key값지정을 위한 변수
    private string objectName;

    // 오브젝트풀들을 관리할 딕셔너리
    private Dictionary<string, IObjectPool<GameObject>> ojbectPoolDic = new Dictionary<string, IObjectPool<GameObject>>();

    // 오브젝트풀에서 오브젝트를 새로 생성할때 사용할 딕셔너리
    private Dictionary<string, GameObject> ObjectDic = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
            
        }
        else
            Destroy(this.gameObject);
        
        Init();
    }


    private void Init()
    {
        IsReady = false;

        for (int idx = 0; idx < objectInfos.Length; idx++)
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
            OnDestroyPoolObject, true, objectInfos[idx].count, objectInfos[idx].count);

            if (ObjectDic.ContainsKey(objectInfos[idx].objectName))
            {
                Debug.LogFormat("{0} 이미 등록된 오브젝트입니다.", objectInfos[idx].objectName);
                return;
            }

            ObjectDic.Add(objectInfos[idx].objectName, objectInfos[idx].perfab);
            ojbectPoolDic.Add(objectInfos[idx].objectName, pool);

            // 미리 오브젝트 생성 해놓기
            for (int i = 0; i < objectInfos[idx].count; i++)
            {
                objectName = objectInfos[idx].objectName;
                PoolAble poolAbleObject = CreatePooledItem().GetComponent<PoolAble>();
                poolAbleObject.Pool.Release(poolAbleObject.gameObject);
            }
        }

        Debug.Log("오브젝트풀링 준비 완료");
        IsReady = true;
        
    }

    // 생성
    private GameObject CreatePooledItem()
    {
        GameObject poolObject = Instantiate(ObjectDic[objectName]);
        poolObject.GetComponent<PoolAble>().Pool = ojbectPoolDic[objectName];
        
        return poolObject;
    }

    // 대여
    private void OnTakeFromPool(GameObject poolObject)
    {
        poolObject.SetActive(true);
    }

    // 반환
    private void OnReturnedToPool(GameObject poolObject)
    {
        poolObject.SetActive(false);
    }

    // 삭제
    private void OnDestroyPoolObject(GameObject poolObject)
    {
        Destroy(poolObject);
    }

    public GameObject GetObject(string objectName)
    {
        this.objectName = objectName;

        if (ObjectDic.ContainsKey(objectName) == false)
        {
            Debug.LogFormat("{0} 오브젝트풀에 등록되지 않은 오브젝트입니다.", objectName);
            return null;
        }

        return ojbectPoolDic[objectName].Get();
    }
}
