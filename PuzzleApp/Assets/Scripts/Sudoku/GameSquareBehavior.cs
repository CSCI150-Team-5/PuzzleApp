using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSquareBehavior : MonoBehaviour
{
    public delegate void gameChanged(int n, int[] pn);
    public static event gameChanged OnPlayedNumsChanged;

    public Text[] texts;
    private bool[] playedNums = {false, false, false, false, false, false, false, false, false, false };
    [SerializeField]
    GameObject controller;
    public int myLoc = 0;
    private bool isLocked;
    
    // Start is called before the first frame update
    void Awake()
    {
        SetVisibleTexts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click()
    {
        if (!isLocked)
        {
            int num = controller.GetComponent<GameBehaviour>().GetSelectedNum();
            Debug.Log(num);
            playedNums[num] = !playedNums[num];
            SetVisibleTexts();
            if (OnPlayedNumsChanged != null)
            {
                OnPlayedNumsChanged(myLoc, GetAllPlayed());
            }
        }
    }

    private void SetVisibleTexts()
    {
        int count = CountPlayed();
        if(count == 0)
        {
            foreach(Text text in texts)
            {
                text.color = new Color(0, 0, 0, 0);
            }
            //Debug.Log("Count 0");
        }
        else if(count == 1)
        {
            int num = GetLowestPlayed();
            texts[0].text = num.ToString();
            texts[0].color = new Color(0, 0, 0, 1);
            for(int i = 1; i<10; ++i)
            {
                texts[i].color = new Color(0, 0, 0, 0);
            }
            
        }
        else
        {
            for(int i = 0; i < 10; ++i)
            {
                if(playedNums[i])
                {
                    texts[i].color = new Color(0, 0, 0, 1);
                }
                else
                {
                    texts[i].color = new Color(0, 0, 0, 0);
                }
            }
        }
    }

    private int CountPlayed()
    {
        int count = 0;
        foreach(bool isPlayed in playedNums)
        {
            if (isPlayed) count++;
        }
        return count;
    }

    private int GetLowestPlayed()
    {
        for(int i = 0; i < 10; ++i)
        {
            if(playedNums[i])
            {
                return i;
            }
        }
        return -1;
    }

    private int[] GetAllPlayed()
    {
        int[] played = new int[10];
        int index = 0;
        for(int i = 0; i < 10; ++i)
        {
            if(playedNums[i])
            {
                played[index++] = i;
            }
        }
        return played;
    }

    public void SetNumber(int num, bool setLock)
    {
       
        for(int i =0; i<10; ++i) 
        {
            playedNums[i] = false;
        }
        if(num>0) playedNums[num] = true;
        SetVisibleTexts();
        isLocked = setLock;
    }
}
