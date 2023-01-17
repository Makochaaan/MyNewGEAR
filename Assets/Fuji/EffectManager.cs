using UnityEngine;
using System;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private VFXClass[] effects;

    public void PlayVFX(string name, Vector3 position, Quaternion rotation)
    {
        VFXClass fx = Array.Find(effects, effect => effect.name == name);
        if(fx != null)
        {
            Instantiate(fx.effectPrefab, position, rotation);
        }
        else
        {
            Debug.LogWarning($"effect '{name}' not found!");
            return;
        }
    }
}
