using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : MonoBehaviour {
    private float start; // 計測の開始時間を格納する変数
    private float elapsedTime; // 経過時間を格納する変数

    // Use this for initialization
    void Start () 
    {
        elapsedTime = 0;
    }
    public void TimerStart()
    {
        start = Time.time;
    }
    public void TimerStop()
    {
        elapsedTime = Time.time - start;
    }
    public TimeSpan GetClearTime()
    {
        TimeSpan record = new TimeSpan();
        record = TimeSpan.FromSeconds(elapsedTime);
        return record;
    }
}