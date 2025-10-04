using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionManager : MonoBehaviour
{
    // 設定項目
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject optionPanel;    
    [SerializeField] private GameObject optionOpenButton;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;

    // AudioMixerのExposed Parameter名と一致させる
    private const string BGM_VOLUME_PARAM = "BGMVolume";
    private const string SE_VOLUME_PARAM = "SEVolume";

    private void Start()
    {
        
    }

    public void SetBGMVolume(float volume)
    {
        // volumeをデシベル値に変換
        float decibel = Mathf.Log10(volume) * 20f;

        // 0の場合
        if(volume == 0f)
        {
            // 最小値を設定
            decibel = -80f;
        }

        // AudioMixerに値を設定
        audioMixer.SetFloat(BGM_VOLUME_PARAM, decibel);
    }

    public void SetSEVolume(float volume)
    {
        // volumeをデシベル値に変換
        float decibel = Mathf.Log10(volume) * 20f;

        // 0の場合
        if (volume == 0f)
        {
            // 最小値を設定
            decibel = -80f;
        }

        // AudioMixerに値を設定
        audioMixer.SetFloat(SE_VOLUME_PARAM, decibel);
    }

    public void OpenOption()
    {
        // 表示させる
        optionPanel.SetActive(true);

        // ボタンを非表示にする
        optionOpenButton.SetActive(false);

        // ゲームを一時停止
        Time.timeScale = 0f;
    }

    public void CloseOption()
    {
        // 画面を閉じる
        optionPanel.SetActive(false);

        // ボタンを表示する
        optionOpenButton.SetActive(true);

        // ゲームを再開
        Time.timeScale = 1f;
    }
}
