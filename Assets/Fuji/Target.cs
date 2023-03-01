using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Target : HPFoundation
{
#if UNITY_EDITOR
    [CustomEditor(typeof(HPFoundation))]
#endif
    [HideInInspector] public RandomSpawn mySpawnScript;
    private void OnEnable()
    {
        VFXManager.SharedInstance.PlayVFX("TargetSpawn", transform.position, Quaternion.identity);
    }
    protected override void OnHPZero()
    {
        mySpawnScript.currentAlive--;
        gameObject.SetActive(false);
        VFXManager.SharedInstance.PlayVFX("TargetBreak", transform.position, Quaternion.identity);
        SEManager.SharedInstance.PlaySE("TargetBreak", false, transform.position);
    }
    protected override void Update()
    {
        base.Update();
        transform.LookAt(Camera.main.transform.position);
    }
}
