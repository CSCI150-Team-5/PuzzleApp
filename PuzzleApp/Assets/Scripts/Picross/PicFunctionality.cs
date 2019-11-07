using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicFunctionality : MonoBehaviour
{
<<<<<<< Updated upstream
	public GameObject[] menuObjects;    //0: Select Difficulty Label	1: 5x5 Difficulty Button
=======
 	public GameObject[] menuObjects;	//0: Select Difficulty Label	1: 5x5 Difficulty Button
>>>>>>> Stashed changes
	public GameObject[] gameUIObjects;  //0: 

	public void deactivateMenu(bool menuState)
	{
		for (int i = 0; i < menuObjects.Length; i++) menuObjects[i].SetActive(!menuState);
		for (int i = 0; i < gameUIObjects.Length; i++) gameUIObjects[i].SetActive(menuState);
	}
	public void generateBoard()
	{
		PicBoard board;
		PicAssets.instance.board = board = new PicBoard(PicAssets.instance.w, PicAssets.instance.h);

		Screen.orientation = ScreenOrientation.Portrait;
<<<<<<< Updated upstream
		//		int greater;
		//		if (w > h) greater = w;
		//		else greater = h;
		PicAssets.instance.cam.orthographicSize = PicAssets.instance.w + (float)3;// (w > h) ? w : h;// greater;// MineAssets.instance.h;
		PicAssets.instance.cam.transform.position = new Vector3(((float)PicAssets.instance.w / 2.0f) - ((PicAssets.instance.w < 10) ? 1f : 2.25f), ((float)PicAssets.instance.h / 2.0f) - 0.5f, -10);
		//		PicAssets.instance.can.transform.position = new Vector3(((float)PicAssets.instance.w / 2.0f) - ((PicAssets.instance.w < 10) ? 1f : 2.25f), ((float)PicAssets.instance.h / 2.0f) - 0.5f, -10);
		//		Camera.current.transform.Translate(new Vector3(1.0f, 1.0f, 1.0f));

		//		MineTile[] tiles = new MineTile[MineAssets.instance.w * MineAssets.instance.h];


		//		GameObject can = new GameObject("can", typeof(Canvas));
		//		can.transform.localPosition = PicAssets.instance.can.transform.localPosition;
		////		can.transform.offsetMax = PicAssets.instance.can.transform.offsetMax;
		////		can.transform.offsetMin = PicAssets.instance.can.transform.offsetMin;
		//		can.transform.localScale = PicAssets.instance.can.transform.localScale;
=======
//		int greater;
//		if (w > h) greater = w;
//		else greater = h;
		PicAssets.instance.cam.orthographicSize = PicAssets.instance.w + (float)3;// (w > h) ? w : h;// greater;// MineAssets.instance.h;
		PicAssets.instance.cam.transform.position = new Vector3(((float)PicAssets.instance.w / 2.0f) - ((PicAssets.instance.w < 10) ? 1f : 2.25f), ((float)PicAssets.instance.h / 2.0f) - 0.5f, -10);
//		PicAssets.instance.can.transform.position = new Vector3(((float)PicAssets.instance.w / 2.0f) - ((PicAssets.instance.w < 10) ? 1f : 2.25f), ((float)PicAssets.instance.h / 2.0f) - 0.5f, -10);
//		Camera.current.transform.Translate(new Vector3(1.0f, 1.0f, 1.0f));

//		MineTile[] tiles = new MineTile[MineAssets.instance.w * MineAssets.instance.h];


//		GameObject can = new GameObject("can", typeof(Canvas));
//		can.transform.localPosition = PicAssets.instance.can.transform.localPosition;
////		can.transform.offsetMax = PicAssets.instance.can.transform.offsetMax;
////		can.transform.offsetMin = PicAssets.instance.can.transform.offsetMin;
//		can.transform.localScale = PicAssets.instance.can.transform.localScale;
>>>>>>> Stashed changes


		for (int i = 0; i < PicAssets.instance.w; i++)
		{
			for (int j = 0; j < PicAssets.instance.h; j++)
			{
				GameObject tile = new GameObject("Tile (" + i + "," + j + ")", typeof(SpriteRenderer));
<<<<<<< Updated upstream
				tile.transform.parent = PicAssets.instance.can.transform;
				tile.GetComponent<SpriteRenderer>().sprite = PicAssets.instance.unfilledTexture;
				tile.transform.position = new Vector3(i, j);//Coordinates need to be fixed.
				tile.tag = "PicObject";
				tile.AddComponent<BoxCollider2D>();
				tile.AddComponent<PicTile>();
				//				tiles[i * MineAssets.instance.w + j] = tile.AddComponent<MineTile>();
=======
//				tile.transform.parent = PicAssets.instance.can.transform;
				tile.GetComponent<SpriteRenderer>().sprite = PicAssets.instance.unfilledTexture;
				tile.transform.position = new Vector3(i, j);//Coordinates need to be fixed.
				tile.tag = "PicObj";
				tile.AddComponent<BoxCollider2D>();
				tile.AddComponent<PicTile>();
//				tiles[i * MineAssets.instance.w + j] = tile.AddComponent<MineTile>();
				PicAssets.instance.tileObjects[i][j] = tile;
>>>>>>> Stashed changes
			}
		}
		////		int a = MineBoard.totalFlags();
		////		GameObject[] tiles = GameObject.FindGameObjectsWithTag("MineTileSprite");
		//		for (int i = 0; i < tiles.Length; i++)
		//		{
		//			tiles[i].board = MineAssets.instance.board;
		//		}

		//		PicAssets.instance.board.checkRows();
		//		PicBoard.checkRows();
		//		board.checkRows();

		//		PicAssets.instance.Hi();

		//		instance.rows = new List<List<int>>();
		//		foreach (PicTile tile in PicAssets.instance.tiles) if (tile.mine) tile.fill(-1);  //Reveal all the mines because we lost.
		for (int rowNumber = 0; rowNumber < PicAssets.instance.h; rowNumber++)
		{
			int current = 0;
			for (int i = 0; i < PicAssets.instance.w; i++)
			{
				if (PicAssets.instance.tiles[i][rowNumber].empty == true) current++;
				else if (current != 0)
				{
					PicAssets.instance.rows[rowNumber].Add(current);
					current = 0;
				}
			}
			if (current != 0) PicAssets.instance.rows[rowNumber].Add(current);

			if (PicAssets.instance.rows[rowNumber].Count == 0) PicAssets.instance.rows[rowNumber].Add(current);
			else if (PicAssets.instance.rows[rowNumber].Count > 5)
			{
				for (int i = 0; i < PicAssets.instance.w; i++)
				{
					if (PicAssets.instance.tiles[i][rowNumber].empty == true)
					{
						PicAssets.instance.tiles[i][rowNumber].empty = false;
						break;
					}
				}
				PicAssets.instance.rows[rowNumber].Clear();
				rowNumber--;
			}
		}

		for (int colNumber = 0; colNumber < PicAssets.instance.w; colNumber++)
		{
			int current = 0;
<<<<<<< Updated upstream
			for (int i = PicAssets.instance.h - 1; i >= 0; i--)
=======
			for (int i = PicAssets.instance.h -1; i >= 0; i--)
>>>>>>> Stashed changes
			{
				if (PicAssets.instance.tiles[colNumber][i].empty == true) current++;
				else if (current != 0)
				{
					PicAssets.instance.columns[colNumber].Add(current);
					current = 0;
				}
			}
			if (current != 0) PicAssets.instance.columns[colNumber].Add(current);

			if (PicAssets.instance.columns[colNumber].Count == 0) PicAssets.instance.columns[colNumber].Add(current);
			else if (PicAssets.instance.columns[colNumber].Count > 5)
			{
				Debug.Log("It happened on: " + colNumber);
				for (int i = 0; i < PicAssets.instance.h; i++)
				{
					if (PicAssets.instance.tiles[colNumber][i].empty == true)
					{
						PicAssets.instance.tiles[colNumber][i].empty = false;
						break;
					}
				}
				PicAssets.instance.columns[colNumber].Clear();
				colNumber--;
			}
		}
<<<<<<< Updated upstream
		//		for (int i = 0; i < PicAssets.instance.h; i++)
		//		{
		//			for (int j = 0; j < PicAssets.instance.tiles[i].Count; j++) Debug.Log("Row " + i + ", node " + j + ":	" + PicAssets.instance.tiles[i][j].empty);
		//
		//			Debug.Log("12346789");
		//		}
		Debug.Log(PicAssets.instance.gameStart);
		if (PicAssets.instance.gameStart) return;
=======

///		Debug.Log(PicAssets.instance.gameStart);
		if(PicAssets.instance.gameStart) return;
>>>>>>> Stashed changes
		for (int i = 0; i < PicAssets.instance.h; i++)
		{
			GameObject obj = new GameObject("Row " + i + " Label", typeof(CanvasRenderer));
			obj.AddComponent<Text>();
			string str = "";
			for (int j = 0; j < PicAssets.instance.rows[i].Count; j++)
			{
				str += PicAssets.instance.rows[i][j];
				if (j + 1 < PicAssets.instance.rows[i].Count) str += " ";
			}
			obj.GetComponent<Text>().text = str;// "" + (-2) + "," + i + "";
			obj.GetComponent<Text>().font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
			obj.GetComponent<Text>().fontSize = (int)(87.5f - PicAssets.instance.w * 2.5f);
<<<<<<< Updated upstream
			obj.GetComponent<Text>().color = Color.black;//gray;
=======
			obj.GetComponent<Text>().color = (str == "0") ? Color.gray : Color.black;//gray;
>>>>>>> Stashed changes
			obj.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Overflow;// horizontalOverflow;
			obj.GetComponent<Text>().alignment = TextAnchor.MiddleRight;

			obj.transform.parent = PicAssets.instance.can.transform;

			obj.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
			obj.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);

<<<<<<< Updated upstream
			obj.tag = "PicObject";

			obj.transform.localScale = new Vector3(1, 1, 1);
			obj.transform.position = new Vector3((PicAssets.instance.w < 10) ? -1f : -1.5f, i, -5);
=======
			obj.tag = "PicObj";

			obj.transform.localScale = new Vector3(1,1,1);
			obj.transform.position = new Vector3((PicAssets.instance.w < 10) ? -1f : -1.5f,i,-5);

			PicAssets.instance.rowObjects.Add(obj);
>>>>>>> Stashed changes
		}

		for (int i = 0; i < PicAssets.instance.w; i++)
		{
			GameObject obj = new GameObject("Column " + i + " Label", typeof(CanvasRenderer));
			obj.AddComponent<Text>();
			string str = "";
			for (int j = 0; j < PicAssets.instance.columns[i].Count; j++)
			{
				str += PicAssets.instance.columns[i][j];
				if (j + 1 < PicAssets.instance.columns[i].Count) str += "\n";
			}
			obj.GetComponent<Text>().text = str;
			obj.GetComponent<Text>().font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
			obj.GetComponent<Text>().fontSize = (int)(87.5f - PicAssets.instance.w * 2.5f);
<<<<<<< Updated upstream
			obj.GetComponent<Text>().color = Color.black;//gray;
=======
			obj.GetComponent<Text>().color = (str == "0") ? Color.gray : Color.black;//gray;
>>>>>>> Stashed changes
			obj.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Overflow;
			obj.GetComponent<Text>().alignment = TextAnchor.LowerCenter;

			obj.transform.parent = PicAssets.instance.can.transform;

			obj.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
			obj.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);

<<<<<<< Updated upstream
			obj.tag = "PicObject";

			obj.transform.localScale = new Vector3(1, 1, 1);
			obj.transform.position = new Vector3(i, ((PicAssets.instance.w < 10) ? 0f : 0.5f) + (float)PicAssets.instance.h, -5);
		}
