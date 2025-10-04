using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultCanvasManager : MonoBehaviour
{
    public GameObject clearPanel;
    public GameObject deadPanel;

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
        GameManager.Instance.StageClear();
        SceneManager.LoadScene("Sakamoto");
    }

    public void Dead()
    {
        deadPanel.SetActive(true);
    }

    public void NextScene()
    {
        SceneManager.LoadScene("Sakamoto");
    }
}
