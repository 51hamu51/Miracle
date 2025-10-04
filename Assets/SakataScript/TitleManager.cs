using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject titleUis;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;

    public Button[] menuButtons;                    // ���j���[�{�^�����i�[����z��
    public Image cursorImage;                       // �J�[�\���摜�iImage�R���|�[�l���g�j
    public float cursorOffset = 50f;                // �J�[�\���ƃ{�^���̊Ԃ̃I�t�Z�b�g�i�����j
    private int currentSelectedButtonIndex = 0;     // ���ݑI�𒆂̃{�^���̃C���f�b�N�X

    // AudioMixer��Exposed Parameter���ƈ�v������
    private const string BGM_VOLUME_PARAM = "BGMVolume";
    private const string SE_VOLUME_PARAM = "SEVolume";

    void Start()
    {
        // �J�[�\���摜���ŏ��͔�\���ɂ���
        if (cursorImage != null)
        {
            cursorImage.gameObject.SetActive(false);
        }

        // �����I���{�^����ݒ肵�A�J�[�\����z�u
        if (menuButtons.Length > 0)
        {
            SelectButton(0);
        }
    }

    void Update()
    {
        // ��L�[�������ꂽ��
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentSelectedButtonIndex--;
            if (currentSelectedButtonIndex < 0)
            {
                currentSelectedButtonIndex = menuButtons.Length - 1; // ��ԉ��̃{�^���փ��[�v
            }
            SelectButton(currentSelectedButtonIndex);
        }

        // ���L�[�������ꂽ��
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentSelectedButtonIndex++;
            if (currentSelectedButtonIndex >= menuButtons.Length)
            {
                currentSelectedButtonIndex = 0; // ��ԏ�̃{�^���փ��[�v
            }
            SelectButton(currentSelectedButtonIndex);
        }

        // Space�L�[�������ꂽ��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ���ݑI�𒆂̃{�^����OnClick�C�x���g�����s
            if (menuButtons.Length > currentSelectedButtonIndex)
            {
                menuButtons[currentSelectedButtonIndex].onClick.Invoke();
            }
        }
    }

    // �{�^����I�����A�J�[�\�����ړ�������֐�
    void SelectButton(int index)
    {
        if (index < 0 || index >= menuButtons.Length) return;

        // �܂��S�Ẵ{�^���̃n�C���C�g�������i�O�̂��߁j
        // EventSystem.current.SetSelectedGameObject(null);

        // �w�肳�ꂽ�{�^����I����Ԃɂ���
        EventSystem.current.SetSelectedGameObject(menuButtons[index].gameObject);
        currentSelectedButtonIndex = index; // ���݂̑I���C���f�b�N�X���X�V

        // �J�[�\���摜��L���ɂ��A�ʒu�𒲐�
        if (cursorImage != null)
        {
            cursorImage.gameObject.SetActive(true);
            // �I�����ꂽ�{�^����RectTransform���擾
            RectTransform buttonRect = menuButtons[index].GetComponent<RectTransform>();

            // �J�[�\���̈ʒu���{�^���̍����ɐݒ�
            // �{�^���̍��[���獶��cursorOffset���ړ�
            float xPos = buttonRect.position.x - buttonRect.rect.width / 2f - cursorOffset;
            float yPos = buttonRect.position.y; // �{�^���Ɠ�������

            cursorImage.rectTransform.position = new Vector3(xPos, yPos, 0);
        }
    }

    // �}�E�X�J�[�\�����{�^���ɏ�������ɌĂ΂��C�x���g�n���h��
    // ����ɂ��A�}�E�X����ł��J�[�\���摜���ړ�����悤�ɂȂ�
    public void OnPointerEnterButton(Button hoveredButton)
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            if (menuButtons[i] == hoveredButton)
            {
                SelectButton(i); // �z�o�[���ꂽ�{�^����I����Ԃɂ���
                break;
            }
        }
    }


    public void StartGame()
    {
        Debug.Log("�Q�[���X�^�[�g�I");
        // (�f�o�b�O�F�I�v�V�����ݒ�V�[���֑J��)
        SceneManager.LoadScene("SakataOption");
    }

    public void OpenOption()
    {
        // �I�v�V������ʂ��J��
        optionPanel.SetActive(true);

        // �^�C�g��UI���\��
        titleUis.SetActive(false);
    }

    public void CloseOption()
    {
        // ��ʂ����
        optionPanel.SetActive(false);

        // �^�C�g��UI��\��
        titleUis.SetActive(true);
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

    public void ExitGame()
    {
        // Unity�G�f�B�^�[�ł̓���
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
