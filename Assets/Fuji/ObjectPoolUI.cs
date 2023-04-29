using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// プールするUIの情報
[System.Serializable]
public class ObjectPoolUIInfo
{
    public string name;
    public GameObject uiToPool;
    public float duration;
    public int initialPool;
}

// 上記の情報をもとにプールされたUIの情報
public class PooledUI
{
    public GameObject uiObject;
    public float duration;
    public RectTransform myTransform;
    public Vector3 positionTemp;
}

public class ObjectPoolUI : MonoBehaviour
{
    // インスタンス
    public static ObjectPoolUI sharedInstance;
    // プールしたいUI情報のリスト
    public List<ObjectPoolUIInfo> infoList;
    // プールされたUIのリスト
    public List<PooledUI> pooledUIs;
    private void Awake()
    {
        sharedInstance = this;
    }
    private void Start()
    {
        InitializePool();
    }
    private void InitializePool()
    {
        //初期プールの作成
        pooledUIs = new List<PooledUI>();
        foreach (ObjectPoolUIInfo info in infoList)
        {
            for (int j = 0; j < info.initialPool; j++)
            {
                PooledUI pooledUI = new PooledUI();
                pooledUI.uiObject = Instantiate(info.uiToPool);
                pooledUI.uiObject.name = info.name;
                pooledUI.uiObject.transform.SetParent(transform);
                pooledUI.uiObject.SetActive(false);
                pooledUI.duration = info.duration;
                pooledUI.myTransform = pooledUI.uiObject.GetComponent<RectTransform>();
                pooledUIs.Add(pooledUI);
            }
        }
    }
    //プールの中で非アクティブのやつを見つける、無ければ拡張
    public PooledUI GetPooledObject(string name)
    {
        for (int i = 0; i < pooledUIs.Count; i++)
        {
            if (!pooledUIs[i].uiObject.activeInHierarchy && pooledUIs[i].uiObject.name == name)
            {
                return pooledUIs[i];
            }
        }
        foreach (ObjectPoolUIInfo info in infoList)
        {
            if (info.name == name)
            {
                PooledUI pooledUI = new PooledUI();
                pooledUI.uiObject = Instantiate(info.uiToPool);
                pooledUI.uiObject.name = info.name;
                pooledUI.uiObject.transform.SetParent(transform);
                pooledUI.uiObject.SetActive(false);
                pooledUI.duration = info.duration;
                pooledUI.myTransform = pooledUI.uiObject.GetComponent<RectTransform>();
                pooledUIs.Add(pooledUI);
                return pooledUI;
            }
        }
        return null;
    }
}
