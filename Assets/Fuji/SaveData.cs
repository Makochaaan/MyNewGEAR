using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public struct StageStatus
{
    //�N���A�ς݂�
    public bool isCleared;
    //�N���A�^�C��
    public float clearTime;
}

[Serializable]
public class JsonProperty
{
    public float bgmVolume, seVolume, cameraSensitivity, aimFactor, flipX, flipY;
    public List<StageStatus> stageStatuses;
}
public class SaveData : MonoBehaviour
{
    [HideInInspector] public string filePath;
    [HideInInspector] public JsonProperty jsonProperty;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/" + ".savedata.json";
        //json�t�@�C�����Ȃ��Ȃ�SaveData�^saveData���쐬�A�����l�������Z�[�u�B
        if (File.Exists(filePath) == false)
        {
            jsonProperty = new JsonProperty();
            jsonProperty.bgmVolume = 0.2f;
            jsonProperty.seVolume = 0.2f;
            jsonProperty.cameraSensitivity = 80;
            jsonProperty.aimFactor = 50;
            jsonProperty.flipX = 1;
            jsonProperty.flipY = 1;
            jsonProperty.stageStatuses = new List<StageStatus>();
            JSONSave();
        }
        //�t�@�C��������Ȃ烍�[�h�AsaveData�ɑ��
        else
        {
            JSONLoad();
        }

    }
    public void JSONSave()
    {
        string json = JsonUtility.ToJson(jsonProperty);
        StreamWriter streamWriter = new StreamWriter(filePath);
        streamWriter.Write(json);
        streamWriter.Flush();
        streamWriter.Close();
    }
    public void JSONLoad()
    {
        //�Z�[�u�f�[�^�����[�h
        StreamReader streamReader;
        streamReader = new StreamReader(filePath);
        string data = streamReader.ReadToEnd();
        streamReader.Close();
        jsonProperty = JsonUtility.FromJson<JsonProperty>(data);
    }

}