 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionSquareBehavior : MonoBehaviour
{
    //Establish colors for all selection buttons
    static Color INACTIVE = new Color(1, 1, 1, 1);
    static Color ACTIVE = new Color(0, 0, 0, 1);
    

    [SerializeField]
    private GameObject master; //Reference to the main game object
    [SerializeField]
    private Text text; //Reference to the text element of this button

    private int myNum; //Store the number this object contains

    //Set this objects number to the number given
    public void SetMyNum(int num)
    {
        //Debug.Log("Setting Num " + num);
        myNum = num;
        text.text = num.ToString(); 
    }

    //Handle clicks of this button
    public void OnClick()
    {
        //Pass the click on to the main game object
        master.GetComponent<Controller>().SelectButtonClicked(myNum);
    }

    //Set the color of this object
    public void SetActive(bool isActive)
    {
        text.color = isActive ? ACTIVE : INACTIVE;
    }
}
