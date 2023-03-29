using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class ObjectPoolVFX
{
    public string name;
    public GameObject vfxToPool;
    public int initialPool;
    public bool expandAllowed = true;
}
public class VFXManager : MonoBehaviour
{
    public static VFXManager SharedInstance;
    private void Awake()
    {
        SharedInstance = this;
    }
    public List<ObjectPoolVFX> vfxsToPool;
    private List<GameObject> pooledVFXs;

    private void Start()
    {
        //初期プールの作成
        pooledVFXs = new List<GameObject>();
        foreach (ObjectPoolVFX item in vfxsToPool)
        {
            for (int i = 0; i < item.initialPool; i++)
            {
                GameObject obj = Instantiate(item.vfxToPool);
                obj.transform.SetParent(transform, true);
                obj.name = item.name;
                obj.SetActive(false);
                pooledVFXs.Add(obj);
            }
        }
    }
    //プールの中で非アクティブのやつを見つける、無ければ拡張
    private GameObject GetPooledObject(string name)
    {
        for (int i = 0; i < pooledVFXs.Count; i++)
        {
            if (!pooledVFXs[i].activeInHierarchy && pooledVFXs[i].name == name)
            {
                return pooledVFXs[i];
            }
        }
        foreach (ObjectPoolVFX item in vfxsToPool)
        {
            if (item.name == name)
            {
                if (item.expandAllowed)
                {
                    GameObject obj = Instantiate(item.vfxToPool);
                    obj.transform.SetParent(transform, true);
                    obj.name = item.name;
                    obj.SetActive(false);
                    pooledVFXs.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }
    /// <summary>
    /// エフェクト発生関数。
    /// </summary>
    /// <param name="name">ObjectPoolVFXクラスに登録してあるエフェクトの名前。</param>
    /// <param name="position">発生させる場所</param>
    /// <param name="rotation">発生させる向き</param>
    public void PlayVFX(string name, Vector3 position, Quaternion rotation)
    {
        //指定した名前のエフェクトをget
        GameObject vfx = GetPooledObject(name);
        if(vfx != null)
        {
            //位置回転を引数に合わせる
            vfx.transform.position = position;
            vfx.transform.rotation = rotation;
            vfx.SetActive(true);
            //LineRendererと共用なのでここに書く
            ParticleSystem ps = vfx.GetComponent<ParticleSystem>();
            ps.Play(true);
            StartCoroutine(ParticleStop(ps.main.duration, vfx));
        }
        else
        {
            Debug.LogWarning($"vfx '{name}' not found!");
            return;
        }
    }
    private IEnumerator ParticleStop(float delay, GameObject vfx)
    {
        yield return new WaitForSeconds(delay);
        vfx.SetActive(false);
    }
    /// <summary>
    /// LineRendererのエフェクト発生関数。
    /// </summary>
    /// <param name="name">ObjectPoolVFXクラスに登録してあるエフェクトの名前</param>
    /// <param name="points">線で結ぶ全ての位置。配列の順につなげる</param>
    /// <param name="duration">エフェクト持続時間</param>
    public void PlayLineRenderer(string name, Vector3[] points,float duration)
    {
        GameObject vfx = GetPooledObject(name);
        if (vfx != null)
        {
            //頂点を引数に合わせる
            LineRenderer lr = vfx.GetComponent<LineRenderer>();
            lr.SetPositions(points);
            vfx.SetActive(true);
            StartCoroutine(LineRendererStop(duration, vfx));
        }
        else
        {
            Debug.LogWarning($"effect '{name}' not found!");
            return;
        }
    }
    private IEnumerator LineRendererStop(float delay, GameObject vfx)
    {
        yield return new WaitForSeconds(delay);
        vfx.SetActive(false);
    }
}
