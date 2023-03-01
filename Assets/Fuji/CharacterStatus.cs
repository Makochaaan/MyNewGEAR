using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CharacterStatus : HPFoundation
{
#if UNITY_EDITOR
    [CustomEditor(typeof(HPFoundation))]
#endif
    public float maxEnergy, currentEnergy, energyRecoverySpeed, energyConsumption;
    public float weight, speed;
    [SerializeField] private Slider hpSlider, energySlider;

    private Transform groundTemp;
    public bool isOnGround,jumpedThisFrame,landedThisFrame;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        if (hpSlider != null && energySlider != null)
        {
            hpSlider.minValue = 0;
            hpSlider.maxValue = maxHP;
            hpSlider.value = maxHP;
            energySlider.minValue = 0;
            currentEnergy = 0;
            energySlider.maxValue = maxEnergy;
        }
    }
    protected override void Update()
    {
        base.Update();
        isOnGround = CheckFootState();
        if (hpSlider != null && energySlider != null)
        {
            hpSlider.value = Mathf.Clamp(currentHP, 0, maxHP);
            energySlider.value = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        }
    }
    private bool CheckFootState()
    {
        //�����ɉ�������Ȃ�
        if (Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out hit, 0.5f))
        {
            //���O�܂ŉ����Ȃ������Ȃ璅�n
            if (groundTemp == null)
            {
                landedThisFrame = true;
                groundTemp = hit.transform;
                return true;
            }
            landedThisFrame = false;
            return true;
        }
        //�����ɉ����Ȃ��Ȃ�
        else
        {
            //���O�܂ŉ����������Ȃ�W�����v
            if (groundTemp != null)
            {
                groundTemp = null;
                return false;
            }
            return false;
        }
    }
}
