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
        //ˆø”‚ª•ÏŠ·‚³‚ê‚é‰ß’ö‚Å4”Ô–Ú‚ª‚Å‚«‚È‚¢‚Ì‚Å‚±‚±‚Å‚â‚é
        ActiveStateChangeUI(4, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //PartsManager‚©‚ç‚Í0”Ô(“ª)‚Æ5”Ô(‹r)‚ª‘—‚ç‚ê‚é‚±‚Æ‚à‚ ‚é‚Ì‚ÅUI•\¦‚µ‚Ä‚¢‚È‚¢‚»‚ê‚ç‚Í‚±‚±‚Å’e‚­
    //‚±‚Ì‚­‚¾‚è‚ğPartsManager‚É‘‚­‚ÆG‘½‚É‚È‚é‚Æ”»’f‚µ‚½
    private bool IsVaildSlotNumber(int slotNumber)
    {
        return (slotNumber == 0 || slotNumber == 5) ? false : true;
    }
    //‘•”õ˜g‚ÆUI‚Ì˜g‚Í“ª‚ª0”Ô‚Ì•ªˆê‚Â‚¸‚ê‚Ä‚¢‚é‚Ì‚ÅC³
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
