using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

using UnityEngine.Audio;

using UnityEngine.UI;


public class PlayerManager : MonoBehaviour
{
    public int playerHP;
    [SerializeField] private int playerMaxHP;
    [SerializeField] private int playerFirstHP;

    public GameObject HPGauge;

    /// <summary>
    /// 継続ダメージを受ける間隔
    /// </summary>
    [SerializeField] private float damageInterval;
    private float timer;

    /// <summary>
    /// 敵を倒したときの回復量
    /// </summary>
    [SerializeField] private int recoverAmount;

    /// <summary>
    /// 敵を倒すのにかかる時間
    /// </summary>
    [SerializeField] private float drainTime;
    private float drainTimer;
    private bool IsDrain;

    public ResultCanvasManager resultCanvasManager;

    [SerializeField] private bool IsDead;
    [SerializeField] private bool IsClear;

    public bool CanMove => !IsDead && !IsRotating && !IsRotating2 && !IsEating && !IsMoving && !IsClear;

    private Vector3 initialScale; // 初期サイズを保持
    private BossController bossController;
    private EnemyController enemyController;

    public bool IsRotating;
    public bool IsRotating2;
    public bool IsEating;
    public bool IsMoving;

    public bool IsFading;

    public bool IsRed;


    public Filter filter;

    public Transform startPosition;

    public Transform cameraTransform;

    [SerializeField] private float eatMoveSpeed;

    /// <summary>
    /// ボスを倒したときの回復ボーナス(倍率)
    /// </summary>
    [SerializeField] private float bossBonus;


    [SerializeField] private GameEndEffect gameEnd;

    [SerializeField] private AudioSource hitSE;
    [SerializeField] private AudioSource eatSE;
    [SerializeField] private AudioSource deadSE;
    [SerializeField] private AudioSource gameEndSE;
    [SerializeField] private AudioSource lastEatSE;

    // 目標の角度
    private Quaternion targetRotation;
    // 回転速度（度/秒）
    public float rotateSpeed = 90f;

    void Start()
    {
        playerHP = playerFirstHP;
        timer = 0f;
        initialScale = transform.localScale;
        UpdatePlayerScale();
        IsDead = false;
        IsClear = false;
        IsDrain = false;
        IsRotating = false;
        IsRotating2 = false;
        IsEating = false;
        IsMoving = false;

        IsFading = false;

        // ↓これは使える、分からん
        // gameEnd.StartFadeIn();

        IsRed = false;

        HPGaugeSet();

    }

    void Update()
    {
        HPGaugeSet();

        if (IsRotating)


        { // 目標方向を計算
            if (!IsRed)
            {
                filter.RedScreen();
                IsRed = true;
            }

            Vector3 direction = (startPosition.position - transform.position).normalized;

            //  目標方向に向かって回転
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.LookRotation(direction),
                rotateSpeed * Time.unscaledDeltaTime
            );

            // 回転終了判定
            if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(direction)) < 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(direction);
                IsRotating = false;
                IsMoving = true; // 回転終わったら移動開始
                Debug.Log("FinishRotate");
            }
        }


        if (IsMoving && startPosition != null)
        {
            //Debug.Log("Move");
            Vector3 direction = (startPosition.position - transform.position).normalized;
            // 目標方向に進む
            transform.position += direction * eatMoveSpeed * Time.unscaledDeltaTime;

            //目標に到達したら停止
            if (Vector3.Distance(transform.position, startPosition.position) < 0.1f)
            {
                IsMoving = false;
                IsRotating2 = true;
            }
        }








        if (IsRotating2)
        { // 目標方向を計算
            Vector3 direction = (cameraTransform.position - transform.position).normalized;

            //  目標方向に向かって回転
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.LookRotation(direction),
                rotateSpeed * Time.unscaledDeltaTime
            );

            // 回転終了判定
            if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(direction)) < 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(direction);
                IsRotating2 = false;
                IsEating = true; // 回転終わったら移動開始
                if (lastEatSE != null)
                {
                    lastEatSE.Play();
                }
            }
        }


        if (IsEating && cameraTransform != null)
        {
            Vector3 direction = (cameraTransform.position - transform.position).normalized;
            // 目標方向に進む
            transform.position += direction * eatMoveSpeed * Time.unscaledDeltaTime;

            //目標に到達したら停止
            if (Vector3.Distance(transform.position, cameraTransform.position) < 0.1f)
            {
                IsEating = false;
                StartCoroutine(CallAfterDelay());
            }
        }


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
            if (deadSE != null)
            {
                deadSE.Play();
            }
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
        scaleRate = Mathf.Clamp(scaleRate, 0f, 1f);
        transform.localScale = initialScale * scaleRate * 0.5f;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IsDrain = true;
            if (hitSE != null)
            {
                hitSE.Play();
            }
            Debug.Log("hitSE再生");
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
        if (bossController != null)
        {
            playerHP = (int)Mathf.Min(playerHP + bossBonus * amount, playerMaxHP);
            bossController.BossDead();
        }
        if (enemyController != null)
        {
            playerHP = Mathf.Min(playerHP + amount, playerMaxHP);
            enemyController.EnemyDead();
        }
        if (eatSE != null)
        {
            eatSE.Play();
            Debug.Log("eatSE再生");
        }
        Debug.Log("回復！HP: " + playerHP);
        IsDrain = false;
        drainTimer = 0;
        UpdatePlayerScale();
    }

    void Attacked(int amount)
    {
        playerHP = Mathf.Max(playerHP - amount, 0);
        Debug.Log("ダメージ！HP: " + playerHP);
        UpdatePlayerScale();
    }

    public void Eat()
    {
        IsRotating = true;
        if (gameEndSE != null)
        {
            gameEndSE.Play();
        }

        TitleManager.Instance.StopGameBGM();
        // ↓これは使えない、分からん
        gameEnd.gameObject.SetActive(true);
        gameEnd.StartFadeIn();
    }

    private IEnumerator CallAfterDelay()
    {
        yield return new WaitForSecondsRealtime(3f);
        BackToTitle();
    }

    private void BackToTitle()
    {
        GameManager.Instance.IsGameClear = true;
        GameManager.Instance.GameReset();
        TitleManager.Instance.ChangeTitleBGM();
        SceneManager.LoadScene("TitleScene");
    }

    public void HPGaugeSet()
    {
        //ImageというコンポーネントのfillAmountを取得して操作する
        HPGauge.GetComponent<Image>().fillAmount = (float)playerHP / playerMaxHP;
    }
}
