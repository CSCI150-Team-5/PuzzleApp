using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineCountDisplay : MonoBehaviour
{
	public Text minesLeftText;

	// Update is called once per frame
	void Update()
    {
		if (MineBoard.firstClick == true) minesLeftText.text = "Mines Left:" + "?";
		else if (MineBoard.gameLost == false) minesLeftText.text = "Mines Left:" + MineBoard.totalMinesLeft();
	}
}
