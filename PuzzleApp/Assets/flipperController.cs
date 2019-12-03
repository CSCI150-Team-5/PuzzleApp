using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class flipperController : MonoBehaviour
{

    public bool flipper;
    // Update is called once per frame
   /* void Update()
    {
        Input.GetMouseButtonDown(0);

        if ((flipper && Input.GetKey(KeyCode.RightControl)) || (!flipper && Input.GetKey(KeyCode.LeftControl)))
        {
            GetComponent<HingeJoint2D>().useMotor = true;
        }
        else
        {
            GetComponent<HingeJoint2D>().useMotor = false;
        }
    }*/

    void Update () {
         if(Input.GetMouseButtonDown(0))
        {
            if(Input.mousePosition.x > Screen.width * 0.5f && flipper)
            {
                //Right side.
                GetComponent<HingeJoint2D>().useMotor = true;
                Debug.Log ("right");
            }
            if(Input.mousePosition.x < Screen.width * 0.5f && !flipper)
            {
                //Right side.
                GetComponent<HingeJoint2D>().useMotor = true;
                Debug.Log ("right");
            }
            
        }
        else
            {
                //Left side.
                GetComponent<HingeJoint2D>().useMotor = false;
                Debug.Log ("left");
            }

        
    }
}
