using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionSystem : MonoBehaviour
{

    // 設定項目
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;

    // 【追加】AudioSourceコンポーネントへの参照
    public AudioSource audioSource;
    // 【追加】カーソル移動SE
    public AudioClip moveSE;
    // 【追加】決定SE
    public AudioClip decideSE;

    // オプションメニューの全項目 (Button, SliderなどのSelectableコンポーネント)
    public Selectable[] menuItems;
    public Image cursorImage;
    public float cursorOffset = 50f;
    //public TitleManager titleManager; // タイトル画面に戻るために必要

    private int currentIndex = 0;

    // スライダーの調整ステップ（左右キーでどれだけ増減させるか）
    private const float SLIDER_STEP = 1.0f;

    // AudioMixerのExposed Parameter名と一致させる
    private const string BGM_VOLUME_PARAM = "BGMVolume";
    private const string SE_VOLUME_PARAM = "SEVolume";

    // dB値をスライダー値に変換するヘルパー関数を定義
    private float DbToSliderValue(float dB)
    {
        // 0dB以上は最大値1。それ以外は対数から逆算。
        if (dB >= 0f) return 1f;

        return Mathf.Clamp01(Mathf.Pow(10f, dB / 20f));
    }

    public void Start()
    {
        //BGM
        audioMixer.GetFloat(BGM_VOLUME_PARAM, out float bgmVolume);
        // 修正デシベル値からスライダー値に変換して代入
        bgmSlider.value = bgmVolume;

        //SE
        audioMixer.GetFloat(SE_VOLUME_PARAM, out float seVolume);
        // 修正デシベル値からスライダー値に変換して代入
        seSlider.value = seVolume;
        //初期位置
        ResetPos();
    }

    // 【追加】効果音を再生するヘルパー関数
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // 現在再生中の音があっても、重ねて再生する
        }
    }

    void ResetPos()
    {
        // 初期位置は0番目の項目の位置に合わせる
        RectTransform itemRect = menuItems[0].GetComponent<RectTransform>();

        // カーソルの位置をボタンの左側に設定
        float xPos = itemRect.position.x - itemRect.rect.width / 2f - cursorOffset;
        float yPos = itemRect.position.y;

        cursorImage.rectTransform.position = new Vector3(xPos, yPos, 0);
    }

    public void Update()
    {
        // オプション項目の選択処理
        // ----------------------------------------------------
        // 1. 上下キーでのナビゲーション
        // ----------------------------------------------------
        bool moved = false;

        // 上キー
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = menuItems.Length - 1; // ループ
            moved = true;

            // 効果音を再生
            PlaySound(moveSE);
        }
        // 下キー
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            currentIndex++;
            if (currentIndex >= menuItems.Length) currentIndex = 0; // ループ
            moved = true;

            // 効果音を再生
            PlaySound(moveSE);
        }

        if (moved)
        {
            SelectNewItem(currentIndex);

            // 効果音を再生
            PlaySound(moveSE);

            // 上下移動したら、スライダーの操作モードから抜けるために念のため左右キー入力をリセット  
            return;
        }


        // ----------------------------------------------------
        // 2. 決定/操作キーの処理
        // ----------------------------------------------------

        // 現在選択中の項目を取得
        Selectable currentItem = menuItems[currentIndex];
        // 選択項目がSliderの場合、左右キーで値を変更する
        Slider currentSlider = currentItem as Slider;
        if (currentSlider != null)
        {
            bool sliderChanged = false;
            if (currentSlider != null)
            {
                float currentValue = currentSlider.value;

                // 右キー: 値を増やす
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    currentValue += SLIDER_STEP;
                    // 値の範囲内であることを保証
                    currentSlider.value = Mathf.Min(currentValue, currentSlider.maxValue);
                    // 効果音を再生
                    PlaySound(moveSE);
                    sliderChanged = true;
                }
                // 左キー: 値を減らす
                else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                {
                    currentValue -= SLIDER_STEP;
                    // 値の範囲内であることを保証
                    currentSlider.value = Mathf.Max(currentValue, currentSlider.minValue);
                    // 効果音を再生
                    PlaySound(moveSE);
                    sliderChanged = true;
                }
            }

            // Spaceキー: 決定
            else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                // SelectableにはOnClickイベントがないため、ButtonコンポーネントにキャストしてInvoke
                Button currentButton = currentItem as Button;
                if (currentButton != null)
                {
                    // 効果音を再生
                    PlaySound(decideSE);
                    currentButton.onClick.Invoke();
                }
                // スライダー項目ではSpaceキーは特に何もしないことが多いですが、
                // 必要に応じてトグルや特別な処理を設定することも可能です。
            }
            if (sliderChanged)
            {
                if (currentSlider == bgmSlider)
                {
                    SetBGMVolume(currentSlider.value);
                }
                else if (currentSlider == seSlider)
                {
                    SetSEVolume(currentSlider.value);
                }
            }
        }
    }

    // 選択された項目をハイライトし、カーソルを移動させる関数
    private void SelectNewItem(int index)
    {
        if (index < 0 || index >= menuItems.Length) return;

        // EventSystemに新しい選択項目を設定 (ButtonのHighlighted Colorの変更に使用)
        EventSystem.current.SetSelectedGameObject(menuItems[index].gameObject);

        // カーソル画像を有効にし、位置を調整
        if (cursorImage != null)
        {
            cursorImage.gameObject.SetActive(true);
            RectTransform itemRect = menuItems[index].GetComponent<RectTransform>();

            // カーソルの位置をボタンの左側に設定
            float xPos = itemRect.position.x - itemRect.rect.width / 2f - cursorOffset;
            float yPos = itemRect.position.y;

            cursorImage.rectTransform.position = new Vector3(xPos, yPos, 0);
        }
    }

    public void SetBGMVolume(float volume)
    {
        // volumeをデシベル値に変換
        //float decibel = Mathf.Log10(volume) * 20f;

        //// 0の場合
        //if (volume == 0f)
        //{
        //    // 最小値を設定
        //    decibel = -80f;
        //}

        // AudioMixerに値を設定
        audioMixer.SetFloat(BGM_VOLUME_PARAM, volume);
    }

    public void SetSEVolume(float volume)
    {
        //// volumeをデシベル値に変換
        //float decibel = Mathf.Log10(volume) * 20f;

        //// 0の場合
        //if (volume == 0f)
        //{
        //    // 最小値を設定
        //    decibel = -80f;
        //}

        // AudioMixerに値を設定
        audioMixer.SetFloat(SE_VOLUME_PARAM, volume);
    }

    public void Close()
    {
        PlaySound(decideSE);

        ResetPos();
    }

    public void ChangeTitle()
    {
        // タイトルへ戻る
        SceneManager.LoadScene("TitleScene");
    }
}
