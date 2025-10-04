using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    private TitleSceneManager titleSceneManager;
    public static TitleManager Instance { get; private set; }
    [SerializeField] private AudioMixer audioMixer;

    public Sprite titleSprite;
    public Sprite titleSprite2;
    public Sprite titleBackSprite;
    public Sprite titleBackSprite2;

    // �y�ǉ��zAudioSource�R���|�[�l���g�ւ̎Q��
    public AudioSource audioSource;
    // �y�ǉ��z�J�[�\���ړ�SE
    public AudioClip moveSE;
    // �y�ǉ��z����SE
    public AudioClip decideSE;

    public Button[] menuButtons;                    // ���j���[�{�^�����i�[����z��
    public Image cursorImage;                       // �J�[�\���摜�iImage�R���|�[�l���g�j
    public float cursorOffset = 50f;                // �J�[�\���ƃ{�^���̊Ԃ̃I�t�Z�b�g�i�����j
    private int currentSelectedButtonIndex = 0;     // ���ݑI�𒆂̃{�^���̃C���f�b�N�X

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
    }

    void Start()
    {
        titleSceneManager = GameObject.Find("TitleSceneManager").GetComponent<TitleSceneManager>();
        //// �J�[�\���摜���ŏ��͔�\���ɂ���
        //if (cursorImage != null)
        //{
        //    cursorImage.gameObject.SetActive(false);
        //}

        // �����I���{�^����ݒ肵�A�J�[�\����z�u
        if (menuButtons.Length > 0)
        {
            SelectButton(0);
        }


        if (GameManager.Instance.IsGameClear)
        {
            titleSceneManager.logoImage.sprite = titleSprite2;
            titleSceneManager.titleBackImage.sprite = titleBackSprite2;
        }
        else
        {
            titleSceneManager.logoImage.sprite = titleSprite;
            titleSceneManager.titleBackImage.sprite = titleBackSprite;
        }
    }

    // �y�ǉ��z���ʉ����Đ�����w���p�[�֐�
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // ���ݍĐ����̉��������Ă��A�d�˂čĐ�����
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
            PlaySound(moveSE);
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
            PlaySound(moveSE);
        }

        // Space�L�[�������ꂽ��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ���ݑI�𒆂̃{�^����OnClick�C�x���g�����s
            if (menuButtons.Length > currentSelectedButtonIndex)
            {
                menuButtons[currentSelectedButtonIndex].onClick.Invoke();
            }
            PlaySound(decideSE);
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
        SceneManager.LoadScene("Sakamoto");
    }

    public void OpenOption()
    {
        if (titleSceneManager == null)
        {
            Debug.LogError("titleSceneManager が見つかりません（破棄されている可能性）");
            return;
        }

        if (titleSceneManager.optionPanel != null)
        {
            titleSceneManager.optionPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("optionPanel が見つかりません（破棄されてる可能性）");
        }

        this.enabled = false;

        if (titleSceneManager.titleUis != null)
        {
            titleSceneManager.titleUis.SetActive(false);
        }
        else
        {
            Debug.LogError("titleUis が見つかりません（破棄されてる可能性）");
        }
    }

    public void CloseOption()
    {
        // ��ʂ����
        titleSceneManager.optionPanel.SetActive(false);

        // �X�V�������J�n
        this.enabled = true;

        // �^�C�g��UI��\��
        titleSceneManager.titleUis.SetActive(true);

        //�{�^���ʒu�����Z�b�g
        //RectTransform buttonRect = menuButtons[0].GetComponent<RectTransform>();
        //float xPos = buttonRect.position.x - buttonRect.rect.width / 2f - cursorOffset;
        //float yPos = buttonRect.position.y; // �{�^���Ɠ�������
        //cursorImage.rectTransform.position = new Vector3(xPos, yPos, 0);
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
        /*  UnityEditor.EditorApplication.isPlaying = false;
         Application.Quit(); */
    }

    public void GetSceneManager()
    {
        titleSceneManager = GameObject.Find("TitleSceneManager").GetComponent<TitleSceneManager>();
        if (GameManager.Instance.IsGameClear)
        {
            titleSceneManager.logoImage.sprite = titleSprite2;
            titleSceneManager.titleBackImage.sprite = titleBackSprite2;
        }
        else
        {
            titleSceneManager.logoImage.sprite = titleSprite;
            titleSceneManager.titleBackImage.sprite = titleBackSprite;
        }
    }
}
