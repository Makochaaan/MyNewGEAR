using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 腕パーツ
// パーツ基底クラスを継承
public class PlayerPartsArm : PlayerPartsFoundation
{
#if UNITY_EDITOR
    [CustomEditor(typeof(PlayerPartsFoundation))]
#endif
    // 腕パーツを左右・前腕・上腕で分けて格納する
    private Transform armorArmL,armorArmR,armorForeArmL,armorForeArmR;
    // 手のTransform。素体(ユニティちゃん)の手を直接使うよりも、
    // 腕パーツについている手を使った方が拡張性が高いかつ管理が楽だと思う
    public Transform myHandTransform;
    // 腕パーツに装備しているパーツ
    private PlayerPartsFoundation partsInHand;
    // 腕パーツ固有のダメージバフ、武器燃費バフ、射程バフ
    public int damageBonus, energyConsumptionBonus, rangeBonus;
    // 腕パーツ固有の連射バフ
    public float fireRateBonus;

    // 輪郭線を設定
    // パーツを分けて登録
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
    // 継承した、装備枠番号判定関数
    // 1か3なら有効な枠としてそのまま返し、それ以外は無効として-1を返す
    public override int GetEquipSlotNumber(int slotNumber)
    {
        return (slotNumber == 1 || slotNumber == 3) ? slotNumber : -1;
    }
    // 継承した、装備時関数
    public override void OnEquipped(Transform parent, int slotLevel)
    {
        base.OnEquipped(parent, slotLevel);
        // 装備先が左腕なら
        if (parent.name == "ItemSlot_Left")
        {
            // 装備先の親をたどって、左前腕・左上腕にパーツを結び付ける
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
            // 手の位置・回転を合わせる
            myHandTransform.position = parent.position;
            myHandTransform.rotation = parent.rotation;
            // 右腕パーツを非表示にする
            armorArmR.gameObject.SetActive(false);
            armorForeArmR.gameObject.SetActive(false);
        }
        // 右腕もノリは上記の左と同様
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
    // 腕パーツをつけた腕に武器をつけた時
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
    // 装備解除時関数
    public override void OnReleased(int slotLevel)
    {
        base.OnReleased(slotLevel);
        armorArmL.gameObject.SetActive(false);
        armorForeArmL.gameObject.SetActive(false);
        armorArmR.gameObject.SetActive(false);
        armorForeArmR.gameObject.SetActive(false);
    }
}
