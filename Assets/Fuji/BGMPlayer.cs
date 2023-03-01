using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class BGMPlayer : MonoBehaviour
{
    public static BGMPlayer instance;
    private AudioSource BGMSource;
    [SerializeField] private SaveData saveData;
    [SerializeField] private float initialDelay;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            BGMSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        BGMSource.volume = 0;
        BGMSource.Play();
        BGMSource.DOFade(saveData.jsonProperty.bgmVolume, 1).SetDelay(initialDelay);
    }
    public void BGMVolumeChange(float volume)
    {
        BGMSource.volume = volume;
    }
}