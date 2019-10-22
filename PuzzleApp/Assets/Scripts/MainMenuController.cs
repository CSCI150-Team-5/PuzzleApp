using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject myMasterMenu; //Make a place for the Master Menu Prefab - Place the prefab in here in the Unity UI
    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Instantiate(myMasterMenu, new Vector3(0, 0, 0), Quaternion.identity); //Create the menu and place it in its default position
        MasterMenuBehavior.OnClickedSettings += ClickedSettings; //Subscribe your function to be notified when setting button is clicked
        MasterMenuBehavior.OnClickedBack += ClickedBack; //Subscribe your function to be notified when back button is clicked
    }
    public void OpenMinesweeper()
    {
        SceneManager.LoadScene(2);
    }
    public void OpenSnake()
    {
        SceneManager.LoadScene(3);
    }

    void ClickedSettings() //Handle Settings Button Clicks Here
    {
        Debug.Log("Clicked Settings Button");
    }

    void ClickedBack() //Handle Back Button Clicks Here
    {
        Debug.Log("Clicked Back Button");
    }
}
