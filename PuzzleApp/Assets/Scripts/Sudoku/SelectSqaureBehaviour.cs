using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSqaureBehaviour : MonoBehaviour
{
    [SerializeField]
    private int myNum;
    [SerializeField]
    private Text textbox;
    [SerializeField]
    GameObject controller;

    // Start is called before the first frame update
    void Awake()
    {
        textbox.text = myNum.ToString();
    }

    private void OnEnable()
    {
        GameBehaviour.OnSelectedNumChanged += ChangeState;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeState(int n)
    {
        if(n==myNum)
        {
            textbox.color = new Color(0, 1, 0, 1);
        }
        else
        {
            textbox.color = new Color(0, 0, 0, 1);
        }
    }

    public void Click()
    {
        controller.GetComponent<GameBehaviour>().SetSelectedNum(myNum);
    }
}
