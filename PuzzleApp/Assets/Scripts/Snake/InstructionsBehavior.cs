using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsBehavior : MonoBehaviour
{
    //When the go button is clicked destroy the instructions
    public void OnClick()
    {
        Destroy(gameObject);
    }
}
