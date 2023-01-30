using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//装備枠クラス。オンオフ、場所、枠レベル、担当アイテムの基本スクリプトを格納する。
[System.Serializable]
public class PartsSlot
{
    public bool isActive;
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
        inputActions.Game.ItemSlot1.performed += context => EquipIfPressed(1);
        inputActions.Game.ItemSlot2.performed += context => EquipIfPressed(2);
        inputActions.Game.ItemSlot3.performed += context => EquipIfPressed(3);
        inputActions.Game.ItemSlot4.performed += context => EquipIfPressed(4);
    }
    //装備枠を初期化
    private void InitializePartsSlots()
    {
        for (int i = 0; i < partsSlots.Length; i++)
        {
            partsSlots[i].isActive = false;
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
    //装備枠に対する操作
    private void EquipIfPressed(int slotNumberInArray)
    {
        //有効な選択なし
        if (!selecting)
        {
            //装備が既にあるならオンオフ切り替え、なければreturn
            if (partsSlots[slotNumberInArray].parts != null)
            {
                if (partsSlots[slotNumberInArray].isActive)
                {
                    partsSlots[slotNumberInArray].isActive = false;
                    partsSlots[slotNumberInArray].parts.OnDeactivated();
                }
                else
                {
                    partsSlots[slotNumberInArray].isActive = true;
                    partsSlots[slotNumberInArray].parts.OnActivated();
                }
            }
            return;
        }
        //有効な選択中
        else
        {
            //装備か確認する(1/30時点では装備しかない)
            if(selectionTemp.TryGetComponent(out PlayerPartsFoundation parts))
            {
                //頭、銅、脚、背中専用パーツはその枠へ、その他は指定した枠へ、腕は左右のみ選べる、装着後オンにするか
                switch (parts.type)
                {
                    case PlayerPartsFoundation.PartsType.Head:
                        EquipSwitchFunction(parts, 0, false);
                        break;
                    case PlayerPartsFoundation.PartsType.Body:
                        EquipSwitchFunction(parts, 2, false);
                        break;
                    case PlayerPartsFoundation.PartsType.Arms:
                        if (slotNumberInArray == 1 || slotNumberInArray == 3) EquipSwitchFunction(parts, slotNumberInArray, true);
                        break;
                    case PlayerPartsFoundation.PartsType.Legs:
                        EquipSwitchFunction(parts, 5, false);
                        break;
                    case PlayerPartsFoundation.PartsType.Back:
                        EquipSwitchFunction(parts, 4, false);
                        break;
                    case PlayerPartsFoundation.PartsType.Booster:
                        EquipSwitchFunction(parts, slotNumberInArray, true);
                        break;
                    case PlayerPartsFoundation.PartsType.Gun:
                        EquipSwitchFunction(parts, slotNumberInArray, true);
                        break;
                    case PlayerPartsFoundation.PartsType.Missile:
                        EquipSwitchFunction(parts, slotNumberInArray, true);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    //人型になるにあたり処理を分けた方が良いのでは
    //switchで共通する部分。装着または交換
    private void EquipSwitchFunction(PlayerPartsFoundation parts, int slotNumber,bool activate)
    {
        //既に装備がついている場合
        if (partsSlots[slotNumber].parts != null)
        {
            //腕以外なら解除する。オフ時処理、解除時処理、枠をオフに
            if (partsSlots[slotNumber].parts.type != PlayerPartsFoundation.PartsType.Arms)
            {
                partsSlots[slotNumber].parts.OnDeactivated();
                partsSlots[slotNumber].parts.OnReleased(partsSlots[slotNumber].slotLevel);
                partsSlots[slotNumber].isActive = false;
            }
            //腕に対し、腕の交換か武器の交換か
            else
            {
                //腕に武器をつける
                if (parts.type != PlayerPartsFoundation.PartsType.Arms)
                {
                    //腕に書いてある装備スクリプトを実行
                    //腕固有の機能なのでFoundationを経由しない方が良いとおもった
                    partsSlots[slotNumber].parts.gameObject.GetComponent<PlayerPartsArm>().EquipOnArm(parts);
                    return;
                }
                //腕を変えずに武器を変えてると腕にある枠のレベルだけ上がっているため、腕の交換の場合、腕からスロットレベルを引き継いで解除する
                else
                {
                    partsSlots[slotNumber].slotLevel = partsSlots[slotNumber].parts.gameObject.GetComponent<PlayerPartsArm>().armSlot.slotLevel;
                    partsSlots[slotNumber].parts.OnDeactivated();
                    partsSlots[slotNumber].parts.OnReleased(partsSlots[slotNumber].slotLevel);
                    partsSlots[slotNumber].isActive = false;
                }
            }
        }
        //場所を枠の場所に、親を枠に、スクリプト取得、オンオフ設定、枠レベルUP、装備時処理とオン時処理
        //※腕に武器を装着するとここは実行されない※
        parts.transform.position = partsSlots[slotNumber].slotTransform.position;
        parts.transform.rotation = partsSlots[slotNumber].slotTransform.rotation;
        parts.transform.parent = partsSlots[slotNumber].slotTransform;
        partsSlots[slotNumber].parts = parts;
        partsSlots[slotNumber].isActive = activate;
        partsSlots[slotNumber].slotLevel++;
        parts.OnEquipped(partsSlots[slotNumber].slotLevel);
        if (activate) parts.OnActivated();
    }
    //オン状態の装備を使う
    private void UseActiveWeapon()
    {
        for (int i = 0; i < partsSlots.Length; i++)
        {
            if (partsSlots[i].isActive)
            {
                partsSlots[i].parts.UseWeapon();
            }
        }
    }
}
