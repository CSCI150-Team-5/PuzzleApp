using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public void playPinball(){
        //set life to 6 when enter game
        SceneManager.LoadScene("pinballGame");
        ballProperty.life = 6;
    }

    public void returnToPinballMain(){
        SceneManager.LoadScene(0);
    }
}
