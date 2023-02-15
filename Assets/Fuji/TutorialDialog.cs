using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TutorialDialog : MonoBehaviour
{
    [SerializeField] private string myDialog;
    [SerializeField] private float time;
    private RectTransform dialogBackground,dialogFrame;
    private TextMeshProUGUI dialogText;

    // Start is called before the first frame update
    void Start()
    {
        dialogBackground = GameObject.Find("DialogBackground").GetComponent<RectTransform>();
        dialogFrame = GameObject.Find("DialogFrame").GetComponent<RectTransform>();
        dialogText = GameObject.Find("DialogText").GetComponent<TextMeshProUGUI>();
        myDialog = myDialog.Replace("\\n", "\n");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            var openSequence = DOTween.Sequence();
            dialogText.text = myDialog;
            openSequence.Append(dialogBackground.DOSizeDelta(new Vector2(400, 200), time).SetEase(Ease.OutQuint))
                        .Join(dialogFrame.DOSizeDelta(new Vector2(400, 200), time).SetEase(Ease.OutQuint))
                        .Append(dialogText.DOFade(1, time));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            var closeSequence = DOTween.Sequence();
            closeSequence.Append(dialogText.DOFade(0, time))
                         .Append(dialogBackground.DOSizeDelta(Vector2.zero, time).SetEase(Ease.OutQuint))
                         .Join(dialogFrame.DOSizeDelta(Vector2.zero, time).SetEase(Ease.OutQuint));
        }
    }
}
