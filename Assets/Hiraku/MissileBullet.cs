using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MissileBullet : MonoBehaviour
{
    [HideInInspector] public Transform target;
    [HideInInspector] public float damage;
    public float trackFactor;
    private Quaternion targetRotation;
    private Vector3 direction;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        direction = (target.position - transform.position).normalized;
        targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, trackFactor);
        transform.Translate(Vector3.forward * Time.deltaTime * 10);
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
