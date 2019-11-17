using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameBtnBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject master;
    public void OnClick()
    {
        master.GetComponent<GameController>().NewGameButtonClicked();
    }
}