=======
			obj.tag = "PicObj";

			obj.transform.localScale = new Vector3(1,1,1);
			obj.transform.position = new Vector3(i,((PicAssets.instance.w < 10) ? 0f : 0.5f) + (float)PicAssets.instance.h, -5);

			PicAssets.instance.columnObjects.Add(obj);
		}
		PicAI.operateOnRow(0);
>>>>>>> Stashed changes
	}

	//	public void waitForTiles()
	//	{
	//
	//	}

	public void select5x5()
	{
		deactivateMenu(true);

		PicAssets.instance.w = 5;
		PicAssets.instance.h = 5;

		PicAssets.instance.gameStart = true;

		generateBoard();
	}

	public void select10x10()
	{
		deactivateMenu(true);

		PicAssets.instance.w = 10;
		PicAssets.instance.h = 10;

		PicAssets.instance.gameStart = true;

		generateBoard();
	}

	public void select15x10()
	{
		deactivateMenu(true);

		PicAssets.instance.w = 15;
		PicAssets.instance.h = 10;

		PicAssets.instance.gameStart = true;

		generateBoard();
	}

	public void select15x15()
	{
		deactivateMenu(true);

		PicAssets.instance.w = 15;
		PicAssets.instance.h = 15;

		PicAssets.instance.gameStart = true;

		generateBoard();
	}

	public void newGame()
	{
<<<<<<< Updated upstream
		Debug.Log("Hello! ! ?");
		foreach (GameObject tile in GameObject.FindGameObjectsWithTag("PicObject"))
			Destroy(tile);

		//		PicAssets.instance.firstClick = true;
		//		PicAssets.instance.gameWon = false;
		//		PicAssets.instance.gameLost = false;

		//		PicAssets.instance.minesLeftText.text = "?";
		//		PicAssets.instance.timerText.text = "?";
		//		PicAssets.instance.timerInt = 0;
=======
		foreach (GameObject tile in GameObject.FindGameObjectsWithTag("PicObj"))
			Destroy(tile);

//		foreach (Vector3 vec in PicAssets.instance.history)
//			Debug.Log("History: " + (vec.x == 1f ? "Marked" : "Flagged" ) + "	(" + vec.y + "," + vec.z + ")");
		PicAssets.instance.history.Clear();
		PicAssets.instance.rehistory.Clear();

//		PicAssets.instance.firstClick = true;
//		PicAssets.instance.gameWon = false;
//		PicAssets.instance.gameLost = false;

//		PicAssets.instance.minesLeftText.text = "?";
//		PicAssets.instance.timerText.text = "?";
//		PicAssets.instance.timerInt = 0;
>>>>>>> Stashed changes

		PicAssets.instance.gameStart = false;

		generateBoard();
	}

	public void returnToMainMenu()
	{
<<<<<<< Updated upstream
		foreach (GameObject tile in GameObject.FindGameObjectsWithTag("PicObject"))
			Destroy(tile);

		//		MineAssets.instance.firstClick = true;
		//		MineAssets.instance.gameWon = false;
		//		MineAssets.instance.gameLost = false;

		//		MineAssets.instance.minesLeftText.text = "?";
		//		MineAssets.instance.timerText.text = "?";
		//		MineAssets.instance.timerInt = 0;
=======
		foreach (GameObject tile in GameObject.FindGameObjectsWithTag("PicObj"))
			Destroy(tile);

		PicAssets.instance.history.Clear();
		PicAssets.instance.rehistory.Clear();

//		MineAssets.instance.firstClick = true;
//		MineAssets.instance.gameWon = false;
//		MineAssets.instance.gameLost = false;

//		MineAssets.instance.minesLeftText.text = "?";
//		MineAssets.instance.timerText.text = "?";
//		MineAssets.instance.timerInt = 0;
>>>>>>> Stashed changes

		PicAssets.instance.board = null;

		deactivateMenu(false);
	}

