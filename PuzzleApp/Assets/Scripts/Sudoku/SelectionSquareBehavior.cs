﻿ using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionSquareBehavior : MonoBehaviour
{
    Color WHITE = new Color(1, 1, 1, 1);
    Color BLACK = new Color(0, 0, 0, 1);
    
    [SerializeField]
    private GameObject master;
    [SerializeField]
    private Text text;

    private int myNum;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMyNum(int num)
    {
        Debug.Log("Setting Num " + num);
        myNum = num;
        text.text = num.ToString(); 
    }

    public void OnClick()
    {
        master.GetComponent<GameController>().SelectButtonClicked(myNum);
    }

    public void SetActive(bool isActive)
    {
        if (isActive) text.color = BLACK;
        else text.color = WHITE;
    }
}