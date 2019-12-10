using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanelBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Move this panel offscreen
        transform.localPosition = new Vector3(10000,10000,0);
    }

    //Handle when settings button is clicked
    public void CallPanel()
    {
        //Move this panel into view
        transform.localPosition = Vector3.zero;
    }

    //Handle X button being clicked
    public void DismissPanel()
    {
        //Move this panel offscreen
        transform.localPosition = new Vector3(10000, 10000, 0);
    }
}
