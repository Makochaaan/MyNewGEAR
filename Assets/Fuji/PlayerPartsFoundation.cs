using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class PlayerPartsFoundation : MonoBehaviour
{
    public enum PartsType
    {
        Head,
        Body,
        Arms,
        Legs,
        Back,
        Booster,
        Gun,
        Missile
    }
    public PartsType type;
    private Outline outline;

    // Start is called before the first frame update
    void Start()
    {
        InitializeOutline();
    }
    private void InitializeOutline()
    {
        outline = GetComponent<Outline>();
        switch (type)
        {
            case PartsType.Head:
                outline.OutlineColor = Color.white;
                break;
            case PartsType.Body:
                outline.OutlineColor = Color.white;
                break;
            case PartsType.Arms:
                outline.OutlineColor = Color.white;
                break;
            case PartsType.Legs:
                outline.OutlineColor = Color.white;
                break;
            case PartsType.Back:
                outline.OutlineColor = Color.yellow;
                break;
            case PartsType.Booster:
                outline.OutlineColor = Color.blue;
                break;
            case PartsType.Gun:
                outline.OutlineColor = Color.red;
                break;
            case PartsType.Missile:
                outline.OutlineColor = Color.red;
                break;
            default:
                break;
        }
    }

    public void OnEquipped(int slotLevel)
    {
        
    }
    public void OnActivated()
    {
        switch (type)
        {
            case PartsType.Booster:
                break;
            default:
                break;
        }
    }
    public void OnDeactivated()
    {
        switch (type)
        {
            case PartsType.Booster:
                break;
            default:
                break;
        }
    }
    public void UseWeapon()
    {
        switch (type)
        {
            case PartsType.Arms:
                break;
            case PartsType.Back:
                break;
            case PartsType.Gun:
                GetComponent<PlayerWeaponGun>().Fire();
                break;
            case PartsType.Missile:
                break;
            default:
                break;
        }
    }
    public void OnReleased()
    {

    }
}
