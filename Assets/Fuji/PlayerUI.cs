using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Michsky.UI.Shift;
//LT用、Pボタンで即タイトルへ
using UnityEngine.SceneManagement;

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
            slotLevelTexts[i] = slots[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            slotLevelTexts[i].text = "Lv.0";
            slotIcons[i] = slots[i].transform.GetChild(3).GetComponent<Image>();
            slotIcons[i].gameObject.SetActive(false);
            ActiveStateChangeUI(i, false);
        }
        //引数が変換される過程で4番目ができないのでここでやる
        ActiveStateChangeUI(4, false);
    }


    //PartsManagerからは0番(頭)と5番(脚)が送られることもあるのでUI表示していないそれらはここで弾く
    //このくだりをPartsManagerに書くと雑多になると判断した
    private bool IsVaildSlotNumber(int slotNumber)
    {
        return (slotNumber == 0 || slotNumber == 5) ? false : true;
    }
    //装備枠とUIの枠は頭が0番の分一つずれているので修正
    private int GetSlotNumberForUI(int slotNumber)
    {
        return slotNumber -1;
    }
    public void ActiveStateChangeUI(int slotNumber, bool activate)
    {
        if (IsVaildSlotNumber(slotNumber))
        {
            slots[GetSlotNumberForUI(slotNumber)].sprite = activate ? activeSlotBackground : inactiveSlotBackground;
            slotFrames[GetSlotNumberForUI(slotNumber)].gameObject.SetActive(activate);
            slots[GetSlotNumberForUI(slotNumber)].rectTransform.localScale = activate ? Vector3.one : Vector3.one * 0.85f;
        }
    }
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
    public void ChangeSlotLevel(int slotNumber, int slotLevel)
    {
        if (IsVaildSlotNumber(slotNumber))
        {
            slotLevelTexts[GetSlotNumberForUI(slotNumber)].text = $"Lv.{slotLevel}";
        }
    }

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
    }
}
