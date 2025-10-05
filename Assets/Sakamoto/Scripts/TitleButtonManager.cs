using UnityEngine;

public class TitleButtonManager : MonoBehaviour
{
    public GameObject secretButton1;
    public GameObject secretButton2;
    void Start()
    {
        if (GameManager.Instance.IsGameClear)
        {
            secretButton1.SetActive(false);
            secretButton2.SetActive(true);
        }
        else
        {
            secretButton2.SetActive(false);
            secretButton1.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartButton()
    {
        TitleManager.Instance.StartGame();
    }

    public void OptionButton()
    {
        TitleManager.Instance.OpenOption();
    }

    public void Close()
    {
        TitleManager.Instance.CloseOption();
    }
}
