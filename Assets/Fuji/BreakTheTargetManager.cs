using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BreakTheTargetManager : MonoBehaviour
{
    // staticインスタンス、Managerともなれば誰もが参照できた方がおいしいのでは
    public static BreakTheTargetManager sharedInstance;
    // 管轄するターゲット、ターゲット発生スクリプトが参照している
    public List<GameObject> targetsList;
    // UI用のターゲット数
    private int targetCount = 0;
    // 時間計測用
    private float start, elapsedTime;
    // UI
    [SerializeField] private TextMeshProUGUI timerText, targetLeftText;
    // タイム計測が始まっているか
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
    // 名前がPlayerのオブジェクトがトリガーを出たらタイム計測スタート
    private void OnTriggerExit(Collider other)
    {
        if(other.name == "Player")
        {
            TimerStart();
        }
    }
    public void TimerStart()
    {
        // タイム計測が始まった最初の一回きり実行
        if (!started)
        {
            // ターゲット数とそれを表すUIを設定
            targetCount = targetsList.Count;
            targetLeftText.text = targetCount.ToString();
            // 開始時刻を格納
            start = Time.time;
            // 2回以上発動しないように対策
            started = true;
            // ｢スタート!｣みたいなボイス再生
            SEManager.SharedInstance.PlayVoice("TimerStartVoice", true);
        }
    }
    private void Update()
    {
        // タイム表示
        if (started)
        {
            timerText.text = TimeSpan.FromSeconds(Time.time - start).ToString(@"mm\:ss\:ff");
        }
    }
    // ターゲットが壊されるたび、ターゲットの状態を調べる
    public void CheckTargetList()
    {
        // ターゲット数の情報を更新
        targetCount--;
        targetLeftText.text = targetCount.ToString();
        // 管轄ターゲットを調べ、一つでもまだあれば終了
        for (int i = 0; i < targetsList.Count; i++)
        {
            if (targetsList[i].activeInHierarchy) return;
        }
        // 一つもターゲットがなければタイム計測終了
        TimerStop();
    }
    public void TimerStop()
    {
        // 現在時刻-開始時刻でタイムが分かる
        elapsedTime = Time.time - start;
        // ランキング表示
        ShowRanking();
        started = false;
        SEManager.SharedInstance.PlayVoice("TimerStopVoice", true);
    }
    public void ShowRanking()
    {
        TimeSpan record;
        record = TimeSpan.FromSeconds(elapsedTime);
        // タイム送信とランキングの表示
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(record);
        // マウス開放
        Cursor.lockState = CursorLockMode.None;
        EventSystem.current.SetSelectedGameObject(GameObject.Find("NameForm"));
    }
}
