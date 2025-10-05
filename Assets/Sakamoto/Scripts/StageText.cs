using UnityEngine;
using TMPro;

public class StageTextScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stageText;
    void Start()
    {

    }

    void Update()
    {
        
            stageText.text = "Stage:" + (GameManager.Instance?.clearStageNum + 1);
        
    }
}
