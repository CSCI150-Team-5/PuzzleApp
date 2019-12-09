using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameStats : MonoBehaviour   

{
    public GameObject gameOver;
    public Text scoreText;
    public Text scoreText1;
    public Text lifeText;
    public static int score;

    // Start is called before the first frame update
    void Start()
    {
        generateBumpers();
    }

    // Update is called once per frame
    void Update()
    {
        //writing to screen from variables
        scoreText.text = score.ToString();
        scoreText1.text = score.ToString();
        lifeText.text = ballProperty.life.ToString();
        //game over status, activates gameover board
        if(ballProperty.life == 0){
            gameOver.SetActive(true);
        }
        else{
            gameOver.SetActive(false);
        }


        
    }
    //generates bumper on gameboard.
    //
    void generateBumpers(){

        //spawn bumpers on map on game start
        GameObject bumper1 = (GameObject)Instantiate(Resources.Load("pinball/rectLeft"));
        GameObject bumper2 = (GameObject)Instantiate(Resources.Load("pinball/rectRight"));
        GameObject bumper3 = (GameObject)Instantiate(Resources.Load("pinball/rectMid"));
        GameObject bumper4 = (GameObject)Instantiate(Resources.Load("pinball/ballMulti"));
        GameObject bumper5 = (GameObject)Instantiate(Resources.Load("pinball/triangleLeft"));
        GameObject bumper6 = (GameObject)Instantiate(Resources.Load("pinball/triangleRight"));
        GameObject bumper7 = (GameObject)Instantiate(Resources.Load("pinball/cLeftBumper"));
        GameObject bumper8 = (GameObject)Instantiate(Resources.Load("pinball/cMidBumper"));
        GameObject bumper9 = (GameObject)Instantiate(Resources.Load("pinball/cRightBumper"));
        

    }
}
