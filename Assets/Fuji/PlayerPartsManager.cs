using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//装備枠クラス。現在の場所(腕装備によって変更があるため)、元の場所(腕破壊によって変更があるため)、腕がついているか、枠レベル、担当アイテムの親クラスを格納する。
[System.Serializable]
public class PartsSlot
{
    public Transform slotTransform;
    [HideInInspector] public Transform slotTransformTemp;
    [HideInInspector] public int slotLevel;
    [HideInInspector] public PlayerPartsFoundation parts;
}

public class PlayerPartsManager : MonoBehaviour
{
    private PlayerUI myPlayerUI;
    //入力、直前まで選択中だったもの、現在レイが当たって選択中のもの、有効なアイテムを選択しているか、装備枠
    private GameInputs inputActions;
    private Transform selectionBefore, selectionAfter;
    private bool selecting;
    [SerializeField] private PartsSlot[] partsSlots;

    //初期化
    // Start is called before the first frame update
    void Start()
    {
        InitializeInputAction();
        InitializePartsSlots();
        selectionBefore = null;
        selecting = false;
        myPlayerUI = GetComponent<PlayerUI>();
    }

    //入力を有効化
    private void InitializeInputAction()
    {
        inputActions = new GameInputs();
        inputActions.Enable();
        inputActions.Player.ItemSlot1.performed += context => SlotCheck(1);
        inputActions.Player.ItemSlot2.performed += context => SlotCheck(2);
        inputActions.Player.ItemSlot3.performed += context => SlotCheck(3);
        inputActions.Player.ItemSlot4.performed += context => SlotCheck(4);
    }
    //装備枠を初期化
    private void InitializePartsSlots()
    {
        for (int i = 0; i < partsSlots.Length; i++)
        {
            partsSlots[i].slotLevel = 0;
            partsSlots[i].parts = null;
            partsSlots[i].slotTransformTemp = partsSlots[i].slotTransform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //選択用レイをとばす
        PartsSelectRay();
        //武器使用ボタンを押している間武器使用
        if(inputActions.Player.Fire.ReadValue<float>() == 1)
        {
            UseActiveWeapon();
        }
    }
    private void PartsSelectRay()
    {
        //画面中央に向かってレイ、当たったものを格納する、何も当たらなかったらnull
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        RaycastHit hit;
        selectionAfter = Physics.Raycast(ray, out hit, 100) ? hit.transform : null;

        //レイがアイテムに当たっているときアイテムの輪郭を描画する。もっとシンプルにならないものか
        //選択が変わっていないならreturn
        if (selectionBefore == selectionAfter)
        {
            return;
        }
        //選択が変わっているなら
        else
        {
            //※"レイが当たったオブジェクトが輪郭スクリプトを持っていること"を有効、持っていないことを無効と表記する※

            //前の選択が有効なら輪郭をなくす
            if (selectionBefore != null && selectionBefore.TryGetComponent(out Outline outlineOld))
            {
                outlineOld.OutlineWidth = 0;
            }
            //現在の選択がnullか無効なら選択を更新してreturn
            //BeforeとAfterの組み合わせはnullnull　null無効　有効null　有効無効　無効null　無効無効の6通り
            if (selectionAfter == null || !selectionAfter.TryGetComponent(out Outline outlineNew))
            {
                selecting = false;
                selectionBefore = selectionAfter;
                return;
            }
            //有効なオブジェクトを選択しているなら選択を更新、輪郭を表示する
            //組み合わせはnull有効　有効有効　無効有効
            else
            {
                selecting = true;
                outlineNew.OutlineWidth = 10;
                selectionBefore = selectionAfter;
            }
        }
    }
    //装備枠に対し、装備か交換かオンオフか確認
    private void SlotCheck(int slotNumberInArray)
    {
        //有効な選択なし
        if (!selecting)
        {
            //パーツが枠に既にあるならオンオフ切り替え、なければreturn
            if (partsSlots[slotNumberInArray].parts != null)
            {
                bool currentState = partsSlots[slotNumberInArray].parts.isActive;
                partsSlots[slotNumberInArray].parts.OnActiveStateChange(!currentState);
                myPlayerUI.ActiveStateChangeUI(slotNumberInArray, !currentState);
            }
            return;
        }
        //有効な選択中
        else
        {
            //選択中オブジェクトがパーツなのか確認する(1/30時点では装備しかない)
            if(selectionAfter.TryGetComponent(out PlayerPartsFoundation parts))
            {
                //オブジェクトに対し有効な入力(腕に対し2、等は無効になる)なら装備する
                if (parts.GetEquipSlotNumber(slotNumberInArray) != -1) EquipParts(parts, parts.GetEquipSlotNumber(slotNumberInArray));
            }
        }
    }
    //装備する
    private void EquipParts(PlayerPartsFoundation selectedParts, int slotNumber)
    {
        //現在のパーツ
        PlayerPartsFoundation currentParts;
        bool partsOnParts = false;
        currentParts = null;
        //装備先にパーツがあるなら追加処理
        if (partsSlots[slotNumber].parts != null)
        {
            //現在の装備を取得
            currentParts = partsSlots[slotNumber].parts;
            //腕がついている枠に腕以外のパーツ(武器)をつける場合のみ腕を取得し、装備先を腕の枠に変更
            if (currentParts.gameObject.TryGetComponent(out PlayerPartsArm myArmScript) && !selectedParts.gameObject.TryGetComponent(out PlayerPartsArm selectedArmScript))
            {
                partsOnParts = true;
                partsSlots[slotNumber].slotTransform = myArmScript.myHandTransform;
            }
            //(旧装備/新装備が)腕以外/腕以外、腕以外/腕、腕/腕の場合、古い方を解除する
            //装備を持った腕が交換されるとき枠の場所を元に戻す
            else
            {
                partsSlots[slotNumber].slotTransform = partsSlots[slotNumber].slotTransformTemp;
                partsSlots[slotNumber].parts.OnActiveStateChange(false);
                partsSlots[slotNumber].parts.OnReleased(partsSlots[slotNumber].slotLevel);
            }
        }
        //装備先が空の場合ここから
        //枠レベルアップ、UIに反映
        partsSlots[slotNumber].slotLevel++;
        //パーツに重ね付けならそういう処理
        if (partsOnParts)
        {
            currentParts.EquipOnParts(selectedParts, partsSlots[slotNumber].slotLevel);
            partsSlots[slotNumber].parts.OnActiveStateChange(selectedParts.activateOnEquip);
        }
        //重ね付けでないなら枠の担当パーツを変更
        else
        {
            partsSlots[slotNumber].parts = selectedParts;
        }
        myPlayerUI.ChangeSlotLevel(slotNumber, partsSlots[slotNumber].slotLevel);

        //パーツが装備時に起動するならそうする
        selectedParts.OnActiveStateChange(selectedParts.activateOnEquip);
        myPlayerUI.ActiveStateChangeUI(slotNumber, selectedParts.activateOnEquip);

        //枠のUIを装備したパーツのものにする
        myPlayerUI.ChangeSlotIcon(slotNumber, selectedParts.slotIcon);
        selectedParts.OnEquipped(partsSlots[slotNumber].slotTransform, partsSlots[slotNumber].slotLevel);
        //装備エフェクト
        VFXManager.SharedInstance.PlayVFX($"Equip{(partsSlots[slotNumber].slotLevel <= 6 ? partsSlots[slotNumber].slotLevel : 6)}", transform.position, transform.rotation);
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
