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
            //ActiveStateChangeUI(i, false);

            Sequence openSequence = DOTween.Sequence();
            //����A�j���[�V������K���I�����Ă���J���A�j���[�V����
            openSequence.Append(slots[i].rectTransform.DOScale(Vector3.one * 0.85f, 1).SetEase(Ease.OutQuint).SetDelay(i*0.5f))
                        //.Join(slotFrames[i].rectTransform.DOScale(Vector3.zero,0).SetEase(Ease.OutQuint))
                        .Append(slotLevelTexts[i].DOFade(1, 1));

        }
        //�������ϊ������ߒ���4�Ԗڂ��ł��Ȃ��̂ł����ł��
        //ActiveStateChangeUI(4, false);
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

        //�ˊу^�C�g���{�^��
        if (MoveForPlayer._gameInputs.Player.ToTitle.WasPressedThisFrame())
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
