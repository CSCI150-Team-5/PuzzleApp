using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	public int timeInt;
	public Text timeText;

	public static bool resetState = false;


	// Update is called once per frame
	void Update()
	{
		if (MineBoard.firstClick == false)
		{
			if ((MineBoard.gameLost == false) && (MineBoard.gameWon == false))
			{
				timeInt++;
				timeText.text = "" + timeInt;
			}
		}

		if(resetState)
		{
			resetState = false;

			timeInt = 0;
			timeText.text = "" + timeInt;
		}
	}
}
