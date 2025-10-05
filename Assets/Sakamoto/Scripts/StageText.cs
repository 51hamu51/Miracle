using UnityEngine;
using TMPro;

public class StageTextScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stageText;
    public GameObject stage1;
    public GameObject stage2;
    public GameObject stage3;
    void Start()
    {
        if (GameManager.Instance.clearStageNum == 0)
        {
            stage2.SetActive(false);
            stage3.SetActive(false);
            stage1.SetActive(true);
        }
        else if (GameManager.Instance.clearStageNum == 1)
        {
            stage1.SetActive(false);
            stage3.SetActive(false);
            stage2.SetActive(true);
        }
        else if (GameManager.Instance.clearStageNum == 2)
        {
            stage1.SetActive(false);
            stage2.SetActive(false);
            stage3.SetActive(true);
        }
    }

    void Update()
    {

        // stageText.text = "Stage:" + (GameManager.Instance?.clearStageNum + 1);

    }
}
