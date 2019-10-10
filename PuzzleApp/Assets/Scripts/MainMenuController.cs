using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void OpenMinesweeper()
    {
        SceneManager.LoadScene(2);
    }
    public void OpenSnake()
    {
        SceneManager.LoadScene(3);
    }
}
