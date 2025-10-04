//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Filter : MonoBehaviour
{
    public Volume volume;
    private Bloom bloom;
    private FilmGrain filmGrain;
    private Vignette vignette;
    private ChromaticAberration chromaticAberration;
    private ColorAdjustments colorAdjustments;

    void Awake()
    {
        // ✅ Volumeを確実に取得する
        if (volume == null)
        {
            volume = GetComponent<Volume>();
        }

        if (volume == null)
        {
            volume = FindObjectOfType<Volume>();
            Debug.Log("FindObjectOfTypeでVolume取得: " + volume);
        }

        if (volume == null)
        {
            Debug.LogError("Volumeが見つかりません！Filterの参照が切れています");
            return;
        }

        // ✅ VolumeProfileからエフェクト取得
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out filmGrain);
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out colorAdjustments);

        Debug.Log($"[Awake完了] Scene={gameObject.scene.name}, Volume={volume}, " +
                  $"Bloom={bloom}, Vignette={vignette}, Grain={filmGrain}, CA={chromaticAberration}, ColorAdj={colorAdjustments}");
    }

    void Start()
    {
        // ✅ Awakeで確保できなかった場合は弾く
        if (bloom == null || filmGrain == null || vignette == null || chromaticAberration == null || colorAdjustments == null)
        {
            Debug.LogError("Start時点でVolumeプロパティが未設定です。Awakeが正常に動いていない可能性");
            return;
        }

        bloom.active = true;
        filmGrain.active = true;
        vignette.active = true;
        chromaticAberration.active = true;
        colorAdjustments.active = true;

        int stage = GameManager.Instance.clearStageNum;

        if (stage == 0)
        {
            bloom.threshold.value = 0f;
            bloom.intensity.value = 0.1f;
            bloom.scatter.value = 0f;
            bloom.dirtIntensity.value = 15f;
        }
        else if (stage == 1)
        {
            bloom.threshold.value = 0f;
            bloom.intensity.value = 0.1f;
            bloom.scatter.value = 0f;
            bloom.dirtIntensity.value = 15f;
            filmGrain.type.value = FilmGrainLookup.Thin1;
            filmGrain.intensity.value = 0.5f;
            filmGrain.response.value = Mathf.PingPong(Time.time * 0.5f, 1f);
            chromaticAberration.intensity.value = 0.3f;
        }
        else if (stage == 2)
        {
            bloom.threshold.value = 0f;
            bloom.intensity.value = 0.1f;
            bloom.scatter.value = 0f;
            bloom.dirtIntensity.value = 15f;
            filmGrain.type.value = FilmGrainLookup.Medium3;
            filmGrain.intensity.value = 1f;
            filmGrain.response.value = Mathf.PingPong(Time.time * 0.5f, 1f);
            vignette.color.value = Color.red;
            vignette.intensity.value = 0.3f;
            vignette.smoothness.value = 0.1f;
            chromaticAberration.intensity.value = 1f;
        }
    }

    public void RedScreen()
    {
        Debug.Log($"RedScreen呼び出し: Scene={gameObject.scene.name}, Volume={volume}");

        if (volume == null)
        {
            Debug.LogError("RedScreen時点でVolumeがnull");
            return;
        }

        // ✅ 念のため再取得（シーン跨ぎ対応）
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out filmGrain);
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out colorAdjustments);

        Debug.Log($"RedScreen直前の状態 => Bloom={bloom}, Grain={filmGrain}, Vignette={vignette}, CA={chromaticAberration}, ColorAdj={colorAdjustments}");

        if (bloom == null || filmGrain == null || vignette == null || chromaticAberration == null || colorAdjustments == null)
        {
            Debug.LogError("RedScreen実行不可：1つ以上のエフェクトがnull");
            return;
        }

        bloom.threshold.value = 0f;
        bloom.intensity.value = 0.1f;
        bloom.scatter.value = 1f;
        bloom.dirtIntensity.value = 100f;
        filmGrain.type.value = FilmGrainLookup.Medium3;
        filmGrain.intensity.value = 1f;
        filmGrain.response.value = Mathf.PingPong(Time.time * 0.5f, 1f);
        vignette.color.value = Color.red;
        vignette.intensity.value = 0.4f;
        vignette.smoothness.value = 1f;
        colorAdjustments.contrast.value = 100f;
        volume.weight = 1f;
        Camera mainCam = Camera.main;
        var camData = mainCam.GetComponent<UniversalAdditionalCameraData>();
        Debug.Log($"Camera PostProcessing: {camData != null}, volumeLayer: {mainCam.cullingMask}");
    }
}
