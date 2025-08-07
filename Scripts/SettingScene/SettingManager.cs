using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider seSlider;
    public Slider textSpeedSlider;

    private const string BGM_KEY = "BGMVolume";
    private const string SE_KEY = "SEVolume";
    private const string TEXT_SPEED_KEY = "TextSpeed";

    void Start()
    {
        // 保存された設定を読み込む
        bgmSlider.value = PlayerPrefs.GetFloat(BGM_KEY, 0.8f);
        seSlider.value = PlayerPrefs.GetFloat(SE_KEY, 0.8f);
        textSpeedSlider.value = PlayerPrefs.GetFloat(TEXT_SPEED_KEY, 0.5f);

        // イベント登録
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        seSlider.onValueChanged.AddListener(SetSEVolume);
        textSpeedSlider.onValueChanged.AddListener(SetTextSpeed);
    }

    void SetBGMVolume(float value)
    {
        PlayerPrefs.SetFloat(BGM_KEY, value);
        // AudioManagerなどがあればここで反映
        Debug.Log("BGM音量: " + value);
    }

    void SetSEVolume(float value)
    {
        PlayerPrefs.SetFloat(SE_KEY, value);
        Debug.Log("SE音量: " + value);
    }

    void SetTextSpeed(float value)
    {
        PlayerPrefs.SetFloat(TEXT_SPEED_KEY, value);
        Debug.Log("テキスト速度: " + value);
    }
}
