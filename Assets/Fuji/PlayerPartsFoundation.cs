using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class PlayerPartsFoundation : MonoBehaviour
{
    [HideInInspector] public Outline outline;
    [HideInInspector] public bool isActive;
    [SerializeField] private int weight;
    

    // Start is called before the first frame update
    public virtual void Start()
    {
        outline = GetComponent<Outline>();
        outline.OutlineWidth = 0;
    }
    public virtual int GetEquipSlotNumber(int slotNumber)
    {
        return 0;
    }

    //枠レベルを引数に装備時処理
    public virtual void OnEquipped(Transform slotTransform, int slotLevel)
    {
        transform.position = slotTransform.position;
        transform.rotation = slotTransform.rotation;
        transform.parent = slotTransform;
        //共通して、自機と同じ白色のシルエットをつける
        outline.OutlineColor = Color.white;
        outline.OutlineMode = Outline.Mode.SilhouetteOnly;
        VFXManager.SharedInstance.PlayVFX($"Equip{(slotLevel <= 6 ? slotLevel : 6)}", transform.root.position, transform.root.rotation);
        //add weight to player parameter script
    }

    //オン時の処理、ブースター等、状態変化系装備はここで処理する予定
    //腕は持っている武器のオンオフを合わせる
    public virtual void OnActivated()
    {

    }
    //オフ時処理
    public virtual void OnDeactivated()
    {

    }
    //武器使用
    public virtual void Use()
    {

    }
    //解除時処理、親を切り離し、非表示に
    public void OnReleased()
    {
        transform.parent = null;
        gameObject.SetActive(false);
    }
}
