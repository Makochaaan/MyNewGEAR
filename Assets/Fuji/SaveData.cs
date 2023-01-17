using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public struct stageStatus
{
    //クリア済みか
    public bool isCleared;
    //クリアタイム
    public float clearTime;
}

[Serializable]
public class SaveData
{
    public float bgmVolume,seVolume;
    public List<stageStatus> stageStatuses;
}