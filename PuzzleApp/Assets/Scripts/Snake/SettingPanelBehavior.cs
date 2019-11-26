using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanelBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector3(10000,10000,0);
    }

    public void CallPanel()
    {
        transform.localPosition = Vector3.zero;
    }

    public void DismissPanel()
    {
        transform.localPosition = new Vector3(10000, 10000, 0);
    }
}
