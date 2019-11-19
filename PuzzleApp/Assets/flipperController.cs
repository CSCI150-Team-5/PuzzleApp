using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flipperController : MonoBehaviour
{

    public bool flipper;
    // Update is called once per frame
    void Update()
    {
        if ((flipper && Input.GetKey(KeyCode.RightControl)) || (!flipper && Input.GetKey(KeyCode.LeftControl)))
        {
            GetComponent<HingeJoint2D>().useMotor = true;
        }
        else
        {
            GetComponent<HingeJoint2D>().useMotor = false;
        }
    }
}
