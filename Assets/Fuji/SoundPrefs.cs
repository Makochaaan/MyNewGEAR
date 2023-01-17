using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;
using System.IO;

public class SoundPrefs : MonoBehaviour
{
    public AudioSource bgmSource;
    public Slider bgmSlider, seSlider;
    [HideInInspector] public string filePath;
    [HideInInspector] public SaveData saveData;
    [SerializeField] List<stageStatus> init;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/" + ".savedata.json";
        StartCoroutine("Initialize");
        bgmSource = GameObject.Find("BGMPlayer").GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    IEnumerator Initialize()
    {
        //json�t�@�C�����Ȃ��Ȃ�SaveData�^saveData���쐬�A�����l�������Z�[�u�B
        if (File.Exists(filePath) == false)
        {
            saveData = new SaveData();
            saveData.bgmVolume = 0.2f;
            saveData.seVolume = 0.2f;
            saveData.stageStatuses = init;
            JSONSave();
        }
        //�t�@�C��������Ȃ烍�[�h�AsaveData�ɑ��
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
            //bgmSource.DOFade(saveData.bgmVolume, 1);
            //bgmSource.Play();
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
        //�Z�[�u�f�[�^�����[�h
        StreamReader streamReader;
        streamReader = new StreamReader(filePath);
        string data = streamReader.ReadToEnd();
        streamReader.Close();
        saveData = JsonUtility.FromJson<SaveData>(data);
    }
}
