﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerPartsFoundation))]
public class PlayerWeaponGun : MonoBehaviour
{
    private Transform muzzle;
    [SerializeField] private int fireRate, accuracy, energyConsumption, range;
    public float damage;
    private float elapsedTime;
    private int damageLevel = 1;
    private Vector3 trailEnd;

    // Start is called before the first frame update
    void Start()
    {
        muzzle = transform.GetChild(0);
    }
    //枠レベルを引数に銃の初期化処理
    public void SetGunStatus(int slotLevel)
    {
        damageLevel = slotLevel <= 6 ? slotLevel : 6;
        damage *= Mathf.Pow(1.15f, damageLevel - 1);
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
    }
    public void Fire()
    {
        //連射確認
        if(elapsedTime > (float)1/fireRate)
        {
            //画面中央に向かってレイ
            elapsedTime = 0;
            VFXManager.SharedInstance.PlayVFX($"GunBulletStart{damageLevel}", muzzle.position, muzzle.rotation);
            Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f,0.5f));
            RaycastHit hit;

            //射程内の何かに当たったら
            if(Physics.Raycast(ray, out hit,range))
            {
                //当たったところでレイを止め、エフェクトを出す
                trailEnd = hit.point;
                SEManager.SharedInstance.PlaySE("GunHit", trailEnd);
                VFXManager.SharedInstance.PlayVFX($"GunBulletEnd{damageLevel}", trailEnd, Quaternion.identity);
                
                //HPのある物に当たったらダメージを与える、みたいなのを書く
                if (hit.transform.TryGetComponent(out BoxCollider boxCollider))
                {
                    
                }
                else
                {

                }
            }
            //当たらなかったら射程最大で終了する
            else
            {
                trailEnd = Camera.main.transform.position + Camera.main.transform.forward * range;
            }
            //銃声エフェクト、弾丸エフェクト
            SEManager.SharedInstance.PlaySE("GunFire",muzzle.position);
            VFXManager.SharedInstance.PlayLineRenderer($"GunBulletTrail{damageLevel}",new Vector3[2]{muzzle.position,trailEnd},0.1f);

        }
    }
}
