using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreenController : MonoBehaviour
{
    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        
    }

    public void clickedButton(int i)
    {
        MainMenuController.instance.ChangeScene(i);
    }
}
