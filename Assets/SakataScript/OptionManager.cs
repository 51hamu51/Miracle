using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionManager : MonoBehaviour
{
    // �ݒ荀��
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject optionPanel;    
    [SerializeField] private GameObject optionOpenButton;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;

    // AudioMixer��Exposed Parameter���ƈ�v������
    private const string BGM_VOLUME_PARAM = "BGMVolume";
    private const string SE_VOLUME_PARAM = "SEVolume";

    private void Start()
    {
        
    }

    public void SetBGMVolume(float volume)
    {
        // volume���f�V�x���l�ɕϊ�
        float decibel = Mathf.Log10(volume) * 20f;

        // 0�̏ꍇ
        if(volume == 0f)
        {
            // �ŏ��l��ݒ�
            decibel = -80f;
        }

        // AudioMixer�ɒl��ݒ�
        audioMixer.SetFloat(BGM_VOLUME_PARAM, decibel);
    }

    public void SetSEVolume(float volume)
    {
        // volume���f�V�x���l�ɕϊ�
        float decibel = Mathf.Log10(volume) * 20f;

        // 0�̏ꍇ
        if (volume == 0f)
        {
            // �ŏ��l��ݒ�
            decibel = -80f;
        }

        // AudioMixer�ɒl��ݒ�
        audioMixer.SetFloat(SE_VOLUME_PARAM, decibel);
    }

    public void OpenOption()
    {
        // �\��������
        optionPanel.SetActive(true);

        // �{�^�����\���ɂ���
        optionOpenButton.SetActive(false);

        // �Q�[�����ꎞ��~
        Time.timeScale = 0f;
    }

    public void CloseOption()
    {
        // ��ʂ����
        optionPanel.SetActive(false);

        // �{�^����\������
        optionOpenButton.SetActive(true);

        // �Q�[�����ĊJ
        Time.timeScale = 1f;
    }
}
