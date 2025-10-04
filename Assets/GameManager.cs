using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// ステージをクリアした回数
    /// </summary>
    public int clearStageNum;


    [SerializeField] private ResultCanvasManager resultCanvasManager;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject optionOpenButton;


    // AudioMixer��Exposed Parameter���ƈ�v������
    private const string BGM_VOLUME_PARAM = "BGMVolume";
    private const string SE_VOLUME_PARAM = "SEVolume";
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        GameReset();
    }

    public void StageClear()
    {
        clearStageNum++;
    }

    public void GameReset()
    {
        clearStageNum = 0;
    }


    public void SetBGMVolume(float volume)
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

    public void ChangeTitle()
    {
        Debug.Log("�Q�[���X�^�[�g�I");
        // �^�C�g���֖߂�
        SceneManager.LoadScene("TitleScene");
    }
}
