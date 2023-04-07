using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerPartsArm : PlayerPartsFoundation
{
#if UNITY_EDITOR
    [CustomEditor(typeof(PlayerPartsFoundation))]
#endif
    private Transform armorArmL,armorArmR,armorForeArmL,armorForeArmR;
    public Transform myHandTransform;
    private PlayerPartsFoundation partsInHand;
    public int damageBonus, energyConsumptionBonus, rangeBonus;
    public float fireRateBonus;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        outline.OutlineColor = Color.red;
        armorArmL = transform.GetChild(0);
        armorForeArmL = transform.GetChild(1);
        armorArmR = transform.GetChild(2);
        armorForeArmR = transform.GetChild(3);
        myHandTransform = transform.GetChild(4);
    }
    public override int GetEquipSlotNumber(int slotNumber)
    {
        return (slotNumber == 1 || slotNumber == 3) ? slotNumber : -1;
    }
    public override void OnEquipped(Transform parent, int slotLevel)
    {
        base.OnEquipped(parent, slotLevel);
        if (parent.name == "ItemSlot_Left")
        {
            foreach (Transform ancestor in transform.GetComponentsInParent<Transform>())
            {
                if (ancestor.name == "Character1_LeftForeArm")
                {
                    armorForeArmL.position = ancestor.position;
                    armorForeArmL.rotation = ancestor.rotation;
                    armorForeArmL.parent = ancestor;
                    continue;
                }
                if (ancestor.name == "Character1_LeftArm")
                {
                    armorArmL.position = ancestor.position;
                    armorArmL.rotation = ancestor.rotation;
                    armorArmL.parent = ancestor;
                    break;
                }
            }
            myHandTransform.position = parent.position;
            myHandTransform.rotation = parent.rotation;
            armorArmR.gameObject.SetActive(false);
            armorForeArmR.gameObject.SetActive(false);
        }
        else if (parent.name == "ItemSlot_Right")
        {
            foreach (Transform ancestor in transform.GetComponentsInParent<Transform>())
            {
                if (ancestor.name == "Character1_RightForeArm")
                {
                    armorForeArmR.position = ancestor.position;
                    armorForeArmR.rotation = ancestor.rotation;
                    armorForeArmR.parent = ancestor;
                    continue;
                }
                if (ancestor.name == "Character1_RightArm")
                {
                    armorArmR.position = ancestor.position;
                    armorArmR.rotation = ancestor.rotation;
                    armorArmR.parent = ancestor;
                    break;
                }
            }
            myHandTransform.position = parent.position;
            myHandTransform.rotation = parent.rotation;
            armorArmL.gameObject.SetActive(false);
            armorForeArmL.gameObject.SetActive(false);
        }
    }
    public override void EquipOnParts(PlayerPartsFoundation parts, int slotLevel)
    {
        //既に手に持ってたら解除
        if(partsInHand != null)
        {
            partsInHand.OnActiveStateChange(false);
            partsInHand.OnReleased(slotLevel);
        }
        partsInHand = parts;
        parts.OnEquipped(transform, slotLevel);
    }
    public override void Use()
    {
        base.Use();
        if (partsInHand != null) partsInHand.Use();
    }
    public override void OnReleased(int slotLevel)
    {
        base.OnReleased(slotLevel);
        armorArmL.gameObject.SetActive(false);
        armorForeArmL.gameObject.SetActive(false);
        armorArmR.gameObject.SetActive(false);
        armorForeArmR.gameObject.SetActive(false);
    }
}
