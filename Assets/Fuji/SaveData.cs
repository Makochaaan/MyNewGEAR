using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public struct StageStatus
{
    //クリア済みか
    public bool isCleared;
    //クリアタイム
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
        //jsonファイルがないならSaveData型saveDataを作成、初期値を代入しセーブ。
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
        //ファイルがあるならロード、saveDataに代入
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
        //セーブデータをロード
        StreamReader streamReader;
        streamReader = new StreamReader(filePath);
        string data = streamReader.ReadToEnd();
        streamReader.Close();
        jsonProperty = JsonUtility.FromJson<JsonProperty>(data);
    }

}