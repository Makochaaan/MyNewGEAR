using UnityEngine;
using System;

[RequireComponent(typeof(SoundPrefs))]
public class SEManager : MonoBehaviour
{
    [SerializeField] private SEClass[] soundEffects;
    private SoundPrefs soundPrefs;

    private void Awake()
    {
        soundPrefs = GetComponent<SoundPrefs>();
    }

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
    public void AddSEComponent(string name, GameObject parent)
    {
        SEClass se = Array.Find(soundEffects, soundEffect => soundEffect.name == name);
        if (se != null)
        {
            AudioSource parentAS = parent.AddComponent<AudioSource>();
            parentAS.clip = se.clip;
            parentAS.volume = soundPrefs.saveData.seVolume;
            parentAS.Play();
        }
        else
        {
            Debug.LogWarning($"SE '{name}' not found!");
        }
    }
}
