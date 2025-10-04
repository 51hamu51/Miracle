using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int playerHP;
    [SerializeField] private int playerMaxHP;
    [SerializeField] private int playerFirstHP;
    [SerializeField] private int damageInterval;
    private float timer;
    [SerializeField] private int recoverAmount;

    [SerializeField] private float drainTime;
    private float drainTimer;
    private bool IsDrain;

    public ResultCanvasManager resultCanvasManager;

    private bool IsDead;
    private bool IsClear;

    private Vector3 initialScale; // 初期サイズを保持
    public BossController bossController;
    public EnemyController enemyController;

    void Start()
    {
        playerHP = playerFirstHP;
        timer = 0f;
        initialScale = transform.localScale;
        UpdatePlayerScale();
        IsDead = false;
        IsClear = false;
        IsDrain = false;
    }

    void Update()
    {
        if (IsDead || IsClear)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= damageInterval)
        {
            playerHP -= 1;
            timer = 0f;
            Debug.Log("HP: " + playerHP);

            UpdatePlayerScale();
        }

        if (playerHP <= 0)
        {
            IsDead = true;
            resultCanvasManager.Dead();
        }

        if (playerHP >= playerMaxHP)
        {
            IsClear = true;
            resultCanvasManager.Clear();
        }

        if (IsDrain)
        {
            drainTimer += Time.deltaTime;
            if (drainTimer >= drainTime)
            {
                drainTimer = 0;
                RecoverHP(recoverAmount);
            }
        }
    }

    void UpdatePlayerScale()
    {
        float scaleRate = (float)playerHP / playerMaxHP;
        transform.localScale = initialScale * scaleRate;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IsDrain = true;

            bossController = collision.gameObject.GetComponent<BossController>();
            enemyController = collision.gameObject.GetComponent<EnemyController>();
            // スクリプトが存在して、かつIsAttackがtrueなら
            if (bossController != null && bossController._isAttack)
            {
                Attacked(10);
            }
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IsDrain = false;
            drainTimer = 0;
        }
    }

    void RecoverHP(int amount)
    {
        playerHP = Mathf.Min(playerHP + amount, playerMaxHP);
        Debug.Log("回復！HP: " + playerHP);
        IsDrain = false;
        drainTimer = 0;
        UpdatePlayerScale();
        if (bossController != null)
        {
            bossController.BossDead();
        }
        if (enemyController != null)
        {
            enemyController.EnemyDead();
        }
    }

    void Attacked(int amount)
    {
        playerHP = Mathf.Max(playerHP - amount, 0);
        Debug.Log("ダメージ！HP: " + playerHP);
        UpdatePlayerScale();
    }
}
