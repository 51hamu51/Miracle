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

    void Start()
    {
        if (volume.profile.TryGet(out bloom))
            if (volume.profile.TryGet(out filmGrain))
                if (volume.profile.TryGet(out vignette))
                    if (volume.profile.TryGet(out chromaticAberration))
                        if (volume.profile.TryGet(out colorAdjustments))

                        {
                            bloom.active = true;
                            filmGrain.active = true;
                            vignette.active = true;
                            chromaticAberration.active = true;
                            colorAdjustments.active = true;
                            string sceneName = SceneManager.GetActiveScene().name;


                            if (GameManager.Instance.clearStageNum == 0)
                            {
                                bloom.threshold.value = 0f;
                                bloom.intensity.value = 0.1f;
                                bloom.scatter.value = 0f;
                                bloom.dirtIntensity.value = 15f;
                            }

                            else if (GameManager.Instance.clearStageNum == 1)
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

                            else if (GameManager.Instance.clearStageNum == 2)
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

                            else if (GameManager.Instance.clearStageNum == 3)
                            {
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

                            }
                        }
    }

    public void RedScreen()
    {
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
    }
}