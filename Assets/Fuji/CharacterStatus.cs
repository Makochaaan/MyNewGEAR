using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CharacterStatus : HPFoundation
{
#if UNITY_EDITOR
    [CustomEditor(typeof(HPFoundation))]
#endif
    public int maxEnergy, energyConsumption, weight;
    public float currentEnergy, energyRecoverySpeed, speed, boostFactor;

    private Transform groundTemp;
    public bool isOnGround,landedThisFrame;
    RaycastHit hit;

    private PlayerUI myUIManager;

    // Start is called before the first frame update
    void Start()
    {
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
    public override void Damage(int damage, Vector3 position)
    {
        base.Damage(damage, position);
        myUIManager.hpSlider.value = currentHP;
    }
    public void UpdateMaxHP(int changeAmount)
    {
        float hpRate = (float)currentHP / (float)maxHP;
        maxHP += changeAmount;
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
        isOnGround = CheckFootState();
        myUIManager.energySlider.value = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }
    private bool CheckFootState()
    {
        //足元に何かあるなら
        if (Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out hit, 0.5f))
        {
            //直前まで何もなかったなら着地
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
            //着前まで何かあったなら落下またはジャンプ
            if (groundTemp != null) groundTemp = null;
            return false;
        }
    }

}
