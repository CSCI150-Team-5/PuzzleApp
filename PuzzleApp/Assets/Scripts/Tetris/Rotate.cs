using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private GameObject temp;

    public void ButtonPress()
    {
        if (temp != null)
        {
            temp.BroadcastMessage("Rotation");
        }
    }

    public void SetTemp(GameObject tet)
    {
        temp = tet;
    }
}
