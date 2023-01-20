using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[RequireComponent(typeof(AudioSource))]
public class ChangeSoundVolume : MonoBehaviour
{
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource shiftUIAudioSource;
    public Slider bgmSlider, seSlider;
    [HideInInspector] public string filePath;
    [HideInInspector] public SaveData saveData;
    [SerializeField] List<stageStatus> init;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/" + ".savedata.json";
        StartCoroutine("Initialize");
        // bgmSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    IEnumerator Initialize()
    {
        //jsonファイルがないならSaveData型saveDataを作成、初期値を代入しセーブ。
        if (File.Exists(filePath) == false)
        {
            saveData = new SaveData();
            saveData.bgmVolume = 0.2f;
            saveData.seVolume = 0.2f;
            saveData.stageStatuses = init;
            JSONSave();
        }
        //ファイルがあるならロード、saveDataに代入
        else
        {
            JSONLoad();
        }
        yield return null;

        if (bgmSlider != null && seSlider != null)
        {
            bgmSlider.value = saveData.bgmVolume;
            seSlider.value = saveData.seVolume;
        }
        yield return null;
        if (bgmSource != null)
        {
            bgmSource.volume = saveData.bgmVolume;
            bgmSource.loop = true;
            bgmSource.spatialBlend = 0;
            bgmSource.Play();
        }
        if(shiftUIAudioSource != null)
        {
            shiftUIAudioSource.volume = saveData.seVolume;
        }
    }

    public void JSONSave()
    {
        string json = JsonUtility.ToJson(saveData);
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
        saveData = JsonUtility.FromJson<SaveData>(data);
    }

	// BGMスライドバー値の変更イベント
	public void BGMSliderOnValueChange(float newSliderValue)
	{
        // BGMの音量をスライドバーの値に変更
        bgmSource.volume = newSliderValue;
        // jsonに保存
        saveData.bgmVolume = newSliderValue;
        JSONSave();
	}


	// SEスライドバー値の変更イベント
	public void SESliderOnValueChange(float newSliderValue)
	{
        // SEの音量をスライドバーの値に変更
        shiftUIAudioSource.volume = newSliderValue;
        // jsonに保存
        saveData.seVolume = newSliderValue;
        JSONSave();
	}
}
