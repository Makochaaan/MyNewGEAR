using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerWeaponMissile : PlayerPartsFoundation
{
#if UNITY_EDITOR
    [CustomEditor(typeof(PlayerPartsFoundation))]
#endif
    private Transform muzzle;
    //連射速度、一発あたりの弾数、二個目以降の集弾性、燃費、射程
    [SerializeField] private float damage, fireRate, bulletCount, energyConsumption, range;
    // public float accuracy;
    private float elapsedTime;
    // private float inaccuracyRatio = 0.125f;
    private int damageLevel = 1;
    // private Vector3 trailEnd;

    private CharacterStatus characterStatus;

    private int missileLayerNum = 7;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        muzzle = transform.GetChild(0);
        outline.OutlineColor = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
    }
    //銃は枠1234どれでもよいのでそのままreturn
    public override int GetEquipSlotNumber(int slotNumber)
    {
        return slotNumber;
    }
    //枠レベルを引数に銃の初期化処理
    public override void OnEquipped(int slotLevel)
    {
        base.OnEquipped(slotLevel);
        characterStatus = transform.root.GetComponent<CharacterStatus>();
        damageLevel = slotLevel <= 6 ? slotLevel : 6;
        damage = (int)(damage * Mathf.Pow(1.15f, damageLevel - 1));
    }
    public override void Use()
    {
        //連射確認
        if (elapsedTime > (float)1 / fireRate)
        {
            // PlayerUI playerUI = transform.root.GetComponent<PlayerUI>();
            for (int i = 0; i < bulletCount; i++)
            {
                if (characterStatus.currentEnergy > energyConsumption)
                {
                    characterStatus.currentEnergy -= energyConsumption;
                    elapsedTime = 0;
                    VFXManager.SharedInstance.PlayVFX($"GunBulletStart{damageLevel}", muzzle.position, muzzle.rotation);
                    Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
                    RaycastHit hit;

                    //射程内の何かに当たったら
                    if (Physics.Raycast(ray, out hit, range))
                    {
                        VFXManager.SharedInstance.Missile("Missile", muzzle.position, muzzle.rotation, hit.transform, damage, missileLayerNum);
                    }

                    //銃声エフェクト
                    SEManager.SharedInstance.PlaySE("GunFire", false, muzzle.position);
                }
                else
                {
                    Debug.Log("out of energy!");
                }
            }
        }
    }
}
