using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPartsArm : PlayerPartsFoundation
{
    [HideInInspector] public PartsSlot armSlot;
    [SerializeField] private Transform myHand;
    [SerializeField] private int fireRateBonus, energyConsumptionBonus;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        outline.OutlineColor = Color.red;
        armSlot = new PartsSlot();
    }
    
    public override int GetEquipSlotNumber(int slotNumber)
    {
        return (slotNumber == 1 || slotNumber == 3) ? slotNumber : -1;
    }
    public override void OnEquipped(Transform slotTransform, int slotLevel)
    {
        base.OnEquipped(slotTransform, slotLevel);
        armSlot.slotLevel = slotLevel <= 6 ? slotLevel : 6;
        OnActivated();
    }
    public override void OnActivated()
    {
        base.OnActivated();
        isActive = true;
    }
    //腕に武器をつけるときだけ実行
    public void EquipOnArm(PlayerPartsFoundation parts)
    {
        if (armSlot.parts != null)
        {
            armSlot.parts.OnDeactivated();
            armSlot.parts.OnReleased();
        }
        parts.transform.position = myHand.position;
        parts.transform.rotation = myHand.rotation;
        parts.transform.parent = myHand;
        armSlot.parts = parts;
        armSlot.slotLevel++;
        parts.OnEquipped(myHand,armSlot.slotLevel);
    }
    public override void Use()
    {
        base.Use();
        if(armSlot.parts != null)
        {
            armSlot.parts.Use();
        }
    }
    public void ReleaseWeapon()
    {
        if (armSlot.parts != null) armSlot.parts.OnReleased();
    }
}
