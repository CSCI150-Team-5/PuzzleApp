using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterMenuBehavior : MonoBehaviour
{
    public delegate void ClickedSettings();
    public delegate void ClickedBack();

    public static event ClickedSettings OnClickedSettings;
    public static event ClickedBack OnClickedBack;

    [SerializeField]
    private Button buttonSettings;
    [SerializeField]
    private Button buttonBack;
    [SerializeField]
    private Text text;

    void Awake()
    {
        buttonSettings.onClick.AddListener(RunSettings);
        buttonBack.onClick.AddListener(RunBack);
    }

    void RunSettings()
    {
        if (OnClickedSettings != null) OnClickedSettings();
    }

    void RunBack()
    {
        if (OnClickedBack != null) OnClickedBack();
    }
}
