using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Michsky.UI.Shift;
using DG.Tweening;
using UnityEditor.Rendering;

using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private UIManager UIManagerAsset;
    [SerializeField] private Image[] slots;
    [SerializeField] private Sprite activeSlotBackground, inactiveSlotBackground;
    private Image[] slotFrames;
    private TextMeshProUGUI[] slotLevelTexts;
    private Image[] slotIcons;
    public Slider hpSlider, energySlider;

    // Start is called before the first frame update
    void Start()
    {
        InitializeSlotUI();
        SEManager.SharedInstance.PlaySE("ReadyVoice",false,Vector3.zero);
    }
    private void InitializeSlotUI()
    {
        slotFrames = new Image[slots.Length];
        slotLevelTexts = new TextMeshProUGUI[slots.Length];
        slotIcons = new Image[slots.Length];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.GetComponent<Image>().color = UIManagerAsset.primaryColor;
            slotFrames[i] = slots[i].transform.GetChild(0).GetComponent<Image>();
            slotFrames[i].color = UIManagerAsset.primaryColor;
            slotFrames[i].gameObject.SetActive(false);
            slotLevelTexts[i] = slots[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            slotLevelTexts[i].text = "Lv.0";
            slotIcons[i] = slots[i].transform.GetChild(3).GetComponent<Image>();
            slotIcons[i].gameObject.SetActive(false);

            //UIが起動していくアニメーション
            Sequence openSequence = DOTween.Sequence();
            openSequence.Append(slots[i].rectTransform.DOScale(Vector3.one * 0.85f, 1).SetEase(Ease.OutQuint).SetDelay(i*0.5f))
                        .Append(slotLevelTexts[i].DOFade(1, 1));

        }
    }


    //PartsManagerからは0番(頭)と5番(脚)が送られることもあるのでUI表示していないそれらはここで弾く
    //このくだりをPartsManagerに書くと雑多になると判断した
    private bool IsVaildSlotNumber(int slotNumber)
    {
        return (slotNumber == 0 || slotNumber == 5) ? false : true;
    }
    /*
     * 装備枠(配列)とUIで表示する枠番号
     * 頭(0)      表示なし
     * 左腕(1)    配列0番、表示は1番
     * 胴(2)      配列1番、表示は2番
     * 右腕(3)    配列2番、表示は3番
     * 背(4)      配列3番、表示は4番
     * 脚(5)      表示なし
     */

    /*
     * UIで表示している枠について記述するとき、配列での番号でなく表示している番号で記述している
     * (この方が混乱しないよね、どうだろ)
     * 
     */
    /// <summary>
    /// 装備枠での番号をUI表示枠での番号に変換する
    /// </summary>
    /// <param name="slotNumber">装備枠の配列での番号</param>
    /// <returns></returns>
    private int GetSlotNumberForUI(int slotNumber)
    {
        return slotNumber -1;
    }
    //装備をオンオフしたときのUI見た目変更
    public void ActiveStateChangeUI(int slotNumber, bool activate)
    {
        if (IsVaildSlotNumber(slotNumber))
        {
            slots[GetSlotNumberForUI(slotNumber)].sprite = activate ? activeSlotBackground : inactiveSlotBackground;
            slotFrames[GetSlotNumberForUI(slotNumber)].gameObject.SetActive(activate);
            slots[GetSlotNumberForUI(slotNumber)].rectTransform.localScale = activate ? Vector3.one : Vector3.one * 0.85f;
        }
    }
    //装備を変更したときのアイコン変更
    public void ChangeSlotIcon(int slotNumber, Sprite icon)
    {
        if (IsVaildSlotNumber(slotNumber))
        {
            if (icon != null)
            {
                slotIcons[GetSlotNumberForUI(slotNumber)].gameObject.SetActive(true);
                slotIcons[GetSlotNumberForUI(slotNumber)].sprite = icon;
            }
            else
            {
                slotIcons[GetSlotNumberForUI(slotNumber)].gameObject.SetActive(false);
                slotIcons[GetSlotNumberForUI(slotNumber)].sprite = null;
            }
        }
    }
    //枠レベルが変わった時の表示変更
    public void ChangeSlotLevel(int slotNumber, int slotLevel)
    {
        if (IsVaildSlotNumber(slotNumber))
        {
            slotLevelTexts[GetSlotNumberForUI(slotNumber)].text = $"Lv.{slotLevel}";
        }
    }

    //Updateで更新が必要なUIはここへ
    //ObjectPoolUIよりはPlayerUIの機能だと思ったのでここに書いている
    private void Update()
    {
        foreach (PooledUI pooledUI in ObjectPoolUI.sharedInstance.pooledUIs)
        {
            if (pooledUI.uiObject.activeInHierarchy)
            {
                switch (pooledUI.uiObject.name)
                {
                    case "DamageText":
                        pooledUI.myTransform.position = Camera.main.WorldToScreenPoint(pooledUI.positionTemp);
                        break;
                    default:
                        break;
                }

            }
        }

        //突貫タイトルボタン
        if (MoveForPlayer._gameInputs.Player.ToTitle.WasPressedThisFrame())
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
