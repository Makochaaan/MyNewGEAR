using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private int quantity;
    [SerializeField] private bool respawnIfZero,spawnOnStart;
    private List<GameObject> pooledObjects;
    [SerializeField] private BoxCollider spawnArea;
    [SerializeField] private BoxCollider[] NGArea;
    public int currentAlive;

    private void Awake()
    {
        pooledObjects = new List<GameObject>();
        currentAlive = 0;
    }
    private void Start()
    {
        if (spawnOnStart) Spawn(quantity);
    }
    private GameObject GetSpawnObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        GameObject obj = Instantiate(spawnPrefab);
        obj.transform.SetParent(transform, true);
        obj.name = spawnPrefab.name;
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }
    private float RandomInRange(float range)
    {
        return Random.Range(-range * 0.5f, range * 0.5f);
    }
    private bool isValidPosition(Vector3 spawnPosition)
    {
        for (int i = 0; i < NGArea.Length; i++)
        {
            Vector3 NGAreaCenter = transform.position + NGArea[i].center;
            Vector3 NGAreaSize = NGArea[i].size;
            //X‚ªNG”ÍˆÍŠO‚È‚çŽŸ‚ÌNG”ÍˆÍ‚ðŒŸ“¢
            if (spawnPosition.x < NGAreaCenter.x - (NGAreaSize.x * 0.5f) || NGAreaCenter.x + (NGAreaSize.x * 0.5f) < spawnPosition.x)
            {
                continue;
            }
            //NG‚È‚çY‚ðŒŸ“¢
            else
            {
                if (spawnPosition.y < NGAreaCenter.y - (NGAreaSize.y * 0.5f) || NGAreaCenter.y + (NGAreaSize.y * 0.5f) < spawnPosition.y)
                {
                    continue;
                }
                else
                {
                    //XY‚ªNG‚Ì‚Æ‚«Z‚ðŒŸ“¢AZ‚àNG‚È‚çNG”ÍˆÍ“à‚È‚Ì‚Åfalse
                    if (spawnPosition.z < NGAreaCenter.z - (NGAreaSize.z * 0.5f) || NGAreaCenter.z + (NGAreaSize.z * 0.5f) < spawnPosition.z)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        //for‚ð”²‚¯‚ç‚ê‚½‚È‚çNG”ÍˆÍŠO‚È‚Ì‚Å—Ç‚µ‚Æ‚·‚é
        return true;
    }
    public void Spawn(int quantity)
    {
        int count = 0;
        Vector3 areaSize = spawnArea.size;
        while(count < quantity)
        {
            Vector3 spawnPosition = transform.position + spawnArea.center + new Vector3(RandomInRange(areaSize.x), RandomInRange(areaSize.y), RandomInRange(areaSize.z));
            if (isValidPosition(spawnPosition))
            {
                GameObject obj = GetSpawnObject();
                if (obj.TryGetComponent(out Target target)) target.mySpawnScript = this;
                obj.transform.position = spawnPosition;
                obj.SetActive(true);
                currentAlive++;
                if (BreakTheTargetManager.sharedInstance != null) BreakTheTargetManager.sharedInstance.targetsList.Add(obj);
                count++;
            }
        }
    }
    private void Update()
    {
        if (currentAlive != 0)
        {
            return;
        }
        else
        {
            if (respawnIfZero) Spawn(quantity);
        }
    }
}
