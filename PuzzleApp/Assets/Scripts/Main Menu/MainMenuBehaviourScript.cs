using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehaviourScript : MonoBehaviour
{
    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }
    public void OnClick(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }
}
