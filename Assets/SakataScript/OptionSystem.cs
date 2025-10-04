using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionSystem : MonoBehaviour
{

    // �ݒ荀��
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;

    // �y�ǉ��zAudioSource�R���|�[�l���g�ւ̎Q��
    private AudioSource audioSource;
    // �y�ǉ��z�J�[�\���ړ�SE
    public AudioClip moveSE;
    // �y�ǉ��z����SE
    public AudioClip decideSE;

    // �I�v�V�������j���[�̑S���� (Button, Slider�Ȃǂ�Selectable�R���|�[�l���g)
    public Selectable[] menuItems;
    public Image cursorImage;
    public float cursorOffset = 50f;
    //public TitleManager titleManager; // �^�C�g����ʂɖ߂邽�߂ɕK�v

    private int currentIndex = 0;

    // �X���C�_�[�̒����X�e�b�v�i���E�L�[�łǂꂾ�����������邩�j
    private const float SLIDER_STEP = 1.0f;

    // AudioMixer��Exposed Parameter���ƈ�v������
    private const string BGM_VOLUME_PARAM = "BGMVolume";
    private const string SE_VOLUME_PARAM = "SEVolume";

    // dB�l���X���C�_�[�l�ɕϊ�����w���p�[�֐����`
    private float DbToSliderValue(float dB)
    {
        // 0dB�ȏ�͍ő�l1�B����ȊO�͑ΐ�����t�Z�B
        if (dB >= 0f) return 1f;

        return Mathf.Clamp01(Mathf.Pow(10f, dB / 20f));
    }

    public void Start()
    {
        audioSource = TitleManager.Instance.audioSource;

        audioMixer.GetFloat(BGM_VOLUME_PARAM, out float bgmVolume);
        bgmSlider.value = DbToSliderValue(bgmVolume);

        audioMixer.GetFloat(SE_VOLUME_PARAM, out float seVolume);
        seSlider.value = DbToSliderValue(seVolume);
        //�����ʒu
        ResetPos();
    }

    // �y�ǉ��z���ʉ����Đ�����w���p�[�֐�
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // ���ݍĐ����̉��������Ă��A�d�˂čĐ�����
        }
    }

    void ResetPos()
    {
        // �����ʒu��0�Ԗڂ̍��ڂ̈ʒu�ɍ��킹��
        RectTransform itemRect = menuItems[0].GetComponent<RectTransform>();

        // �J�[�\���̈ʒu���{�^���̍����ɐݒ�
        float xPos = itemRect.position.x - itemRect.rect.width / 2f - cursorOffset;
        float yPos = itemRect.position.y;

        cursorImage.rectTransform.position = new Vector3(xPos, yPos, 0);
    }

    public void Update()
    {
        // �I�v�V�������ڂ̑I������
        // ----------------------------------------------------
        // 1. �㉺�L�[�ł̃i�r�Q�[�V����
        // ----------------------------------------------------
        bool moved = false;

        // ��L�[
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = menuItems.Length - 1; // ���[�v
            moved = true;

            // ���ʉ����Đ�
            PlaySound(moveSE);
        }
        // ���L�[
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            currentIndex++;
            if (currentIndex >= menuItems.Length) currentIndex = 0; // ���[�v
            moved = true;

            // ���ʉ����Đ�
            PlaySound(moveSE);
        }

        if (moved)
        {
            SelectNewItem(currentIndex);

            // ���ʉ����Đ�
            PlaySound(moveSE);

            // �㉺�ړ�������A�X���C�_�[�̑��샂�[�h���甲���邽�߂ɔO�̂��ߍ��E�L�[���͂����Z�b�g  
            return;
        }


        // ----------------------------------------------------
        // 2. ����/����L�[�̏���
        // ----------------------------------------------------

        // ���ݑI�𒆂̍��ڂ��擾
        Selectable currentItem = menuItems[currentIndex];
        // �I�����ڂ�Slider�̏ꍇ�A���E�L�[�Œl��ύX����
        Slider currentSlider = currentItem as Slider;
        if (currentSlider != null)
        {
            bool sliderChanged = false;
            if (currentSlider != null)
            {
                float currentValue = currentSlider.value;

                // �E�L�[: �l�𑝂₷
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    currentValue += SLIDER_STEP;
                    // �l�͈͓̔��ł��邱�Ƃ�ۏ�
                    currentSlider.value = Mathf.Min(currentValue, currentSlider.maxValue);
                    // ���ʉ����Đ�
                    PlaySound(moveSE);
                    sliderChanged = true;
                }
                // ���L�[: �l�����炷
                else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                {
                    currentValue -= SLIDER_STEP;
                    // �l�͈͓̔��ł��邱�Ƃ�ۏ�
                    currentSlider.value = Mathf.Max(currentValue, currentSlider.minValue);
                    // ���ʉ����Đ�
                    PlaySound(moveSE);
                    sliderChanged = true;
                }
            }

            // Space�L�[: ����
            else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                // Selectable�ɂ�OnClick�C�x���g���Ȃ����߁AButton�R���|�[�l���g�ɃL���X�g����Invoke
                Button currentButton = currentItem as Button;
                if (currentButton != null)
                {
                    // ���ʉ����Đ�
                    PlaySound(decideSE);
                    currentButton.onClick.Invoke();
                }
                // �X���C�_�[���ڂł�Space�L�[�͓��ɉ������Ȃ����Ƃ������ł����A
                // �K�v�ɉ����ăg�O������ʂȏ�����ݒ肷�邱�Ƃ��\�ł��B
            }
            if (sliderChanged)
            {
                if (currentSlider == bgmSlider)
                {
                    TitleManager.Instance.SetBGMVolume(currentSlider.value);
                }
                else if (currentSlider == seSlider)
                {
                    TitleManager.Instance.SetSEVolume(currentSlider.value);
                }
            }
        }
    }

    // �I�����ꂽ���ڂ��n�C���C�g���A�J�[�\�����ړ�������֐�
    private void SelectNewItem(int index)
    {
        if (index < 0 || index >= menuItems.Length) return;

        // EventSystem�ɐV�����I�����ڂ�ݒ� (Button��Highlighted Color�̕ύX�Ɏg�p)
        EventSystem.current.SetSelectedGameObject(menuItems[index].gameObject);

        // �J�[�\���摜��L���ɂ��A�ʒu�𒲐�
        if (cursorImage != null)
        {
            cursorImage.gameObject.SetActive(true);
            RectTransform itemRect = menuItems[index].GetComponent<RectTransform>();

            // �J�[�\���̈ʒu���{�^���̍����ɐݒ�
            float xPos = itemRect.position.x - itemRect.rect.width / 2f - cursorOffset;
            float yPos = itemRect.position.y;

            cursorImage.rectTransform.position = new Vector3(xPos, yPos, 0);
        }
    }

    public void SetBGMVolume(float volume)
    {
        // volume���f�V�x���l�ɕϊ�
        //float decibel = Mathf.Log10(volume) * 20f;

        //// 0�̏ꍇ
        //if (volume == 0f)
        //{
        //    // �ŏ��l��ݒ�
        //    decibel = -80f;
        //}

        // AudioMixer�ɒl��ݒ�
        /* float dB;
        if (volume <= 0.0001f) dB = -80f;
        else dB = Mathf.Log10(volume) * 20f;
        audioMixer.SetFloat("BGMVolume", dB); */
        TitleManager.Instance.SetBGMVolume(bgmSlider.value);
    }




    public void SetSEVolume(float volume)
    {
        //// volume���f�V�x���l�ɕϊ�
        //float decibel = Mathf.Log10(volume) * 20f;

        //// 0�̏ꍇ
        //if (volume == 0f)
        //{
        //    // �ŏ��l��ݒ�
        //    decibel = -80f;
        //}

        // AudioMixer�ɒl��ݒ�
        /* float dB;
        if (volume <= 0.0001f) dB = -80f;
        else dB = Mathf.Log10(volume) * 20f;
        audioMixer.SetFloat(SE_VOLUME_PARAM, dB); */
        TitleManager.Instance.SetSEVolume(bgmSlider.value);
    }

    public void Close()
    {
        PlaySound(decideSE);

        ResetPos();
    }

    public void ChangeTitle()
    {
        // �^�C�g���֖߂�
        SceneManager.LoadScene("TitleScene");
    }
}
