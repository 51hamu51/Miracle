using UnityEngine;

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
    }

    public void Dead()
    {
        deadPanel.SetActive(true);
    }
}
