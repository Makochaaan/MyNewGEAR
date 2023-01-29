using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerPartsFoundation))]
public class PlayerWeaponGun : MonoBehaviour
{
    private Transform muzzle;
    public int fireRate, damage, accuracy, energyConsumption,range;
    private float elapsedTime;
    private Vector3 trailEnd;

    // Start is called before the first frame update
    void Start()
    {
        muzzle = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
    }
    public void Fire()
    {
        if(elapsedTime > (float)1/fireRate)
        {
            elapsedTime = 0;
            VFXManager.SharedInstance.PlayVFX("GunBulletFlash", muzzle.position, muzzle.rotation);
            Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f,0.5f));
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit,range))
            {
                trailEnd = hit.point;
                SEManager.SharedInstance.PlaySE("GunHit", trailEnd);
                VFXManager.SharedInstance.PlayVFX("GunBulletHit", trailEnd, Quaternion.identity);
                if (hit.transform.TryGetComponent(out BoxCollider boxCollider))
                {
                    
                }
                else
                {

                }
            }
            else
            {
                trailEnd = Camera.main.transform.position + Camera.main.transform.forward * range;
            }
            SEManager.SharedInstance.PlaySE("GunFire",muzzle.position);
            VFXManager.SharedInstance.PlayLineRenderer("GunBulletTrail",new Vector3[2]{muzzle.position,trailEnd},0.1f);

        }
    }
}
