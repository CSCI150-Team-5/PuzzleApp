using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public void playPinball(){
        SceneManager.LoadScene("pinballGame");
    }

    public void returnToPinballMain(){
        SceneManager.LoadScene("pbMainMenu");
    }
}
