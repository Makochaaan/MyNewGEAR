using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class PlayerPartsFoundation : MonoBehaviour
{
    //装備タイプ、輪郭スクリプト、重量(1/30時点で使っていない。自機の速度に影響、とかやってみる?)
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
    [SerializeField] private int weight;

    //装備の専用スクリプト。要改善
    private PlayerPartsArm armScript;
    private PlayerWeaponGun gunScript;
    

    // Start is called before the first frame update
    void Start()
    {
        InitializeOutline();
    }
    //輪郭初期設定
    private void InitializeOutline()
    {
        outline = GetComponent<Outline>();
        outline.OutlineWidth = 0;
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

    //枠レベルを引数に装備時処理
    public void OnEquipped(int slotLevel)
    {
        //共通して、自機と同じ白色のシルエットをつける
        outline.OutlineColor = Color.white;
        outline.OutlineMode = Outline.Mode.SilhouetteOnly;
        //枠レベルに対応した装備エフェクト
        VFXManager.SharedInstance.PlayVFX($"Equip{(slotLevel <= 6 ? slotLevel : 6)}", transform.root.position, transform.root.rotation);
        //add weight to player parameter script

        //各種武器の初期設定とスクリプト取得
        switch (type)
        {
            case PartsType.Head:
                break;
            case PartsType.Body:
                break;
            case PartsType.Arms:
                armScript = GetComponent<PlayerPartsArm>();
                armScript.SetArmStatus(slotLevel);
                break;
            case PartsType.Legs:
                break;
            case PartsType.Back:
                break;
            case PartsType.Booster:
                break;
            case PartsType.Gun:
                gunScript = GetComponent<PlayerWeaponGun>();
                gunScript.SetGunStatus(slotLevel);
                break;
            case PartsType.Missile:
                break;
            default:
                break;
        }
    }
    //オン時の処理、ブースター等、状態変化系装備はここで処理する予定
    //腕は持っている武器のオンオフを合わせる
    public void OnActivated()
    {
        switch (type)
        {
            case PartsType.Arms:
                armScript.ActivateWeapon();
                break;
            case PartsType.Booster:
                break;
            default:
                break;
        }
    }
    //オフ時処理
    public void OnDeactivated()
    {
        switch (type)
        {
            case PartsType.Arms:
                armScript.DeActivateWeapon();
                break;
            case PartsType.Booster:
                break;
            default:
                break;
        }
    }
    //武器使用
    //腕が二段構えなのゆるして
    public void UseWeapon()
    {
        switch (type)
        {
            case PartsType.Arms:
                armScript.UseWeapon();
                break;
            case PartsType.Back:
                break;
            case PartsType.Gun:
                gunScript.Fire();
                break;
            case PartsType.Missile:
                break;
            default:
                break;
        }
    }
    //解除時処理、解除エフェクト(要る?)、親を切り離し、非表示に
    public void OnReleased(int slotLevel)
    {
        VFXManager.SharedInstance.PlayVFX($"Release{(slotLevel <= 6 ? slotLevel : 6)}", transform.position, Quaternion.identity);
        transform.parent = null;
        gameObject.SetActive(false);
        if (type == PartsType.Arms) gameObject.GetComponent<PlayerPartsArm>().ReleaseWeapon();
    }
}
