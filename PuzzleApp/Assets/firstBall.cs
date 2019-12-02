using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstBall : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < -10)
        {
            
            ballProperty.ballCount--;

        }
    }
}
