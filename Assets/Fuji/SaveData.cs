using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public struct stageStatus
{
    //�N���A�ς݂�
    public bool isCleared;
    //�N���A�^�C��
    public float clearTime;
}

[Serializable]
public class SaveData
{
    public float bgmVolume,seVolume;
    public List<stageStatus> stageStatuses;
}