using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//装備枠クラス。場所、枠レベル、担当アイテムの親クラスを格納する。
[System.Serializable]
public class PartsSlot
{
    public Transform slotTransform;
    public int slotLevel;
    public PlayerPartsFoundation parts;
}

public class PlayerPartsManager : MonoBehaviour
{
    //入力、現在レイが当たって選択中となっているもの、有効なアイテムを選択しているか、装備枠
    private Debug_Player inputActions;
    private Transform selectionTemp;
    private bool selecting;
    [SerializeField] private PartsSlot[] partsSlots;

    //初期化
    // Start is called before the first frame update
    void Start()
    {
        InitializeInputAction();
        InitializePartsSlots();
    }

    //入力を有効化
    private void InitializeInputAction()
    {
        inputActions = new Debug_Player();
        inputActions.Enable();
        inputActions.Game.ItemSlot1.performed += context => SlotCheck(1);
        inputActions.Game.ItemSlot2.performed += context => SlotCheck(2);
        inputActions.Game.ItemSlot3.performed += context => SlotCheck(3);
        inputActions.Game.ItemSlot4.performed += context => SlotCheck(4);
    }
    //装備枠を初期化
    private void InitializePartsSlots()
    {
        for (int i = 0; i < partsSlots.Length; i++)
        {
            partsSlots[i].slotLevel = 0;
            partsSlots[i].parts = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //選択用レイをとばす
        PartsSelectRay();
        //押している間武器使用
        if(inputActions.Game.Fire.ReadValue<float>() == 1)
        {
            UseActiveWeapon();
        }
    }
    private void PartsSelectRay()
    {
        //レイがアイテムに当たっているときアイテムの輪郭を描画する。もっとシンプルにならないものか
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit,100))
        {
            //選択中オブジェクトが現在のレイヒットと同じならreturn、異なるなら輪郭表示を切り替える
            if (selectionTemp == hit.transform)
            {
                return;
            }
            //選択が変わったなら
            else
            {
                //選択中オブジェクトがあり、輪郭スクリプトがあるなら非表示に。nullから遷移したとき用にnullチェックが要る
                if (selectionTemp != null && selectionTemp.TryGetComponent(out Outline outlineOld))
                {
                    outlineOld.OutlineWidth = 0;
                }
                //レイヒットが有効なアイテムなら輪郭表示、その真偽値をtrueに
                if (hit.transform != null && hit.transform.TryGetComponent(out Outline outlineNew))
                {
                    selecting = true;
                    outlineNew.OutlineWidth = 10;
                }
                //レイヒットが無効ならfalse
                else
                {
                    selecting = false;
                }
                //選択中オブジェクトを更新
                selectionTemp = hit.transform;
            }
        }
        
        
    }
    //装備枠に対し、装備か交換かオンオフか確認
    private void SlotCheck(int slotNumberInArray)
    {
        //有効な選択なし
        if (!selecting)
        {
            //装備が既にあるならオンオフ切り替え、なければreturn
            if (partsSlots[slotNumberInArray].parts != null)
            {
                if (partsSlots[slotNumberInArray].parts.isActive)
                {
                    partsSlots[slotNumberInArray].parts.isActive = false;
                    partsSlots[slotNumberInArray].parts.OnDeactivated();
                }
                else
                {
                    partsSlots[slotNumberInArray].parts.isActive = true;
                    partsSlots[slotNumberInArray].parts.OnActivated();
                }
            }
            return;
        }
        //有効な選択中
        else
        {
            //選択中オブジェクトが装備なのか確認する(1/30時点では装備しかない)
            if(selectionTemp.TryGetComponent(out PlayerPartsFoundation parts))
            {
                //オブジェクトに対し有効な入力か確認(腕に対し2、等は無効になる)
                if (parts.GetEquipSlotNumber(slotNumberInArray) != -1)
                {
                    PlayerPartsFoundation myParts = partsSlots[parts.GetEquipSlotNumber(slotNumberInArray)].parts;
                    //装備先の枠に装備がついているなら
                    if (myParts != null)
                    {
                        //ついているのが腕なら
                        if (myParts.gameObject.TryGetComponent(out PlayerPartsArm myArm))
                        {
                            //つけようとするのが腕なら交換、腕のレベルを枠に引き継ぐ
                            if (parts.gameObject.TryGetComponent(out PlayerPartsArm selectedArm))
                            {
                                partsSlots[slotNumberInArray].slotLevel = myArm.armSlot.slotLevel;
                                EquipParts(parts, parts.GetEquipSlotNumber(slotNumberInArray), true);
                            }
                            //つけようとするのが腕以外なら(1/30時点では装備しかない)腕に装備
                            else
                            {
                                myArm.EquipOnArm(parts);
                            }
                        }
                        //ついているのが腕でないなら交換
                        else
                        {
                            EquipParts(parts, parts.GetEquipSlotNumber(slotNumberInArray), true);
                        }
                    }
                    //ついていないなら装備
                    else
                    {
                        EquipParts(parts, parts.GetEquipSlotNumber(slotNumberInArray), false);
                    }
                }
            }
        }
    }
    //装備か交換か
    private void EquipParts(PlayerPartsFoundation parts, int slotNumber, bool releaseOldParts)
    {
        if (releaseOldParts)
        {
            partsSlots[slotNumber].parts.OnDeactivated();
            partsSlots[slotNumber].parts.OnReleased();
        }
        partsSlots[slotNumber].slotLevel++;
        parts.OnEquipped(partsSlots[slotNumber].slotTransform, partsSlots[slotNumber].slotLevel);
        partsSlots[slotNumber].parts = parts;
    }
    //オン状態の装備を使う
    private void UseActiveWeapon()
    {
        for (int i = 0; i < partsSlots.Length; i++)
        {
            if (partsSlots[i].parts != null && partsSlots[i].parts.isActive)
            {
                partsSlots[i].parts.Use();
            }
        }
    }
}
