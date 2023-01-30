using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerPartsFoundation))]
public class PlayerPartsArm : MonoBehaviour
{
    [HideInInspector] public PartsSlot armSlot;
    private PlayerWeaponGun gunScript;
    [SerializeField] private Transform myHand;
    [SerializeField] private int fireRateBonus, energyConsumptionBonus;
    // Start is called before the first frame update
    void Start()
    {
        armSlot = new PartsSlot();
    }
    public void SetArmStatus(int slotLevel)
    {
        armSlot.slotLevel = slotLevel <= 6 ? slotLevel : 6;
    }
    //腕に武器をつけるときだけ実行
    public void EquipOnArm(PlayerPartsFoundation parts)
    {
        if (armSlot.parts != null)
        {
            armSlot.parts.OnDeactivated();
            armSlot.parts.OnReleased(armSlot.slotLevel);
        }
        parts.transform.position = myHand.position;
        parts.transform.rotation = myHand.rotation;
        parts.transform.parent = myHand;
        armSlot.parts = parts;
        armSlot.slotLevel++;
        armSlot.isActive = true;
        parts.OnEquipped(armSlot.slotLevel);
        parts.OnActivated();
        switch (parts.type)
        {
            case PlayerPartsFoundation.PartsType.Gun:
                gunScript = parts.gameObject.GetComponent<PlayerWeaponGun>();
                break;
            case PlayerPartsFoundation.PartsType.Missile:
                break;
            default:
                break;
        }
    }
    public void ActivateWeapon()
    {
        armSlot.isActive = true;
    }
    public void DeActivateWeapon()
    {
        armSlot.isActive = false;
    }
    public void UseWeapon()
    {
        switch (armSlot.parts.type)
        {
            case PlayerPartsFoundation.PartsType.Gun:
                gunScript.Fire();
                break;
            case PlayerPartsFoundation.PartsType.Missile:
                break;
            default:
                break;
        }
    }
    public void ReleaseWeapon()
    {
        if (armSlot.parts != null) armSlot.parts.OnReleased(armSlot.slotLevel);
    }
}
