using UnityEngine;
using TMPro;

public class StageTextScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stageText;
    public GameObject Room1;
    public GameObject Room2;
    public GameObject Room3;
    void Start()
    {
        if (GameManager.Instance.clearStageNum == 0)
        {
            Room2.SetActive(false);
            Room3.SetActive(false);
            Room1.SetActive(true);
        }
        else if (GameManager.Instance.clearStageNum == 1)
        {
            Room1.SetActive(false);
            Room3.SetActive(false);
            Room2.SetActive(true);
        }
        else if (GameManager.Instance.clearStageNum == 2)
        {
            Room1.SetActive(false);
            Room2.SetActive(false);
            Room3.SetActive(true);
        }
    }

    void Update()
    {

        //  stageText.text = "Stage:" + (GameManager.Instance?.clearStageNum + 1);

    }
}
