using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private ResultCanvasManager resultCanvasManager;  
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject optionOpenButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    public void StageClear()
    {
        resultCanvasManager.Clear();
    }

    public void Dead()
    {
        resultCanvasManager.Dead();
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
