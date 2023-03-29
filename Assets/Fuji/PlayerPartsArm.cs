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
    public Transform myHand;
    private PlayerPartsFoundation partsInHand;
    [SerializeField] private int fireRateBonus, energyConsumptionBonus;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        outline.OutlineColor = Color.red;
    }
    public override int GetEquipSlotNumber(int slotNumber)
    {
        return (slotNumber == 1 || slotNumber == 3) ? slotNumber : -1;
    }
    public override void EquipOnParts(PlayerPartsFoundation parts, int slotLevel)
    {
        //既に手に持ってたら解除
        if(partsInHand != null)
        {
            partsInHand.OnActiveStateChange(false);
            partsInHand.OnReleased();
        }
        partsInHand = parts;
        Debug.Log(parts.gameObject.name + $" level{slotLevel}");
    }
    public override void Use()
    {
        base.Use();
        if(partsInHand != null)
        {
            partsInHand.Use();
        }
    }
}
