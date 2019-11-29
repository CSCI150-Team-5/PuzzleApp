using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSquareBehavior : MonoBehaviour
{
    static Color BLACK = new Color(0, 0, 0, 1);
    static Color RED = new Color(1, 0, 0, 1);
    static Color WHITE = new Color(1, 1, 1, 1);
    static Color ALPHA = new Color(0, 0, 0, 0);

    [SerializeField]
    private Text[] texts = new Text[10];
    [SerializeField]
    private GameObject master;
    private Vector2Int cell;
    private bool isLocked = false;

    public void OnClick()
    {
        //if(!isLocked)
        //{
            master.GetComponent<Controller>().GameButtonClicked(cell);
        //}
    }

    public void SetDisplay(List<int> nums, bool highlight, bool isClickable = true)
    {
        //Debug.Log("SETLOCKED: " + (setLocked ? "true" : "false"));
        isLocked = !isClickable;
        foreach (Text t in texts)
        {
            t.color = ALPHA;
        }
        if(nums.Count == 1)
        {
            Color thisColor = BLACK;
            if (isClickable) thisColor = WHITE;
            int thisNumber = nums[0];
            if(thisNumber < 0)
            {
                if(highlight) thisColor = RED;
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
                    if(highlight) thisColor = RED;
                    thisNum *= -1;
                }
                texts[thisNum].color = thisColor;
            }
        }
    }

    public void SetLoc(Vector2Int l)
    {
        cell = l;
    }
}
