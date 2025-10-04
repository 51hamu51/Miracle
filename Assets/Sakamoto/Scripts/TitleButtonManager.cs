using UnityEngine;

public class TitleButtonManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
