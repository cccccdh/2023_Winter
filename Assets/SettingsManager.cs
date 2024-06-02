using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider seSlider;

    private void Start()
    {
        // �����̴� �ʱⰪ ����
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        seSlider.value = PlayerPrefs.GetFloat("SEVolume", 1.0f);

        // �����̴� �̺�Ʈ ����
        bgmSlider.onValueChanged.AddListener(SoundManager.soundManager.SetBgmVolume);
        seSlider.onValueChanged.AddListener(SoundManager.soundManager.SetSeVolume);
    }
}
