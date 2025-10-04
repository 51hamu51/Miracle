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

    public Button[] menuButtons;                    // メニューボタンを格納する配列
    public Image cursorImage;                       // カーソル画像（Imageコンポーネント）
    public float cursorOffset = 50f;                // カーソルとボタンの間のオフセット（距離）
    private int currentSelectedButtonIndex = 0;     // 現在選択中のボタンのインデックス

    // AudioMixerのExposed Parameter名と一致させる
    private const string BGM_VOLUME_PARAM = "BGMVolume";
    private const string SE_VOLUME_PARAM = "SEVolume";

    void Start()
    {
        // カーソル画像を最初は非表示にする
        if (cursorImage != null)
        {
            cursorImage.gameObject.SetActive(false);
        }

        // 初期選択ボタンを設定し、カーソルを配置
        if (menuButtons.Length > 0)
        {
            SelectButton(0);
        }
    }

    void Update()
    {
        // 上キーが押されたら
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentSelectedButtonIndex--;
            if (currentSelectedButtonIndex < 0)
            {
                currentSelectedButtonIndex = menuButtons.Length - 1; // 一番下のボタンへループ
            }
            SelectButton(currentSelectedButtonIndex);
        }

        // 下キーが押されたら
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentSelectedButtonIndex++;
            if (currentSelectedButtonIndex >= menuButtons.Length)
            {
                currentSelectedButtonIndex = 0; // 一番上のボタンへループ
            }
            SelectButton(currentSelectedButtonIndex);
        }

        // Spaceキーが押されたら
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 現在選択中のボタンのOnClickイベントを実行
            if (menuButtons.Length > currentSelectedButtonIndex)
            {
                menuButtons[currentSelectedButtonIndex].onClick.Invoke();
            }
        }
    }

    // ボタンを選択し、カーソルを移動させる関数
    void SelectButton(int index)
    {
        if (index < 0 || index >= menuButtons.Length) return;

        // まず全てのボタンのハイライトを解除（念のため）
        // EventSystem.current.SetSelectedGameObject(null);

        // 指定されたボタンを選択状態にする
        EventSystem.current.SetSelectedGameObject(menuButtons[index].gameObject);
        currentSelectedButtonIndex = index; // 現在の選択インデックスを更新

        // カーソル画像を有効にし、位置を調整
        if (cursorImage != null)
        {
            cursorImage.gameObject.SetActive(true);
            // 選択されたボタンのRectTransformを取得
            RectTransform buttonRect = menuButtons[index].GetComponent<RectTransform>();

            // カーソルの位置をボタンの左側に設定
            // ボタンの左端から左にcursorOffset分移動
            float xPos = buttonRect.position.x - buttonRect.rect.width / 2f - cursorOffset;
            float yPos = buttonRect.position.y; // ボタンと同じ高さ

            cursorImage.rectTransform.position = new Vector3(xPos, yPos, 0);
        }
    }

    // マウスカーソルがボタンに乗った時に呼ばれるイベントハンドラ
    // これにより、マウス操作でもカーソル画像が移動するようになる
    public void OnPointerEnterButton(Button hoveredButton)
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            if (menuButtons[i] == hoveredButton)
            {
                SelectButton(i); // ホバーされたボタンを選択状態にする
                break;
            }
        }
    }


    public void StartGame()
    {
        Debug.Log("ゲームスタート！");
        // (デバッグ：オプション設定シーンへ遷移)
        SceneManager.LoadScene("SakataOption");
    }

    public void OpenOption()
    {
        // オプション画面を開く
        optionPanel.SetActive(true);

        // タイトルUIを非表示
        titleUis.SetActive(false);
    }

    public void CloseOption()
    {
        // 画面を閉じる
        optionPanel.SetActive(false);

        // タイトルUIを表示
        titleUis.SetActive(true);
    }

    public void SetBGMVolume(float volume)
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

    public void ExitGame()
    {
        // Unityエディターでの動作
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
