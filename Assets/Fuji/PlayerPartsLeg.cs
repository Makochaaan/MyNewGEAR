using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerPartsLeg : PlayerPartsFoundation
{
#if UNITY_EDITOR
    [CustomEditor(typeof(PlayerPartsFoundation))]
#endif
    private Transform armorUpLegL, armorUpLegR, armorLegL, armorLegR;
    public int speedBonus, energyConsumptionBonus;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        outline.OutlineColor = Color.red;
        armorUpLegL = transform.GetChild(0);
        armorLegL = transform.GetChild(1);
        armorUpLegR = transform.GetChild(2);
        armorLegR = transform.GetChild(3);
    }

    public override int GetEquipSlotNumber(int slotNumber)
    {
        return 5;
    }
    public override void OnEquipped(Transform parent, int slotLevel)
    {
        base.OnEquipped(parent, slotLevel);
        status = transform.root.GetComponent<CharacterStatus>();
        status.energyConsumption -= (energyConsumptionBonus * slotLevel);
        status.speed += (speedBonus * slotLevel);
        foreach (Transform ancestor in parent.parent.GetComponentsInChildren<Transform>())
        {
            if (ancestor.name == "Character1_LeftUpLeg")
            {
                armorUpLegL.position = ancestor.position;
                armorUpLegL.rotation = ancestor.rotation;
                armorUpLegL.parent = ancestor;
                continue;
            }
            if (ancestor.name == "Character1_LeftLeg")
            {
                armorLegL.position = ancestor.position;
                armorLegL.rotation = ancestor.rotation;
                armorLegL.parent = ancestor;
                continue;
            }
            if (ancestor.name == "Character1_RightUpLeg")
            {
                armorUpLegR.position = ancestor.position;
                armorUpLegR.rotation = ancestor.rotation;
                armorUpLegR.parent = ancestor;
                continue;
            }
            if (ancestor.name == "Character1_RightLeg")
            {
                armorLegR.position = ancestor.position;
                armorLegR.rotation = ancestor.rotation;
                armorLegR.parent = ancestor;
                break;
            }
        }
    }
    public override void OnReleased(int slotLevel)
    {
        base.OnReleased(slotLevel);
        status.energyConsumption += (energyConsumptionBonus * slotLevel);
        status.speed -= (speedBonus * slotLevel);
        armorUpLegL.gameObject.SetActive(false);
        armorLegL.gameObject.SetActive(false);
        armorUpLegR.gameObject.SetActive(false);
        armorLegR.gameObject.SetActive(false);
    }
}
