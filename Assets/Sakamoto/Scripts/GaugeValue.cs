using UnityEngine;
using UnityEngine.UI;

public class GaugeValue : MonoBehaviour
{
    public GameObject VolumeGauge;
    public Slider volumeSlider;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        VolumeGaugeSet();
    }

    public void VolumeGaugeSet()
    {
        //ImageというコンポーネントのfillAmountを取得して操作する
        VolumeGauge.GetComponent<Image>().fillAmount = volumeSlider.value;
    }
}
