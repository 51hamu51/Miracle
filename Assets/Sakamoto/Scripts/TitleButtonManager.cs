using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class TitleButtonManager : MonoBehaviour
{
    public GameObject secretButton1;
    public GameObject secretButton2;
    public AudioSource dicieded;
    public AudioSource antiDicieded;
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
        dicieded.Play();
    }

    public void OptionButton()
    {
        TitleManager.Instance.OpenOption();
        dicieded.Play();
    }

    public void Close()
    {
        TitleManager.Instance.CloseOption();
        dicieded.Play();
    }

    public void SecletLock()
    {
        antiDicieded.Play();
    }

    public void SecletAnLock()
    {
        dicieded.Play();
    }
}
