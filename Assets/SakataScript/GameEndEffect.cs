using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameEndEffect : MonoBehaviour
{
    // 設定する値
    [SerializeField] Image imgBack; // 背景画像
    [SerializeField] Image imgEnd;  // 終了画像
    [SerializeField] float fadeDuration = 2.0f;// 透過率が0から1に変化するまでの秒数
    [SerializeField] float scaleDuration = 2.0f;// 拡大率が0から1に変化するまでの秒数
    [SerializeField] private PlayerManager player;// プレイヤーを参照

    // 経過時間
    private float elapsedTime = 0f;

    // 目標拡大スケール
    private Vector3 targetScale = Vector3.one;

    void Start()
    {
        // 透過率を0にする
        // C#のstructのプロパティを直接変更する場合、img.color のコピーを操作してしまうため、
        // 一度 color を取得し、値を変更してから、img.color に戻す必要があります。
        //Color color = imgBack.color;
        //color.a = 0f;
        //imgBack.color = color;

        // 終了画像の目標スケールの設定
        targetScale = imgEnd.rectTransform.localScale;

        // 終了画像の初期スケールの設定
        imgEnd.rectTransform.localScale = Vector3.zero;

        // アタッチしているグループのUIをすべて非表示
        gameObject.SetActive(false);
    }

    void Update()
    {

        //// 画像の透過率を設定した秒数に応じて上昇させる
        //if (!isFading) return;

        //// 経過時間を加算
        //elapsedTime += Time.deltaTime;

        //// 透過率を計算
        //// 経過時間 / 設定秒数 で 0.0f から 1.0f の間の値（t）を得る
        //float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

        //// 透過率を画像に適用
        //Color color = imgBack.color;
        //color.a = alpha;
        //imgBack.color = color;

        //// 完全に不透明になったらUpdate処理を止める
        //if (alpha >= 1.0f)
        //{
        //    enabled = false; // このスクリプトのUpdateを停止
        //}
    }

    //  外部から呼ばれてフェードインを開始するための public メソッド
    public void StartFadeIn()
    {
        // UIを表示
        gameObject.SetActive(true);

        Debug.Log("フェード開始");

        // 処理開始
       // StartCoroutine(FadeInAndScaleUpSequence());
    }

    // 透過率変化から拡大を順番に行うコルーチン
    private IEnumerator FadeInAndScaleUpSequence()
    {
        // ----------------------------------------
        // 1. 透過率を0から1に戻す (フェードイン)
        // ----------------------------------------

        // フェードインのコルーチンを開始し、完了を待機する
        yield return StartCoroutine(FadeImageAlpha(imgBack, 1.0f, fadeDuration));
        Debug.Log("フェードイン完了。拡大処理へ移行します。");

        // ----------------------------------------
        // 2. 別の画像を拡大する
        // ----------------------------------------


        // 拡大処理のコルーチンを開始し、完了を待機する
        yield return StartCoroutine(ScaleRectTransform(imgEnd, targetScale, scaleDuration));
        Debug.Log("全てのアニメーションシーケンスが完了しました。");
    }

    private IEnumerator FadeImageAlpha(Image img, float endAlpha, float duration)
    {
        float startTime = Time.time;
        Color startColor = img.color;
        Color endColor = startColor;
        endColor.a = endAlpha;

        while (Time.time < startTime + duration)
        {
            float timeElapsed = Time.time - startTime;
            float progress = timeElapsed / duration;

            // Color.Lerpで滑らかに値を変化させる
            img.color = Color.Lerp(startColor, endColor, progress);

            yield return null; // 1フレーム待機
        }

        // 確実に目標値に設定
        img.color = endColor;
    }

    // 指定したRectTransformの拡大率を目標値まで変化させるコルーチン
    private IEnumerator ScaleRectTransform(Image img, Vector3 endScale, float duration)
    {
        Vector3 startScale = img.rectTransform.localScale;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float timeElapsed = Time.time - startTime;
            float progress = timeElapsed / duration;

            // Vector3.Lerpで滑らかにスケールを変化させる
            img.rectTransform.localScale = Vector3.Lerp(startScale, endScale, progress);

            yield return null; // 1フレーム待機
        }

        // 確実に目標スケールに設定
        img.rectTransform.localScale = endScale;
    }
}