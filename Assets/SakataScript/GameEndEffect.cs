using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameEndEffect : MonoBehaviour
{
    // �ݒ肷��l
    [SerializeField] Image imgBack; // �w�i�摜
    [SerializeField] Image imgEnd;  // �I���摜
    [SerializeField] float fadeDuration = 2.0f;// ���ߗ���0����1�ɕω�����܂ł̕b��
    [SerializeField] float scaleDuration = 2.0f;// �g�嗦��0����1�ɕω�����܂ł̕b��
    [SerializeField] private PlayerManager player;// �v���C���[���Q��

    // �o�ߎ���
    private float elapsedTime = 0f;

    // �ڕW�g��X�P�[��
    private Vector3 targetScale = Vector3.one;

    void Start()
    {
        // ���ߗ���0�ɂ���
        // C#��struct�̃v���p�e�B�𒼐ڕύX����ꍇ�Aimg.color �̃R�s�[�𑀍삵�Ă��܂����߁A
        // ��x color ���擾���A�l��ύX���Ă���Aimg.color �ɖ߂��K�v������܂��B
        Color color = imgBack.color;
        color.a = 0f;
        imgBack.color = color;

        // �I���摜�̖ڕW�X�P�[���̐ݒ�
        targetScale = imgEnd.rectTransform.localScale;

        // �I���摜�̏����X�P�[���̐ݒ�
        imgEnd.rectTransform.localScale = Vector3.zero;

        // �A�^�b�`���Ă���O���[�v��UI�����ׂĔ�\��
        gameObject.SetActive(false);
    }

    void Update()
    {

        //// �摜�̓��ߗ���ݒ肵���b���ɉ����ď㏸������
        //if (!isFading) return;

        //// �o�ߎ��Ԃ����Z
        //elapsedTime += Time.deltaTime;

        //// ���ߗ����v�Z
        //// �o�ߎ��� / �ݒ�b�� �� 0.0f ���� 1.0f �̊Ԃ̒l�it�j�𓾂�
        //float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

        //// ���ߗ����摜�ɓK�p
        //Color color = imgBack.color;
        //color.a = alpha;
        //imgBack.color = color;

        //// ���S�ɕs�����ɂȂ�����Update�������~�߂�
        //if (alpha >= 1.0f)
        //{
        //    enabled = false; // ���̃X�N���v�g��Update���~
        //}
    }

    //  �O������Ă΂�ăt�F�[�h�C�����J�n���邽�߂� public ���\�b�h
    public void StartFadeIn()
    {
        // UI��\��
        gameObject.SetActive(true);

        Debug.Log("�t�F�[�h�J�n");

        // �����J�n
        StartCoroutine(FadeInAndScaleUpSequence());
    }

    // ���ߗ��ω�����g������Ԃɍs���R���[�`��
    private IEnumerator FadeInAndScaleUpSequence()
    {
        // ----------------------------------------
        // 1. ���ߗ���0����1�ɖ߂� (�t�F�[�h�C��)
        // ----------------------------------------

        // �t�F�[�h�C���̃R���[�`�����J�n���A������ҋ@����
        yield return StartCoroutine(FadeImageAlpha(imgBack, 1.0f, fadeDuration));
        Debug.Log("�t�F�[�h�C�������B�g�又���ֈڍs���܂��B");

        // ----------------------------------------
        // 2. �ʂ̉摜���g�傷��
        // ----------------------------------------


        // �g�又���̃R���[�`�����J�n���A������ҋ@����
        yield return StartCoroutine(ScaleRectTransform(imgEnd, targetScale, scaleDuration));
        Debug.Log("�S�ẴA�j���[�V�����V�[�P���X���������܂����B");
    }

    private IEnumerator FadeImageAlpha(Image img, float endAlpha, float duration)
    {
        float startTime = Time.time;
        Color startColor = img.color;
        Color endColor = startColor;
        endColor.a = endAlpha;

        while (Time.time < startTime + duration)
        {
            float timeElapsed = Time.time - startTime;
            float progress = timeElapsed / duration;

            // Color.Lerp�Ŋ��炩�ɒl��ω�������
            img.color = Color.Lerp(startColor, endColor, progress);

            yield return null; // 1�t���[���ҋ@
        }

        // �m���ɖڕW�l�ɐݒ�
        img.color = endColor;
    }

    // �w�肵��RectTransform�̊g�嗦��ڕW�l�܂ŕω�������R���[�`��
    private IEnumerator ScaleRectTransform(Image img, Vector3 endScale, float duration)
    {
        Vector3 startScale = img.rectTransform.localScale;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float timeElapsed = Time.time - startTime;
            float progress = timeElapsed / duration;

            // Vector3.Lerp�Ŋ��炩�ɃX�P�[����ω�������
            img.rectTransform.localScale = Vector3.Lerp(startScale, endScale, progress);

            yield return null; // 1�t���[���ҋ@
        }

        // �m���ɖڕW�X�P�[���ɐݒ�
        img.rectTransform.localScale = endScale;
    }
}