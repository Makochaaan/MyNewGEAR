using UnityEngine;
using System;
using UnityEngine.Audio;

[RequireComponent(typeof(SoundPrefs))]
public class SEManager : MonoBehaviour
{
    [SerializeField] private SEClass[] soundEffects;
    private SoundPrefs soundPrefs;

    private void Awake()
    {
        soundPrefs = GetComponent<SoundPrefs>();
    }
    //非ループSEを出す
    public void PlaySE(string name,Vector3 position)
    {
        SEClass se = Array.Find(soundEffects, soundEffect => soundEffect.name == name);
        if(se != null)
        {
            AudioSource.PlayClipAtPoint(se.clip, position, soundPrefs.saveData.seVolume);
        }
        else
        {
            Debug.LogWarning($"SE '{name}' not found!");
        }
    }
    //消耗品にループSEをつける
    public void AddSEComponent(string name, GameObject parent)
    {
        SEClass se = Array.Find(soundEffects, soundEffect => soundEffect.name == name);
        if (se != null)
        {
            AudioSource parentAS = parent.AddComponent<AudioSource>();
            parentAS.clip = se.clip;
            parentAS.volume = soundPrefs.saveData.seVolume;
            parentAS.loop = true;
            parentAS.spatialBlend = 1;
            parentAS.Play();
        }
        else
        {
            Debug.LogWarning($"SE '{name}' not found!");
        }
    }
}
