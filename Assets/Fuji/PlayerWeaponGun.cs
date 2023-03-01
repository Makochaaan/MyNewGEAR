using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerWeaponGun : PlayerPartsFoundation
{
#if UNITY_EDITOR
    [CustomEditor(typeof(PlayerPartsFoundation))]
#endif
    private Transform muzzle;
    //連射速度、一発あたりの弾数、二個目以降の集弾性、燃費、射程
    [SerializeField] private float damage, fireRate, bulletCount, energyConsumption, range;
    public float accuracy;
    private float elapsedTime;
    private float inaccuracyRatio = 0.125f;
    private int damageLevel = 1;
    private Vector3 trailEnd;

    private CharacterStatus characterStatus;

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
            PlayerUI playerUI = transform.root.GetComponent<PlayerUI>();
            for (int i = 0; i < bulletCount; i++)
            {
                if (characterStatus.currentEnergy > energyConsumption)
                {
                    characterStatus.currentEnergy -= energyConsumption;
                    int isMoreThanZero = i > 0 ? 1 : 0;
                    //画面中央に向かってレイ、二個目以降の弾丸は集弾性補正がかかる
                    //集弾性最低の場合、画面の幅1/8ぐらいの半径でブレる
                    elapsedTime = 0;
                    VFXManager.SharedInstance.PlayVFX($"GunBulletStart{damageLevel}", muzzle.position, muzzle.rotation);
                    Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f + isMoreThanZero * Mathf.Max(0, 1 - accuracy) * Random.Range(-inaccuracyRatio, inaccuracyRatio),
                                                                         0.5f + isMoreThanZero * Mathf.Max(0, 1 - accuracy) * Random.Range(-inaccuracyRatio, inaccuracyRatio) * ((float)Screen.width / (float)Screen.height)));
                    RaycastHit hit;

                    //射程内の何かに当たったら
                    if (Physics.Raycast(ray, out hit, range))
                    {
                        //当たったところでレイを止め、エフェクトを出す
                        trailEnd = hit.point;
                        SEManager.SharedInstance.PlaySE("GunHit", true, trailEnd);
                        //当たったところより少しプレイヤー側にするとライトが見えやすい
                        VFXManager.SharedInstance.PlayVFX($"GunBulletEnd{damageLevel}", trailEnd + (Camera.main.transform.position - trailEnd).normalized, Quaternion.identity);

                        //HPのある物に当たったらダメージを与える
                        if (hit.transform.TryGetComponent(out HPFoundation hpScript))
                        {
                            hpScript.Damage(damage, trailEnd);
                        }
                    }
                    //当たらなかったら射程最大で終了する
                    else
                    {
                        trailEnd = Camera.main.transform.position + ray.direction * range;
                    }
                    //弾丸エフェクト
                    VFXManager.SharedInstance.PlayLineRenderer($"GunBulletTrail{damageLevel}", new Vector3[2] { muzzle.position, trailEnd }, 0.1f);

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
