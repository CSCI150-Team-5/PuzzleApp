using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightButton : MonoBehaviour
{
    private GameObject temp;

    public void ButtonPress()
    {
        if (temp != null)
        {
            temp.BroadcastMessage("MoveRight");
        }
    }

    public void SetTemp(GameObject tet)
    {
        temp = tet;
    }
}
