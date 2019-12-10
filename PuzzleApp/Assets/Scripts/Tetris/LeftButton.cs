using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftButton : MonoBehaviour
{
    private GameObject temp;

    public void ButtonPress()
    {
        if (temp != null)
        {
            temp.BroadcastMessage("MoveLeft");
        }
    }

    public void SetTemp(GameObject tet)
    {
        temp = tet;
    }
}
