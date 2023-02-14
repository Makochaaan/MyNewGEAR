using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using System.Collections;

[RequireComponent(typeof(SoundPrefs))]
[System.Serializable]
public class ObjectPoolSE
{
    public string name;
    public AudioClip seToPool;
    public int initialPool;
    public bool expandAllowed = true;
}
public class SEManager : MonoBehaviour
{
    public static SEManager SharedInstance;
    private SoundPrefs soundPrefs;

    private void Awake()
    {
        soundPrefs = GetComponent<SoundPrefs>();
        SharedInstance = this;
    }
    public List<ObjectPoolSE> sesToPool;
    private List<GameObject> pooledSEs;
    private void Start()
    {
        //初期プールの作成
        pooledSEs = new List<GameObject>();
        foreach (ObjectPoolSE item in sesToPool)
        {
            for (int i = 0; i < item.initialPool; i++)
            {
                GameObject obj = new GameObject(item.name);
                AudioSource audioSource = obj.AddComponent<AudioSource>();
                audioSource.clip = item.seToPool;
                audioSource.volume = soundPrefs.saveData.seVolume;
                audioSource.loop = false;
                audioSource.spatialBlend = 1;
                obj.SetActive(false);
                pooledSEs.Add(obj);
            }
        }
    }
    //プールの中で非アクティブのやつを見つける、無ければ拡張
    private GameObject GetPooledObject(string name)
    {
        for (int i = 0; i < pooledSEs.Count; i++)
        {
            if (!pooledSEs[i].activeInHierarchy && pooledSEs[i].name == name)
            {
                return pooledSEs[i];
            }
        }
        foreach (ObjectPoolSE item in sesToPool)
        {
            if (item.name == name)
            {
                if (item.expandAllowed)
                {
                    GameObject obj = new GameObject(item.name);
                    AudioSource audioSource = obj.AddComponent<AudioSource>();
                    audioSource.clip = item.seToPool;
                    audioSource.volume = soundPrefs.saveData.seVolume;
                    audioSource.loop = false;
                    audioSource.spatialBlend = 1;
                    obj.SetActive(false);
                    pooledSEs.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }

    //非ループSEを出す
    /// <summary>
    /// SE再生関数。
    /// </summary>
    /// <param name="name">ObjectPoolSEクラスに登録してあるSEの名前</param>
    /// <param name="position">SEの発生する場所</param>
    public void PlaySE(string name,Vector3 position)
    {
        GameObject se = GetPooledObject(name);
        if(se != null)
        {
            se.transform.position = position;
            se.SetActive(true);
            AudioSource audioSource = se.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioSource.clip);
            StartCoroutine(SEStop(audioSource.clip.length, name, se));
        }
        else
        {
            Debug.LogWarning($"SE '{name}' not found!");
            return;
        }
    }
    private IEnumerator SEStop(float delay, string name, GameObject se)
    {
        yield return new WaitForSeconds(delay);
        se.SetActive(false);
    }
}
