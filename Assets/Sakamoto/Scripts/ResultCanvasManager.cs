using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultCanvasManager : MonoBehaviour
{
    public GameObject clearPanel;
    public GameObject deadPanel;

    public PlayerManager playerManager;

    void Start()
    {
        clearPanel.SetActive(false);
        deadPanel.SetActive(false);

    }

    void Update()
    {

    }

    public void Clear()
    {
        //パネルを出す場合
        /* clearPanel.SetActive(true);
        GameManager.Instance.StageClear(); */

        //パネルを出さずに直接次のステージへ
        if (GameManager.Instance.clearStageNum < 2)
        {
            GameManager.Instance.StageClear();
            SceneManager.LoadScene("Sakamoto");
        }
        else
        {
            Time.timeScale = 0f;
            playerManager.Eat();
            Debug.Log("EAAAAAAAAAAAAT");
        }


    }

    public void Dead()
    {
        deadPanel.SetActive(true);
    }

    public void NextScene()
    {
        SceneManager.LoadScene("Sakamoto");
    }

    public void BackTitle()
    {
        GameManager.Instance.GameReset();
        GameManager.Instance.IsGameClear = false;
        TitleManager.Instance.ChangeTitleBGM();
        SceneManager.LoadScene("TitleScene");
    }
}
