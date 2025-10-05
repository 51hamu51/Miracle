using UnityEngine;

public class Players : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    void Start()
    {
        if (GameManager.Instance.clearStageNum == 0)
        {
            player2.SetActive(false);
            player3.SetActive(false);
            player1.SetActive(true);
        }
        else if (GameManager.Instance.clearStageNum == 1)
        {
            player2.SetActive(true);
            player3.SetActive(false);
            player1.SetActive(false);
        }
        else if (GameManager.Instance.clearStageNum == 2)
        {
            player1.SetActive(false);
            player2.SetActive(false);
            player3.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
