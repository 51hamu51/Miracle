using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Filter : MonoBehaviour
{
    public Volume volume;
    private Bloom bloom;
    void Start()
    {
        if (volume.profile.TryGet(out bloom))
        {
            bloom.active = true;
            string sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == "Stage1")
            {
                bloom.threshold.value = 0f;
                bloom.intensity.value = 0.1f;
                bloom.scatter.value = 0f;
                bloom.dirtIntensity.value = 15f;
            }
            
            else if (sceneName == "Stage2")
            {
                
            }
        }   

    }
}
