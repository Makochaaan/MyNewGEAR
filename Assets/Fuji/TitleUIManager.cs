using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class TitleUIManager : MonoBehaviour
{
    [SerializeField] private Image fadeBlack;
    [SerializeField] private float fadeTime;
    [SerializeField] private RectTransform[] rootButtonsRect;
    [SerializeField] private Button gameStartButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private RectTransform[] gameButtonsRect;
    [SerializeField] private Button timeAttackButton;
    [SerializeField] private RectTransform settingScreen;
    [SerializeField] private Button settingBackButton;

    [SerializeField] private float initialDelay;

    private void Awake()
    {
        foreach (var rect in rootButtonsRect)
        {
            rect.localScale = new Vector3(1, 0, 1);
        }
        foreach (var rect in gameButtonsRect)
        {
            rect.localScale = new Vector3(1, 0, 1);
        }
        settingScreen.localScale = new Vector3(1, 0, 1);
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        foreach (var rect in rootButtonsRect)
        {
            rect.DOScaleY(1, 0.5f).SetDelay(initialDelay).SetEase(Ease.OutExpo);
            Invoke("EnableSelect", initialDelay);
        }
    }
    private void EnableSelect()
    {
        EventSystem.current.SetSelectedGameObject(gameStartButton.gameObject);
    }
    public void OpenOptionScreen()
    {
        EventSystem.current.SetSelectedGameObject(settingBackButton.gameObject);
        foreach (var rect in rootButtonsRect)
        {
            rect.DOScaleY(0, 0.5f).SetEase(Ease.OutExpo);
        }
        settingScreen.DOScaleY(1, 0.5f).SetEase(Ease.OutExpo);
    }
    public void CloseOptionScreen()
    {
        EventSystem.current.SetSelectedGameObject(settingButton.gameObject);
        foreach (var rect in rootButtonsRect)
        {
            rect.DOScaleY(1, 0.5f).SetEase(Ease.OutExpo);
        }
        settingScreen.DOScaleY(0, 0.5f).SetEase(Ease.OutExpo);
    }
    public void OpenGameMenu()
    {
        EventSystem.current.SetSelectedGameObject(timeAttackButton.gameObject);
        foreach (var rect in rootButtonsRect)
        {
            rect.DOScaleY(0, 0.5f).SetEase(Ease.OutExpo);
        }
        foreach (var rect in gameButtonsRect)
        {
            rect.DOScaleY(1, 0.5f).SetEase(Ease.OutExpo);
        }
    }
    public void CloseGameMenu()
    {
        EventSystem.current.SetSelectedGameObject(gameStartButton.gameObject);
        foreach (var rect in rootButtonsRect)
        {
            rect.DOScaleY(1, 0.5f).SetEase(Ease.OutExpo);
        }
        foreach (var rect in gameButtonsRect)
        {
            rect.DOScaleY(0, 0.5f).SetEase(Ease.OutExpo);
        }
    }
    public void ChangeSceneFade()
    {
        EventSystem.current.SetSelectedGameObject(null);
        fadeBlack.raycastTarget = true;
        fadeBlack.DOFade(1,fadeTime).SetEase(Ease.Linear);
        SEManager.SharedInstance.PlaySE("StartVoice", false, Vector3.zero);
    }
}
