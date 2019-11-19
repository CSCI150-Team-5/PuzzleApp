using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineFunctionality : MonoBehaviour
{
	public GameObject[] menuObjects;    //0: Select Difficulty Label	1: 3x3 Difficulty Button	2: 10x10 Difficulty Button
	public GameObject[] gameUIObjects;  //0: Mines Left Label	1: Time left Label
	public Slider sliderMineFrequency;

	public void deactivateMenu(bool menuState)
	{
		for (int i = 0; i < menuObjects.Length; i++) menuObjects[i].SetActive(!menuState);
		for (int i = 0; i < gameUIObjects.Length; i++) gameUIObjects[i].SetActive(menuState);
	}

	public void setMineCount()
	{
		Debug.Log("Testin456");
//		MineAssets.instance.minesLeftText.text = "Mines Left\n" + (MineBoard.totalMines() - MineBoard.totalFlags());
		//		if (MineAssets.instance.firstClick) MineAssets.instance.minesLeftText.text = "Mines Left\n" + "?";
		//		else if (MineAssets.instance.gameLost == false) MineAssets.instance.minesLeftText.text = "Mines Left\n" + (MineAssets.instance.minesTotal - MineBoard.totalFlags());//MineBoard.totalMinesLeft();
	}

	public void generateBoard()
	{
//		MineAssets.instance.board = new MineBoard(this, MineAssets.instance.w, MineAssets.instance.h);
		MineAssets.instance.board = new MineBoard(MineAssets.instance.w, MineAssets.instance.h);

		Screen.orientation = ScreenOrientation.Portrait;
//		int greater;
//		if (w > h) greater = w;
//		else greater = h;
		MineAssets.instance.cam.orthographicSize = MineAssets.instance.w;// (w > h) ? w : h;// greater;// MineAssets.instance.h;
		MineAssets.instance.cam.transform.position = new Vector3(((float)MineAssets.instance.w / 2.0f) - 0.5f, ((float)MineAssets.instance.h / 2.0f) - 0.5f, -10);
//		Camera.current.transform.Translate(new Vector3(1.0f, 1.0f, 1.0f));

//		MineTile[] tiles = new MineTile[MineAssets.instance.w * MineAssets.instance.h];

		for (int i = 0; i < MineAssets.instance.w; i++)
		{
			for (int j = 0; j < MineAssets.instance.h; j++)
			{
				GameObject tile = new GameObject("Tile (" + i + "," + j + ")", typeof(SpriteRenderer));
				tile.GetComponent<SpriteRenderer>().sprite = MineAssets.instance.unfilledTexture;
				tile.transform.position = new Vector3(i, j);
				tile.tag = "MineObj";
				tile.AddComponent<BoxCollider2D>();
				tile.AddComponent<MineTile>();
//				tiles[i * MineAssets.instance.w + j] = tile.AddComponent<MineTile>();
			}
		}
////		int a = MineBoard.totalFlags();
////		GameObject[] tiles = GameObject.FindGameObjectsWithTag("MineTileSprite");
//		for (int i = 0; i < tiles.Length; i++)
//		{
//			tiles[i].board = MineAssets.instance.board;
//		}
	}

	public void select5x5()
	{
		deactivateMenu(true);

		MineAssets.instance.w = 5;
		MineAssets.instance.h = 5;

		generateBoard();
	}

	public void select10x10()
	{
		deactivateMenu(true);

		MineAssets.instance.w = 10;
		MineAssets.instance.h = 10;

		generateBoard();
	}

	public void select10x15()
	{
		deactivateMenu(true);

		MineAssets.instance.w = 10;
		MineAssets.instance.h = 15;

		generateBoard();
	}

	public void setMineFrequency()
	{
		Debug.Log("SliderFreqValue is:" + sliderMineFrequency.value);
		MineAssets.instance.mineFrequency = (float)sliderMineFrequency.value /100f;
	}

	public void newGame()
	{
		foreach (GameObject tile in GameObject.FindGameObjectsWithTag("MineObj"))
		{
			Destroy(tile);
		}

		MineAssets.instance.firstClick = true;
		MineAssets.instance.gameWon = false;
		MineAssets.instance.gameLost = false;

		MineAssets.instance.minesLeftText.text = "?";
		MineAssets.instance.timerText.text = "?";
		MineAssets.instance.timerInt = 0;

		generateBoard();
	}

	public void returnToMainMenu()
	{
		foreach (GameObject tile in GameObject.FindGameObjectsWithTag("MineObj"))
		{
			Destroy(tile);
		}

		MineAssets.instance.firstClick = true;
		MineAssets.instance.gameWon = false;
		MineAssets.instance.gameLost = false;

		MineAssets.instance.minesLeftText.text = "?";
		MineAssets.instance.timerText.text = "?";
		MineAssets.instance.timerInt = 0;

		MineAssets.instance.board = null;

		deactivateMenu(false);
	}

	void Awake()
	{
//		MineAssets.instance.minesLeftText.text = "Mines Left\n?";
		deactivateMenu(false);
//		MineAssets.instance.func = this;
	}
}
