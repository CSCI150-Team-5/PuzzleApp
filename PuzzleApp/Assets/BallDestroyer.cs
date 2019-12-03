using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDestroyer : MonoBehaviour
{
    
  
    // Update is called once per frame
    void Update()
    {
        
        if (this.transform.position.y < -10)
        {
            Destroy(this.gameObject);
            ballProperty.ballCount--;
            ballProperty.life--;
            ballProperty.alive = false;
        }

        
    }
}
