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
            dialogBackground.DOSizeDelta(new Vector2(400,200), time).SetEase(Ease.OutQuint);
            dialogFrame.DOSizeDelta(new Vector2(400, 200), time).SetEase(Ease.OutQuint);
            StartCoroutine(SetDialogText(myDialog, time));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            dialogBackground.DOSizeDelta(Vector2.zero, time).SetEase(Ease.OutQuint);
            dialogFrame.DOSizeDelta(Vector2.zero, time).SetEase(Ease.OutQuint);
            StartCoroutine(SetDialogText("", 0));
        }
    }
    private IEnumerator SetDialogText(string text,float delay)
    {
        yield return new WaitForSeconds(delay);
        dialogText.text = text;
    }
}
