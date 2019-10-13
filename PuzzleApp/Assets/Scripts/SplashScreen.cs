using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Invoke("LoadMainMenu", 0.5f);
    }

    public void LoadMainMenu()
    {
        MainMenuController.instance.ChangeScene(1);
    }
}

