using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using TMPro; //TextMeshProを扱う際に必要


public class TimeManager : MonoBehaviour {

    [SerializeField] TextMeshProUGUI time_text; //TextMeshProの変数宣言
    private float start; // 計測の開始時間を格納する変数
    private float elapsedTime; // 経過時間を格納する変数
    private bool isStoped; // 計測が終了したかどうかを表す変数
    public Button StopButton;

    // Use this for initialization
    void Start () {
        start = Time.time;
        elapsedTime = 0f;
        isStoped = false;

        StopButton.onClick.AddListener(TimerStop);
    }

    // Update is called once per frame
    void Update () {
        if(isStoped) return;
        
        // テキストの表示を入れ替える
        elapsedTime = Time.time-start;
        time_text.text = elapsedTime.ToString();
        
    }

    void TimerStop()
    {
        isStoped = true;
    }
}