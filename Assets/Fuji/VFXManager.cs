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
        //�����v�[���̍쐬
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
    //�v�[���̒��Ŕ�A�N�e�B�u�̂��������A������Ίg��
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
    /// �G�t�F�N�g�����֐��B
    /// </summary>
    /// <param name="name">ObjectPoolVFX�N���X�ɓo�^���Ă���G�t�F�N�g�̖��O�B</param>
    /// <param name="position">����������ꏊ</param>
    /// <param name="rotation">�������������</param>
    public void PlayVFX(string name, Vector3 position, Quaternion rotation)
    {
        //�w�肵�����O�̃G�t�F�N�g��get
        GameObject vfx = GetPooledObject(name);
        if(vfx != null)
        {
            //�ʒu��]�������ɍ��킹��
            vfx.transform.position = position;
            vfx.transform.rotation = rotation;
            vfx.SetActive(true);
            //LineRenderer�Ƌ��p�Ȃ̂ł����ɏ���
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

    public void Missile(string name, Vector3 position, Quaternion rotation, Transform target, float damage, int layerNum, Vector3 hitPosition, bool isTargeting)
    {
        GameObject missile = GetPooledObject(name);
        MissileBullet missileBullet = missile.GetComponent<MissileBullet>();
        if(missile != null)
        {
            // missileのレイヤーを変更する(7: Missileはignore raycastとの衝突を無視する)
            missile.layer = layerNum;
            // missileのターゲットを与える
            missile.transform.position = position;
            missile.transform.rotation = rotation;
            missile.SetActive(true);

            missileBullet.target = target;
            missileBullet.damage = damage;
            missileBullet.direction = (hitPosition - position).normalized;
            missileBullet.isTargeting = isTargeting;
        }

    }

    private IEnumerator ParticleStop(float delay, GameObject vfx)
    {
        yield return new WaitForSeconds(delay);
        vfx.SetActive(false);
    }
    /// <summary>
    /// LineRenderer�̃G�t�F�N�g�����֐��B
    /// </summary>
    /// <param name="name">ObjectPoolVFX�N���X�ɓo�^���Ă���G�t�F�N�g�̖��O</param>
    /// <param name="points">���Ō��ԑS�Ă̈ʒu�B�z��̏��ɂȂ���</param>
    /// <param name="duration">�G�t�F�N�g��������</param>
    public void PlayLineRenderer(string name, Vector3[] points,float duration)
    {
        GameObject vfx = GetPooledObject(name);
        if (vfx != null)
        {
            //���_�������ɍ��킹��
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
