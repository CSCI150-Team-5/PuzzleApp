 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionSquareBehavior : MonoBehaviour
{
    static Color WHITE = new Color(1, 1, 1, 1);
    static Color BLACK = new Color(0, 0, 0, 1);
    
    [SerializeField]
    private GameObject master;
    [SerializeField]
    private Text text;

    private int myNum;

    public void SetMyNum(int num)
    {
        Debug.Log("Setting Num " + num);
        myNum = num;
        text.text = num.ToString(); 
    }

    public void OnClick()
    {
        master.GetComponent<Controller>().SelectButtonClicked(myNum);
    }

    public void SetActive(bool isActive)
    {
        text.color = isActive ? BLACK : WHITE;
    }
}