<<<<<<< Updated upstream
=======
	public void undo()
	{
		if (PicAssets.instance.history.Count == 0) return;
		Vector3 toUndo = PicAssets.instance.history[PicAssets.instance.history.Count - 1];
		PicAssets.instance.history.RemoveAt(PicAssets.instance.history.Count - 1);
		PicAssets.instance.rehistory.Add(toUndo);

		if (toUndo.x == 1f)			PicAssets.instance.tiles[(int)toUndo.y][(int)toUndo.z].fill();
//		else if (toUndo.x == 0f)	PicAssets.instance.tiles[(int)toUndo.y][(int)toUndo.z].flag();
		else						PicAssets.instance.tiles[(int)toUndo.y][(int)toUndo.z].flag();
	}

	public void redo()
	{
		if (PicAssets.instance.rehistory.Count == 0) return;
		Vector3 toRedo = PicAssets.instance.rehistory[PicAssets.instance.rehistory.Count - 1];
		PicAssets.instance.rehistory.RemoveAt(PicAssets.instance.rehistory.Count - 1);
		PicAssets.instance.history.Add(toRedo);

		if (toRedo.x == 1f)			PicAssets.instance.tiles[(int)toRedo.y][(int)toRedo.z].fill();
//		else if (toRedo.x == 0f)	PicAssets.instance.tiles[(int)toRedo.y][(int)toRedo.z].flag();
		else						PicAssets.instance.tiles[(int)toRedo.y][(int)toRedo.z].flag();
	}

	void Update()
	{
		Debug.Log("TESTING "+PicAI.longestEmptyStreak(false,0,0,PicAssets.instance.w));
	}
>>>>>>> Stashed changes
}
