using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MissileBullet : MonoBehaviour
{
    [HideInInspector] public Transform target;
    [HideInInspector] public float damage;
    public float trackFactor;
    private Quaternion targetRotation;
    public Vector3 direction;
    public bool isTargeting;


    // Update is called once per frame
    void Update()
    {
        // HPを持つオブジェクトであった場合
        if(isTargeting && target.gameObject.activeSelf){
            direction = (target.position - transform.position).normalized;
        }
        targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, trackFactor);
        transform.Translate(Vector3.forward * Time.deltaTime * 20);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out HPFoundation hpScript))
        {
            hpScript.Damage(damage, transform.position);
        }

        Debug.Log("trigger enter");
        VFXManager.SharedInstance.PlayVFX("TargetBreak", transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
}
