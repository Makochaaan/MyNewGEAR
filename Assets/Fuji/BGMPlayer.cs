using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class BGMPlayer : MonoBehaviour
{
    // このクラスのインスタンス
    public static BGMPlayer instance;
    // 音源
    private AudioSource BGMSource;
    // 音量を保存したセーブデータ
    private SaveData saveData;
    // 最初(ゲーム開始時)何秒待って再生し始めるか
    [SerializeField] private float initialDelay;
    private void Awake()
    {
        // インスタンスがないならこれを登録、これをシーン変更で破壊しないよう設定する
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            BGMSource = GetComponent<AudioSource>();
        }
        // インスタンスがもうあるなら自壊、BGMがいくつもなるのを防ぐ
        else
        {
            Destroy(gameObject);
        }
    }
    // セーブデータから音量を取得、そこへ向かって音量を徐々に上げる
    private void Start()
    {
        saveData = GameObject.Find("SaveData").GetComponent<SaveData>();
        BGMSource.volume = 0;
        BGMSource.Play();
        BGMSource.DOFade(saveData.jsonProperty.bgmVolume, 1).SetDelay(initialDelay);
    }
    // 音量変更あった時用
    public void BGMVolumeChange(float volume)
    {
        BGMSource.volume = volume;
    }
}