using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public Transform ballPos;
    public GameObject spawmBall;
    public Vector3 MyGameObjectPosition;
    public float posX;
    public float posY;
    

    // Update is called once per frame
    void Update()
    {
        MyGameObjectPosition = GameObject.Find("ball").transform.position;
        posX = MyGameObjectPosition.x;
        posY = MyGameObjectPosition.y;
        if (posY < -10 && (ballProperty.ballCount == 0) && (ballProperty.alive == false) && (ballProperty.life > 0))
        {
            for (int i = 0; i < 1; i++)
            {
                Instantiate(Resources.Load("pinball/ball") as GameObject, ballPos.position, transform.rotation);
                //Instantiate(spawmBall, ballPos.position, transform.rotation);
                ballProperty.ballCount++;
                ballProperty.alive = true;
            }
            

        }
    }
}
