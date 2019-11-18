using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSquareBehavior : MonoBehaviour
{
    Color RED = new Color(1, 0, 0, 1);
    Color WHITE = new Color(1, 1, 1, 1);
    Color ALPHA = new Color(0, 0, 0, 0);

    [SerializeField]
    private Text[] texts = new Text[10];
    private bool[] selected = new bool[9];

    private Vector2Int loc;
    private bool isClickable = true;

    [SerializeField]
    private GameObject master;

    void Start()
    {
        /*
        if(selected[1])
        { 
            Debug.Log("Selected is true");
        }
        else
        {
            Debug.Log("Selected is false");
        }
        */
    }

    public void OnClick()
    {
        if (isClickable)
        {
            master.GetComponent<GameController>().GameButtonClicked(loc);
        }
    }

    //Takes a list of numbers and sets the display acoringly
    //Negative numbers are converted to red text to indicate a conflict in placement
    //SetClickable to false to prevent the player from being able to click this square.
    public void SetDisplay(List<int> nums, bool setClickable = true)
    {
        isClickable = setClickable;
        foreach (Text t in texts)
        {
            t.color = ALPHA;
        }
        foreach(int i in nums)
        {
            Debug.Log(loc.x+","+loc.y+" Recieved: " + i);
        }
        if(nums.Count == 1)
        {
            Color thisColor = WHITE;
            int thisNumber = nums[0];
            if(thisNumber < 0)
            {
                thisColor = RED;
                thisNumber *= -1;
            }
            texts[0].text = thisNumber.ToString();
            texts[0].color = thisColor;
        }
        else if(nums.Count == 0) Debug.Log("TRIGGER\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\");
        else if(nums.Count > 1)
        {
            foreach(int num in nums)
            {
                int thisNum = num;
                Color thisColor = WHITE;
                if(num < 0)
                {
                    thisColor = RED;
                    thisNum *= -1;
                }
                texts[thisNum].color = thisColor;
            }
        }
    }

    public void SetLoc(Vector2Int l)
    {
        loc = l;
    }
}
