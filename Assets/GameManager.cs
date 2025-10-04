using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// ステージをクリアした回数
    /// </summary>
    public int clearStageNum;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        GameReset();
    }

    public void StageClear()
    {
        clearStageNum++;
    }

    public void GameReset()
    {
        clearStageNum = 0;
    }
}
