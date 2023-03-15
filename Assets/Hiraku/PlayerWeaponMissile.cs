using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

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
    
    // missileレイヤーの番号
    private int missileLayerNum = 7;


    // // ロックオンのコルーチン
    // private Coroutine _lockOnCoroutine;
    // コルーチンが重複しないようにする
    private bool isRunning = false;

    // ロックオン対象とするターゲット
    private List<Transform> lockonTargets = new List<Transform>();
    // ロックオンしてるかどうか
    private bool isTargeting = false;

    private float targetTime = 0;


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

        if(!isRunning)
        {
            TryLockOnCoroutine();
        }
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

                    // ロックオンしているターゲットがあるかどうか
                    while(lockonTargets.Count > 0)
                    {
                        
                        if(lockonTargets.Last().gameObject.activeSelf)
                        {
                            // そのターゲットがまだ壊れていないならばbreak
                            break;
                        }
                        else
                        {
                            // 壊れているならば削除
                            lockonTargets.RemoveAt(lockonTargets.Count-1);
                        }
                    }

                    //射程内の何かに当たったら
                    if (Physics.Raycast(ray, out hit, range))
                    {
                        if(lockonTargets.Count > 0) VFXManager.SharedInstance.Missile("Missile", muzzle.position, muzzle.rotation, lockonTargets.Last(), damage, missileLayerNum, hit.point, true);
                        else VFXManager.SharedInstance.Missile("Missile", muzzle.position, muzzle.rotation, hit.transform, damage, missileLayerNum, hit.point, false);
                        
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

    // ロックオンできる状態か判定
    private void TryLockOnCoroutine()
    {
        
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        RaycastHit hit;

        if
        (
            MoveForPlayer._gameInputs.Player.Aim.ReadValue<float>() == 1   // 右クリックしてる
            && Physics.Raycast(ray, out hit, range)                        // 衝突した
            && hit.transform.TryGetComponent(out HPFoundation hpScript)    // hpを持ったターゲットである
        )    
        {
            // ロックオン状態として登録済みでないならば、コルーチンスタート
            if(!lockonTargets.Find(_target => _target == hit.transform)){
                StartCoroutine("LockOnCoroutine", hit.transform);
            }
        }
    }

    // ロックオンのコルーチン
    IEnumerator LockOnCoroutine(Transform target)
    {
        isRunning = true;
        int cnt = 10;
        while(cnt > 0){
            // 残り時間が0以上の場合はタイマーを更新　
            yield return new WaitForSeconds(0.1f);
            /*右クリックかつターゲットを狙っているならば継続*/
            /*そうでないならば、中断*/
            Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            RaycastHit hit;
            if(
                !(MoveForPlayer._gameInputs.Player.Aim.ReadValue<float>() == 1)  // 右クリックしてない
                || !Physics.Raycast(ray, out hit, range)                         // 何にも衝突しない
                || hit.transform != target)                                      // ターゲットが外れている
            {
                isRunning = false;
                yield break;
            }
            cnt--;
        }

        /*ここまできたならロックオン状態として登録*/
        if(!lockonTargets.Find(_target => _target == target)){
            Debug.Log("add target");
            lockonTargets.Add(target);
        }
        isRunning = false;
    }
   
}

