using UnityEngine;

public class GameButtonManage : MonoBehaviour
{
    public GameObject optionPanel;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Close()
    {
        optionPanel.SetActive(false);
    }
}
