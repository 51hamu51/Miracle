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

    public void ChangeTitle()
    {
        Debug.Log("ゲームスタート！");
        // タイトルへ戻る
        SceneManager.LoadScene("TitleScene");
    }
}
