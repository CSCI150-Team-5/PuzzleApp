using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSquareBehavior : MonoBehaviour
{
    //Establish colors for all gameboard buttons
    static Color UNSELECTABLE_COLOR = new Color(0, 0, 0, 1);
    static Color ERROR_COLOR = new Color(1, 0, 0, 1);
    static Color SELECTABLE_COLOR = new Color(1, 1, 1, 1);
    static Color INVISIBLE_COLOR = new Color(0, 0, 0, 0);

    [SerializeField]
    private Text[] texts = new Text[10]; //Reference to all ten text items on this object

    [SerializeField]
    private GameObject master; //Reference to main game object
    
    private Vector2Int cell; //Value of the cell this object represents
    //private bool isLocked = false; //Is clickable or not

    //Handle clicks on this object
    public void OnClick()
    { 
        //Pass the click on to the main game object
        master.GetComponent<Controller>().GameButtonClicked(cell);
    }

    //Show given numbers on this object
    public void SetDisplay(List<int> nums, bool highlight, bool isClickable = true)
    {
        //Debug.Log("SETLOCKED: " + (setLocked ? "true" : "false"));
        //isLocked = !isClickable;
        //Clearing all value displays
        foreach (Text t in texts)
        {
            t.color = INVISIBLE_COLOR;
        }

        //If only one value is sent...
        if(nums.Count == 1)
        {
            //Use isClickable and the sign of the value to determine color
            Color thisColor = UNSELECTABLE_COLOR; //Unselecable numbers can one be single values
            if (isClickable) thisColor = SELECTABLE_COLOR;
            int thisNumber = nums[0];
            if(thisNumber < 0)
            {
                if(highlight) thisColor = ERROR_COLOR;
                thisNumber *= -1; //Now reset the value back to positive
            }

            //Set the main text to the number and color
            texts[0].text = thisNumber.ToString();
            texts[0].color = thisColor;
        }
        //If more than one value is sent....
        else if(nums.Count > 1)
        {
            foreach(int num in nums)//itterate through each value
            {
                //use the sign of the number to determine color
                int thisNum = num;
                Color thisColor = SELECTABLE_COLOR;
                if(num < 0)
                {
                    if(highlight) thisColor = ERROR_COLOR;
                    thisNum *= -1;
                }
                //Set the given number tiny text to the color required
                texts[thisNum].color = thisColor;
            }
        }
    }

    //Set this objects location to the given cell
    public void SetLoc(Vector2Int l)
    {
        cell = l;
    }
}
