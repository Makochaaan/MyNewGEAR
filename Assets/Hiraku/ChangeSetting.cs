using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Newtonsoft.Json.Serialization;
using Michsky.UI.Shift;

[RequireComponent(typeof(SaveData))]
public class ChangeSetting : MonoBehaviour
{
    private SaveData saveData;
    [SerializeField] private Slider bgmSlider, seSlider, sensitivitySlider, aimFactorSlider;
    [SerializeField] private SwitchManager flipX, flipY;
    [SerializeField] private BGMPlayer bgmPlayer;
    private void Start()
    {
        bgmPlayer = GameObject.Find("BGMSource").GetComponent<BGMPlayer>();
        saveData = GetComponent<SaveData>();
        if (bgmSlider != null && seSlider != null)
        {
            bgmSlider.value = saveData.jsonProperty.bgmVolume;
            seSlider.value = saveData.jsonProperty.seVolume;
        }
        else
        {
            Debug.LogWarning("BGM or SE Slider Not Defined");
        }
        if (sensitivitySlider != null && aimFactorSlider != null)
        {
            sensitivitySlider.value = saveData.jsonProperty.cameraSensitivity;
            aimFactorSlider.value = saveData.jsonProperty.aimFactor;
        }
        else
        {
            Debug.LogWarning("Sensitivity or Aim factor Slider Not Defined ");
        }
        if(flipX != null && flipY != null)
        {
            if (saveData.jsonProperty.flipX == -1) flipX.AnimateSwitch();
            if (saveData.jsonProperty.flipY == -1) flipY.AnimateSwitch();
        }
        else
        {
            Debug.LogWarning("FlipX or FlipY Switch Manager Not Defined ");
        }
    }

    //スライダー値変更時にそのことを記録。セーブ処理は設定画面を閉じるときの1回のみに分離
    public void OnSliderValueChange(string property)
    {
        switch (property)
        {
            case "bgm":
                saveData.jsonProperty.bgmVolume = bgmSlider.value;
                bgmPlayer.BGMVolumeChange(bgmSlider.value);
                break;
            case "se":
                saveData.jsonProperty.seVolume = seSlider.value;
                break;
            case "sensitivity":
                saveData.jsonProperty.cameraSensitivity = sensitivitySlider.value;
                break;
            case "aimfactor":
                saveData.jsonProperty.aimFactor = aimFactorSlider.value;
                break;
            default:
                break;
        }
    }
    public void FlipEnable(string axis)
    {
        switch (axis)
        {
            case "x":
                saveData.jsonProperty.flipX = -1;
                break;
            case "y":
                saveData.jsonProperty.flipY = -1;
                break;
            default:
                break;
        }
    }
    public void FlipDisable(string axis)
    {
        switch (axis)
        {
            case "x":
                saveData.jsonProperty.flipX = 1;
                break;
            case "y":
                saveData.jsonProperty.flipY = 1;
                break;
            default:
                break;
        }
    }
    public void SaveOnBackButton()
    {
        saveData.JSONSave();
    }
}
