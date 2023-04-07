using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class PlayerPartsFoundation : MonoBehaviour
{
    public bool activateOnEquip;
    public Sprite slotIcon;
    [System.NonSerialized] public Outline outline;
    [System.NonSerialized] public bool isActive;
    [SerializeField] private int hp, weight;
    protected CharacterStatus status;
    
    // Start is called before the first frame update
    public virtual void Start()
    {
        outline = GetComponent<Outline>();
        outline.OutlineWidth = 0;
    }
    public virtual int GetEquipSlotNumber(int slotNumber)
    {
        return -1;
    }
    //パーツの場所と親を枠に変更
    //枠レベルを引数に装備時処理
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

    //オン時の処理、ブースター等、状態変化系装備はここで処理する予定
    //腕は持っている武器のオンオフを合わせる

    public virtual void OnActiveStateChange(bool activate)
    {
        isActive = activate;
    }
    public virtual void EquipOnParts(PlayerPartsFoundation parts, int slotLevel)
    {

    }
    //武器使用
    public virtual void Use()
    {

    }
    //解除時処理、親を切り離し、非表示に
    public virtual void OnReleased(int slotLevel)
    {
        status.UpdateMaxHP(-hp);
        status.weight -= weight;
        transform.parent = null;
        gameObject.SetActive(false);
    }
}
