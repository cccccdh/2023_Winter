using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider seSlider;

    private void Start()
    {
        // 슬라이더 초기값 설정
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        seSlider.value = PlayerPrefs.GetFloat("SEVolume", 1.0f);

        // 슬라이더 이벤트 연결
        bgmSlider.onValueChanged.AddListener(SoundManager.soundManager.SetBgmVolume);
        seSlider.onValueChanged.AddListener(SoundManager.soundManager.SetSeVolume);
    }
}
