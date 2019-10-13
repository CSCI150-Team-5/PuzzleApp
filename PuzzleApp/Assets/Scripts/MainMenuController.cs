using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class MainMenuController : MonoBehaviour
{
    public delegate void MenuStateChanged(bool b);
    public static event MenuStateChanged onChange;

    public static MainMenuController instance;
    public GameObject topPanel;
    public GameObject mainPanel;
    public GameObject snakePanel;
    
    private GameObject curPanel;

    private float masterMusicVol;
    private float masterSFXVol;

    private float snakeMusicVol;
    private float snakeSFXVol;
    private float snakeBaseSpeed;
    private bool snakeWallCol;
    
    private int curScene;

    private enum games { SPLASH, HOME, MINE, SNAKE}

    private void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
        Screen.orientation = ScreenOrientation.Portrait;
        topPanel.SetActive(false);
        mainPanel.SetActive(false);
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
            curPanel.SetActive(true);
        }
        if(onChange != null) onChange(curPanel.activeSelf);
    }

    public bool isMenuActive()
    {
        return curPanel.activeSelf;
    }

    public float getMasterMusicVol()
    {
        return masterMusicVol;
    }

    public void setMasterMusicVol(float v)
    {
        masterMusicVol = v;
    }

    public float getMasterSFXVol()
    {
        return masterSFXVol;
    }

    public void setMasterSFXVol(float v)
    {
        masterSFXVol = v;
    }

    public float getSnakeMusicVol()
    {
        return snakeMusicVol;
    }

    public void setsnakeMusicVol(float v)
    {
        snakeMusicVol = v;
    }

    public float getSnakeSFXVol()
    {
        return snakeSFXVol;
    }

    public void setSnakeSFXVol(float v)
    {
        snakeSFXVol = v;
    }

    public float getSnakeBaseSpeed()
    {
        return snakeBaseSpeed;
    }

    public void setSnakeBaseSpeed(float v)
    {
        snakeBaseSpeed = v;
    }

    public bool getSnakeWallCol()
    {
        return snakeWallCol;
    }

    public void setSnakeWallCol(bool b)
    {
        snakeWallCol = b;
    }
}
