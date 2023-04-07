using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BreakTheTargetManager : MonoBehaviour
{
    public static BreakTheTargetManager sharedInstance;
    public List<GameObject> targetsList;
    private int targetCount = 0;
    private float start, elapsedTime;
    [SerializeField] private TextMeshProUGUI timerText, targetLeftText;
    private bool started = false;
    private void Awake()
    {
        sharedInstance = this;
        targetsList = new List<GameObject>();
    }
    private void Start()
    {
        timerText.text = "00:00:00";
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.name == "Player")
        {
            TimerStart();
        }
    }
    public void TimerStart()
    {
        if (!started)
        {
            targetCount = targetsList.Count;
            targetLeftText.text = targetCount.ToString();
            start = Time.time;
            started = true;
            SEManager.SharedInstance.PlayVoice("TimerStartVoice", true);
        }
    }
    private void Update()
    {
        if (started)
        {
            timerText.text = TimeSpan.FromSeconds(Time.time - start).ToString(@"mm\:ss\:ff");
        }
    }
    public void CheckTargetList()
    {
        targetCount--;
        targetLeftText.text = targetCount.ToString();
        for (int i = 0; i < targetsList.Count; i++)
        {
            if (targetsList[i].activeInHierarchy) return;
        }
        TimerStop();
    }
    public void TimerStop()
    {
        elapsedTime = Time.time - start;
        ShowRanking();
        started = false;
        SEManager.SharedInstance.PlayVoice("TimerStopVoice", true);
    }
    private TimeSpan GetClearTime()
    {
        TimeSpan record;
        record = TimeSpan.FromSeconds(elapsedTime);
        return record;
    }
    public void ShowRanking()
    {
        // ƒ‰ƒ“ƒLƒ“ƒO‚Ì•\Ž¦
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(GetClearTime());
        Cursor.lockState = CursorLockMode.None;
        EventSystem.current.SetSelectedGameObject(GameObject.Find("NameForm"));
    }
}
