using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballProperty : MonoBehaviour
{
    //initialize ball stats ball bount, life, and alive 
    public static int ballCount = 1;
    public static int life = 5;
    public static bool alive = true;

    private void Start()
    {
        
        if (ballCount == 0)
        {
            alive = false;
        }
    }


    void Update()
    {

        if (ballCount == 0)
        {
            alive = false;
        }


    }
}
