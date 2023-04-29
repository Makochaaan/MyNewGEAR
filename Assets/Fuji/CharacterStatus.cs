using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// キャラクターの状態、ステータスを数値で置いておく
// 文章量の都合でUIとは分けた
// HP基底クラスを継承
public class CharacterStatus : HPFoundation
{
#if UNITY_EDITOR
    [CustomEditor(typeof(HPFoundation))]
#endif
    // 最大エネルギー、移動系の燃費、重量
    public int maxEnergy, energyConsumption, weight;
    // 現在エネルギー、エネルギー回復速度、速度、ダッシュ係数
    // Time deltatimeと関わるものはfloatが都合よい
    public float currentEnergy, energyRecoverySpeed, speed, boostFactor;

    // 着地判定用に現在の接地オブジェクトを格納する
    private Transform groundTemp;
    // 接地判定と着地判定
    public bool isOnGround,landedThisFrame;
    RaycastHit hit;

    private PlayerUI myUIManager;

    // Start is called before the first frame update
    void Start()
    {
        // UIを設定
        myUIManager = GetComponent<PlayerUI>();
        if (myUIManager.hpSlider != null &&myUIManager.energySlider != null)
        {
            myUIManager.hpSlider.minValue = 0;
            myUIManager.hpSlider.maxValue = maxHP;
            myUIManager.hpSlider.value = maxHP;
            myUIManager.energySlider.minValue = 0;
            currentEnergy = 0;
            myUIManager.energySlider.maxValue = maxEnergy;
        }
    }
    // 継承したダメージ関数
    public override void Damage(int damage, Vector3 position)
    {
        base.Damage(damage, position);
        myUIManager.hpSlider.value = currentHP;
    }
    /// <summary>
    /// 最大HP変更関数
    /// </summary>
    /// <param name="changeAmount">変化量</param>
    public void UpdateMaxHP(int changeAmount)
    {
        // 現在HPの割合を算出
        float hpRate = (float)currentHP / (float)maxHP;
        maxHP += changeAmount;
        // 最大HP変更後でも変更前の割合を保持する
        // (なので数値としては回復する)
        currentHP = (int)(maxHP * hpRate);
        if (myUIManager != null)
        {
            myUIManager.hpSlider.maxValue = maxHP;
            myUIManager.hpSlider.value = maxHP * hpRate;
        }
    }
    protected override void Update()
    {
        base.Update();
        // 接地判定
        isOnGround = CheckFootState();
        myUIManager.energySlider.value = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }
    private bool CheckFootState()
    {
        //足元に何かあるなら
        if (Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out hit, 0.5f))
        {
            //直前まで何もなかったなら着地、足元を現在の接地面として登録
            if (groundTemp == null)
            {
                landedThisFrame = true;
                groundTemp = hit.transform;
                return true;
            }
            landedThisFrame = false;
            return true;
        }
        //足元に何もないなら
        else
        {
            //着前まで何かあったなら落下またはジャンプ、現在の接地面をnullに
            if (groundTemp != null) groundTemp = null;
            return false;
        }
    }

}
