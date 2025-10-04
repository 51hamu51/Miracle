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
        clearPanel.SetActive(true);
        GameManager.Instance.StageClear();
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
