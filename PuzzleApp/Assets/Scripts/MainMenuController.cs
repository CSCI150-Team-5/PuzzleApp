using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
<<<<<<< Updated upstream
=======
        topPanel.SetActive(false);
        mainPanel.SetActive(false);
        exitPanel.SetActive(false);
        snakePanel.SetActive(false);
        masterMusicVol = 1f;
        masterSFXVol = 1f;
        snakeMusicVol = 1f;
        snakeSFXVol = 1f;
        snakeBaseSpeed = 0.4f;
        snakeWallCol = true;
        curScene = 0;
    }

    public void ChangeScene(int s)
    {
        SceneManager.LoadScene(s);
        topPanel.SetActive(true);
        curScene = s;
        if(s == (int)games.HOME)
        {
            curPanel = mainPanel;
        }
        else if(s == (int)games.SNAKE)
        {
            curPanel = snakePanel;
        }
    }

    public void onMenuPress()
    {
        if (curPanel.activeSelf)
        {
            curPanel.SetActive(false);
        }
        else
        {
            exitPanel.SetActive(false);
            curPanel.SetActive(true);
        }
        if(onChange != null) onChange(curPanel.activeSelf);
    }

    public void onExitPress()
    {
        if (onChange != null) onChange(curPanel.activeSelf);
        curPanel.SetActive(false);
        exitPanel.SetActive(true);
>>>>>>> Stashed changes
    }
    public void OpenMinesweeper()
    {
        SceneManager.LoadScene(2);
    }
    public void OpenSnake()
    {
        SceneManager.LoadScene(3);
    }
}
