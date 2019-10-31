using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameButton : MonoBehaviour
{
	public void newGameFunction()
	{
		print("Test NewGameButton");
		foreach (MineTile tile in MineBoard.tiles) tile.reinitialize();

		MineBoard.firstClick = true;
		MineBoard.gameWon = false;
		MineBoard.gameLost = false;

		Timer.resetState = true;
	}
}
