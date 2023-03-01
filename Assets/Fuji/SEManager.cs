using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(SaveData))]
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
    [SerializeField] private SaveData saveData;

    private void Awake()
    {
        SharedInstance = this;
    }
    public List<ObjectPoolSE> sesToPool;
    private List<GameObject> pooledSEs;
    private void Start()
    {
        //�����v�[���̍쐬
        pooledSEs = new List<GameObject>();
        foreach (ObjectPoolSE item in sesToPool)
        {
            for (int i = 0; i < item.initialPool; i++)
            {
                GameObject obj = new GameObject(item.name);
                obj.transform.SetParent(transform, true);
                AudioSource audioSource = obj.AddComponent<AudioSource>();
                audioSource.clip = item.seToPool;
                audioSource.volume = saveData.jsonProperty.seVolume;
                audioSource.loop = false;
                audioSource.spatialBlend = 1;
                obj.SetActive(false);
                pooledSEs.Add(obj);
            }
        }
    }
    //�v�[���̒��Ŕ�A�N�e�B�u�̂��������A������Ίg��
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
                    obj.transform.SetParent(transform, true);
                    AudioSource audioSource = obj.AddComponent<AudioSource>();
                    audioSource.clip = item.seToPool;
                    audioSource.loop = false;
                    obj.SetActive(false);
                    pooledSEs.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }

    //�񃋁[�vSE���o��
    /// <summary>
    /// SE�Đ��֐��B
    /// </summary>
    /// <param name="name">ObjectPoolSE�N���X�ɓo�^���Ă���SE�̖��O</param>
    /// <param name="is3D">�����ɂ���Č������邩</param>
    /// <param name="position">SE�̔�������ꏊ</param>
    public void PlaySE(string name, bool is3D, Vector3 position)
    {
        GameObject se = GetPooledObject(name);
        if(se != null)
        {
            se.transform.position = position;
            se.SetActive(true);
            AudioSource audioSource = se.GetComponent<AudioSource>();
            audioSource.spatialBlend = is3D ? 1 : 0;
            audioSource.volume = saveData.jsonProperty.seVolume;
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
