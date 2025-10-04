using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel;
    void Start()
    {
        optionPanel.SetActive(false);
    }

    void Update()
    {

    }

    public void OptionOnOff()
    {
        optionPanel.SetActive(!optionPanel.activeSelf);
        float tmp = Time.timeScale;
        tmp *= -1;
        tmp += 1;
        Time.timeScale = tmp;
    }
}
