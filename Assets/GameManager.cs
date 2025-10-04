using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private ResultCanvasManager resultCanvasManager;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    public void StageClear()
    {
        resultCanvasManager.Clear();
    }

    public void Dead()
    {
        resultCanvasManager.Dead();
    }
}
