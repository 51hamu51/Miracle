using UnityEngine;

public class PanelManger : MonoBehaviour
{
    [SerializeField] GameObject asobikataPanel;
    [SerializeField] GameObject secretPanel;
    void Start()
    {
        asobikataPanel.SetActive(false);
        secretPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AsobikataOnOff()
    {
        asobikataPanel.SetActive(!asobikataPanel.activeSelf);
    }
    public void SecretOnOff()
    {
        secretPanel.SetActive(!secretPanel.activeSelf);
    }
}
