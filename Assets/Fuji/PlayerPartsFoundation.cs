using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class PlayerPartsFoundation : MonoBehaviour
{
    // 装備時に起動するか
    public bool activateOnEquip;
    // (任意)武器スロットに表示するアイコン
    public Sprite slotIcon;
    // 輪郭
    [System.NonSerialized] public Outline outline;
    // 現在起動しているか
    [System.NonSerialized] public bool isActive;
    // パーツのHP、重量
    [SerializeField] private int hp, weight;
    protected CharacterStatus status;
    
    // Start is called before the first frame update
    public virtual void Start()
    {
        outline = GetComponent<Outline>();
        outline.OutlineWidth = 0;
    }
    /// <summary>
    /// 装備枠番号判定関数。プレイヤーが指定した番号を受け取り、それが有効か、何番になるべきか返す
    /// </summary>
    /// <param name="slotNumber"></param>
    /// <returns></returns>
    public virtual int GetEquipSlotNumber(int slotNumber)
    {
        return -1;
    }
    /// <summary>
    /// 装備時処理。位置・回転を装備先に合わせ、HP、重量を追加する
    /// </summary>
    /// <param name="parent">装備先のTransform</param>
    /// <param name="slotLevel">装備枠のレベル</param>
    public virtual void OnEquipped(Transform parent, int slotLevel)
    {
        GetComponent<BoxCollider>().enabled = false;
        transform.position = parent.position;
        transform.transform.rotation = parent.rotation;
        transform.parent = parent;
        //共通して、自機と同じ白色のシルエットをつける
        outline.OutlineColor = Color.white;
        outline.OutlineMode = Outline.Mode.SilhouetteOnly;
        OnActiveStateChange(activateOnEquip);
        //HPと重量を加算
        status = transform.root.GetComponent<CharacterStatus>();
        status.UpdateMaxHP(hp);
        status.weight += weight;
    }
    /// <summary>
    /// オンオフ切り替え関数。オン時の処理、ブースター等、状態変化系装備はここで処理する予定
    /// </summary>
    /// <param name="activate"></param>
    public virtual void OnActiveStateChange(bool activate)
    {
        isActive = activate;
    }
    /// <summary>
    /// パーツの上に装備する処理。現時点では腕パーツの上に武器をつけるのみ
    /// </summary>
    /// <param name="parts">上乗せするパーツ</param>
    /// <param name="slotLevel">装備する枠のレベル</param>
    public virtual void EquipOnParts(PlayerPartsFoundation parts, int slotLevel)
    {

    }
    /// <summary>
    /// パーツ使用関数
    /// </summary>
    public virtual void Use()
    {

    }
    /// <summary>
    /// 装備解除時処理。親子関係を解除、HPと重量を戻し、非表示にする
    /// </summary>
    /// <param name="slotLevel"></param>
    public virtual void OnReleased(int slotLevel)
    {
        status.UpdateMaxHP(-hp);
        status.weight -= weight;
        transform.parent = null;
        gameObject.SetActive(false);
    }
}
