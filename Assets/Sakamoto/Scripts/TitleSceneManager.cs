using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    public Image logoImage;
    public Image titleBackImage;

    public GameObject optionPanel;
    public GameObject titleUis;
    void Start()
    {

        TitleManager.Instance.GetSceneManager();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
