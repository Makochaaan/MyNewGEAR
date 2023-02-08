using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Michsky.UI.Shift;
using System.Runtime.CompilerServices;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private UIManager UIManagerAsset;
    [SerializeField] private Image[] slots;
    [SerializeField] private Sprite activeSlotBackground, inactiveSlotBackground;
    private Image[] slotFrames;
    private TextMeshProUGUI[] slotLevelTexts;
    private Image[] slotIcons;


    // Start is called before the first frame update
    void Start()
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
        //�������ϊ������ߒ���4�Ԗڂ��ł��Ȃ��̂ł����ł��
        ActiveStateChangeUI(4, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //PartsManager�����0��(��)��5��(�r)�������邱�Ƃ�����̂�UI�\�����Ă��Ȃ������͂����Œe��
    //���̂������PartsManager�ɏ����ƎG���ɂȂ�Ɣ��f����
    private bool IsVaildSlotNumber(int slotNumber)
    {
        return (slotNumber == 0 || slotNumber == 5) ? false : true;
    }
    //�����g��UI�̘g�͓���0�Ԃ̕������Ă���̂ŏC��
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
    public void ChangeSlotLevel(int slotNumber, int slotLevel)
    {
        if (IsVaildSlotNumber(slotNumber))
        {
            slotLevelTexts[GetSlotNumberForUI(slotNumber)].text = $"Lv.{slotLevel}";
        }
    }
}
