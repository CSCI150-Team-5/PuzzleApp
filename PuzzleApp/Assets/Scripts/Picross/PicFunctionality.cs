using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PicFunctionality : MonoBehaviour
{
	//Assign these Game Objects manually in the editor if they are left unassigned.
	public GameObject[] menuObjects;    //0: Select Difficulty Label	1: 5x5 Difficulty Button	2: 10x10 Difficulty Button	3: 15x10 Difficulty Button	4: 15x15 Difficulty Button	5: Text Empty Frequency	6:Slider Empty Frequency	7: Text Frequency Low	8: Text Frequency High
	public GameObject[] gameUIObjects;  //0: Empty Left Label	1: Empty Left Count	2: Time Left Label	3: Time Left Count	4: New Game Button	5: Return Button	6: Undo Button	7: Redo Button	8: Select Flag Button	9: Select Fill Button	10: Selection Border
	public Slider sliderEmptyFrequency; //The Empty Frequency Slider, it's in menuObjects but I also have it individually, it needs to be assigned to both.
	public GameObject border;           //The picker border, it's in gameUIObjects but I also have it individually, it needs to be assigned to both.
	public GameObject tooltipYouWon;    //The winner tooltip, it's in gameUIObjects but I also have it individually, it needs to be assigned to both.

	public void deactivateMenu(bool menuState)  //Depending on if we're loading the menu or game, it activates the menu objects or the UI objects.
	{
		for (int i = 0; i < menuObjects.Length; i++) menuObjects[i].SetActive(!menuState);
		for (int i = 0; i < gameUIObjects.Length; i++) gameUIObjects[i].SetActive(menuState);
	}

	public void generateBoard() //The function for generating the board. We're assuming the width and height was already assigned to instance.
	{
		PicAssets.instance.board = new PicBoard(PicAssets.instance.w, PicAssets.instance.h);    //Assigning the board to instance.

		//The following is some cleanup to move the camera to the correct location and setting the scale. The camera scales with the width/height we chose.
		Screen.orientation = ScreenOrientation.Portrait;
		PicAssets.instance.cam.orthographicSize = PicAssets.instance.w + (float)3;// (w > h) ? w : h;// greater;// MineAssets.instance.h;
		PicAssets.instance.cam.transform.position = new Vector3(((float)PicAssets.instance.w / 2.0f) - ((PicAssets.instance.w < 10) ? 1f : 2.25f), ((float)PicAssets.instance.h / 2.0f) - 0.5f, -10);

		//This section is for generating all the tiles themselves, graphically in the game world, and assigning them corresponding tile scripts as well.
		for (int i = 0; i < PicAssets.instance.w; i++)
		{
			for (int j = 0; j < PicAssets.instance.h; j++)
			{
				GameObject tile = new GameObject("Tile (" + i + "," + j + ")", typeof(SpriteRenderer)); //Name of the tile so it's easily identifiable. X comma y coordinates in the name.
				tile.GetComponent<SpriteRenderer>().sprite = PicAssets.instance.unfilledTexture;		//The default texture is unfilled.
				tile.transform.position = new Vector3(i, j,100);										//Setting its position relative to the world.
				tile.tag = "PicObj";																	//A tag I use for keeping track of the object.
				tile.AddComponent<BoxCollider2D>();														//Giving it collision.
				tile.AddComponent<PicTile>();															//Assigning it the Mine Tile script.
				PicAssets.instance.tileObjects[i][j] = tile;											//Assigning the tile to the list.
			}
		}

		//These functions go through and check that the numbers in the rows/columns aren't off screen, and if they are, they change the board until they aren't.
		//Then it stores that array of numbers in instance to be used later.
		//Row first
		for (int rowNumber = 0; rowNumber < PicAssets.instance.h; rowNumber++)	//One run of the loop for each row.
		{
			int current = 0;	//How long of a streak we have.
			for (int i = 0; i < PicAssets.instance.w; i++)	//Itterating across the row.
			{
				if (PicAssets.instance.tiles[i][rowNumber].empty == true) current++;	//If the tile should be empty, increment.
				else if (current != 0)													//If the tiles were empty but is no longer empty, that means we reached the end of a streak.
				{
					PicAssets.instance.rows[rowNumber].Add(current);					//At which point, save the number
					current = 0;														//then reset the streak to 0.
				}
			}
			if (current != 0) PicAssets.instance.rows[rowNumber].Add(current);			//If there is still a number left after we reached the end of the board, add that number.

			if (PicAssets.instance.rows[rowNumber].Count == 0) PicAssets.instance.rows[rowNumber].Add(current);	//If we didn't add any numbers at all, add a 0 to the list for that row.
			else if (PicAssets.instance.rows[rowNumber].Count > ((PicAssets.instance.w < 15) ? 5 : 4))	//This section is for checking if the numbers go off screen. We only allow a row to have 5 numbers in small heights, and only 4 in larger heights, to allocate for screen and font size.
			{
				for (int i = 0; i < PicAssets.instance.w; i++)	//Itterate across the row
				{
					if (PicAssets.instance.tiles[i][rowNumber].empty == true)	//Find the first nonempty
					{
						PicAssets.instance.tiles[i][rowNumber].empty = false;	//Set it to an empty
						break;	//Then break
					}
				}
				PicAssets.instance.rows[rowNumber].Clear();	//Erase the current row's values in the list
				rowNumber--;	//And go back in the for loop, redo this row until we don't reach this scope.
			}
		}

		//Now we do the same with the columns but we keep a bool to keep track of if the columns were changed. If they were, rows need to also be updated.
		//We itterate backwards for height since we read numbers top to bottom instead of bottom to top.
		bool heightChangedTiles = false;
		for (int colNumber = 0; colNumber < PicAssets.instance.w; colNumber++)
		{
			int current = 0;
			for (int i = PicAssets.instance.h -1; i >= 0; i--)
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
			else if (PicAssets.instance.columns[colNumber].Count > ((PicAssets.instance.h < 15) ? 5 : 4))
			{
				heightChangedTiles = true;
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

		//If the columns were changed, lets make the rows match so there isn't any inconsistencies. Same code as before for row, but less checking because we already did that.
		if (heightChangedTiles)
		{
			PicAssets.instance.rows = new List<List<int>>(PicAssets.instance.h);
			for (int i = 0; i < PicAssets.instance.h; i++)
				PicAssets.instance.rows.Add(new List<int>());

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
			}
		}


		generateLines();	//Draw lines to the screen if we're in 10x10 mode or greater, immediately returns if in 5x5 mode.


		if(PicAssets.instance.gameStart) return;	//If the game hasn't started yet, return, don't draw the rest.


		//Draw the rows to the screen, largely the same logic as the tiles.
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
			obj.GetComponent<Text>().text = str;
			obj.GetComponent<Text>().font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
			obj.GetComponent<Text>().fontSize = (int)(87.5f - PicAssets.instance.w * 2.5f);	//Font size scales with the width.
			obj.GetComponent<Text>().color = (str == "0") ? Color.gray : Color.black;	//A solved row is gray, an unsolved is black. Elsewhere in the code we change the color of a row. But if a row is 0, then it's already solved, so it starts out gray.
			obj.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Overflow;
			obj.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Overflow;
			obj.GetComponent<Text>().alignment = TextAnchor.MiddleRight;

			obj.transform.parent = PicAssets.instance.can.transform;

			obj.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
			obj.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);

			obj.tag = "PicObj";

			obj.transform.localScale = new Vector3(1,1,1);
			obj.transform.position = new Vector3(((PicAssets.instance.w < 10) ? -2f : ((PicAssets.instance.w < 15) ? -3f : -3.5f)), i, -5);				//Position of the rows depends on the width.
			obj.GetComponent<RectTransform>().sizeDelta = new Vector2((PicAssets.instance.w < 15) ? 350 : 300, (PicAssets.instance.w < 10) ? 100 : 50);	//

			//The following is a lambda function that is activated when a gray row is clicked on, it flags all unflagged tiles in the row.
			obj.AddComponent<Button>();
			obj.GetComponent<Button>().onClick.AddListener(() => { if (!PicAssets.instance.gameStart && !PicAssets.instance.afterGame) if (obj.GetComponent<Text>().color == Color.gray) PicAssets.instance.board.markRow((int)Math.Round(obj.transform.position.y)); });

			PicAssets.instance.rowObjects.Add(obj);	//Keep track of this game object.
		}


		//Draw the columns to the screen, same logic as rows except height and width are swapped.
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
			obj.GetComponent<Text>().color = (str == "0") ? Color.gray : Color.black;
			obj.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Overflow;
			obj.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Overflow;
			obj.GetComponent<Text>().alignment = TextAnchor.LowerCenter;

			obj.transform.parent = PicAssets.instance.can.transform;

			obj.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
			obj.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);

			obj.tag = "PicObj";

			obj.transform.localScale = new Vector3(1,1,1);
			obj.transform.position = new Vector3(i, ((PicAssets.instance.w < 10) ? 0f : 0.5f) + (float)PicAssets.instance.h + ((PicAssets.instance.w < 10) ? 1 : ((PicAssets.instance.w < 15) ? 1.5f : 2f)), -5);
			obj.GetComponent<RectTransform>().sizeDelta = new Vector2((PicAssets.instance.w < 10) ? 100 : 50, (PicAssets.instance.w < 15) ? 350 : 300);

			obj.AddComponent<Button>();
			obj.GetComponent<Button>().onClick.AddListener(() => { if (!PicAssets.instance.gameStart && !PicAssets.instance.afterGame) if (obj.GetComponent<Text>().color == Color.gray) PicAssets.instance.board.markColumn((int)Math.Round(obj.transform.position.x)); });

			PicAssets.instance.columnObjects.Add(obj);
		}

		tooltipYouWon.transform.SetSiblingIndex(1000);  //Ensuring that the tooltip is above everything else.

		PicAssets.instance.stopTimer = false;	//Start the timer.

// The following code is for the AI which is not yet complete. It should run as soon as a puzzle is generated, and if the AI doesn't struggle to solve the puzzle, that means the puzzle is solvable. If the AI cannot solve the puzzle, then draw a new board instead of giving the player this board. //

//		PicAI.operateOnRow(true, 0);///

//		for (int row = 0; row < PicAssets.instance.h; row++)
//		{
////			PicAssets.instance.tiles[1][row].GetComponent<SpriteRenderer>().sprite = PicAssets.instance.filledTexture;
//			PicAssets.instance.tiles[PicAssets.instance.w - 2][row].GetComponent<SpriteRenderer>().sprite = PicAssets.instance.filledTexture;
//			List<Vector2> definitelyFill = PicAI.operateOnRow(true, row);
//			for (int i = 0; i < definitelyFill.Count; i++)
//			{
//				PicAssets.instance.tiles[(int)definitelyFill[i].x][(int)definitelyFill[i].y].justFill();
//			}
//		}

//		for (int row = 0; row < PicAssets.instance.w; row++)
//		{
////			PicAssets.instance.tiles[row][PicAssets.instance.h - 2].GetComponent<SpriteRenderer>().sprite = PicAssets.instance.filledTexture;
//			PicAssets.instance.tiles[row][1].GetComponent<SpriteRenderer>().sprite = PicAssets.instance.filledTexture;
//			List<Vector2> definitelyFill = PicAI.operateOnRow(false, row);
//			for (int i = 0; i < definitelyFill.Count; i++)
//			{
//				PicAssets.instance.tiles[(int)definitelyFill[i].x][(int)definitelyFill[i].y].justFill();
//			}
//		}
	}

	public void generateLines()	//Generates the vertical and horizontal lines associated with game modes larger than 5x5.
	{
		int w = PicAssets.instance.w;	//Keep tracking of width and height here for convenience.
		int h = PicAssets.instance.h;	//

		if ((w == 5) && (h == 5)) return;	//If we're in 5x5, return, we don't need to do anything.

		//Generating all the lines, how many lines and their positons and thickness depends on the game mode and is largely hard coded in.
		GameObject obj = new GameObject("Row Line " + 5, typeof(CanvasRenderer));
		obj.AddComponent<Image>();
		obj.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f);
		obj.transform.parent = PicAssets.instance.can.transform;
		obj.transform.localScale = new Vector3(1, 1, 1);
		obj.transform.localPosition = new Vector3(w == 15 ? 94 : 130, h == 15 ? -133 : 0, 0);
		obj.GetComponent<RectTransform>().sizeDelta = new Vector2(w == 15 ? 795 : 730, w == 15 ? 6 : 8);
		obj.tag = "PicObj";
		PicAssets.instance.lines.Add(obj);

		obj = new GameObject("Column Line " + 5, typeof(CanvasRenderer));
		obj.AddComponent<Image>();
		obj.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f);
		obj.transform.parent = PicAssets.instance.can.transform;
		obj.transform.localScale = new Vector3(1, 1, 1);
		obj.transform.localPosition = new Vector3(w == 15 ? -40 : 130, 0, 0);
		obj.GetComponent<RectTransform>().sizeDelta = new Vector2(w == 15 ? 6 : 8, h == 15 ? 795 : w == 15 ? 530 : 730);
		obj.tag = "PicObj";
		PicAssets.instance.lines.Add(obj);

		if (w == 15)
		{
			obj = new GameObject("Column Line " + 10, typeof(CanvasRenderer));
			obj.AddComponent<Image>();
			obj.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f);
			obj.transform.parent = PicAssets.instance.can.transform;
			obj.transform.localScale = new Vector3(1, 1, 1);
			obj.transform.localPosition = new Vector3(227, 0, 0);
			obj.GetComponent<RectTransform>().sizeDelta = new Vector2(5, h == 15 ? 795 : 530);
			obj.tag = "PicObj";
			PicAssets.instance.lines.Add(obj);
		}

		if (h == 15)
		{
			obj = new GameObject("Row Line " + 10, typeof(CanvasRenderer));
			obj.AddComponent<Image>();
			obj.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f);
			obj.transform.parent = PicAssets.instance.can.transform;
			obj.transform.localScale = new Vector3(1, 1, 1);
			obj.transform.localPosition = new Vector3(94, 133, 0);
			obj.GetComponent<RectTransform>().sizeDelta = new Vector2(795, 5);
			obj.tag = "PicObj";
			PicAssets.instance.lines.Add(obj);
		}
	}

	//The following functions are for the buttons, they are all identical other than the size that is assigned.
	public void select5x5()
	{
		deactivateMenu(true);                   //Deactivate the menu objects and activate the UI objects.

		PicAssets.instance.w = 5;				//Assign the width and height to instance.
		PicAssets.instance.h = 5;				//These are used in the generateBoard function as well.

		PicAssets.instance.gameStart = true;	//Set the game to have started.

		generateBoard();                        //Now generate the board.
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

	public void setEmptyFrequency()	//This function is invoked when you move the slider for empty frequency in the menu.
	{
		PicAssets.instance.emptyFrequency = (100f - (float)sliderEmptyFrequency.value) / 100f;  //The value the slider is set at is turned into a percentage and stored in instance's emptyFrequency. Note that it's 100 - the slider value, not the slider value.
	}

	public void newGame()   //This function deletes the previous instance of the game and creates a new one, resetting everything as needed.
	{
		foreach (GameObject tile in GameObject.FindGameObjectsWithTag("PicObj"))    //Grabs every single tile, I assigned all tiles the PicObj tag on creation so this is how we find them.
			Destroy(tile);	//Delete those game objects.

		PicAssets.instance.history.Clear();		//Clearing the histories.
		PicAssets.instance.rehistory.Clear();	//

		tooltipYouWon.SetActive(false);			//Removing the you won tooltip since we're starting a new game.

		PicAssets.instance.gameStart = false;   //Reset all the booleans.
		PicAssets.instance.gameWon = false;		//
		PicAssets.instance.afterGame = false;	//

		PicAssets.instance.timerText.text = "?";	//Reset all the UI values.
		PicAssets.instance.timerInt = 0;			//

		generateBoard();					    //Generate a new game, using the same width/height.
	}

	public void returnToMainMenu()  //This function deletes the previous instance of the game and returns to the minesweeper menu.
	{
		foreach (GameObject tile in GameObject.FindGameObjectsWithTag("PicObj"))
			Destroy(tile);

		PicAssets.instance.history.Clear();     //Clearing the histories.
		PicAssets.instance.rehistory.Clear();   //

		tooltipYouWon.SetActive(false);         //Removing the you won tooltip since we're starting a new game.

		PicAssets.instance.gameStart = false;   //Reset all the booleans.
		PicAssets.instance.gameWon = false;		//
		PicAssets.instance.afterGame = false;   //

		PicAssets.instance.timerText.text = "?";	//Reset all the UI values.
		PicAssets.instance.timerInt = 0;			//

		PicAssets.instance.board = null;		//Set the board to null, this lets the code know that the previous board is okay to delete since nothing points to it anymore.

		deactivateMenu(false);                  //Turn all the UI objects off and turn back on the menu objects to return to the picross menu.
	}

	public void undo()	//This function handles when the player hit undo.
	{
		if (PicAssets.instance.gameStart) return;	//If the game is still in the process of starting, return, don't even try to undo.
		if (PicAssets.instance.afterGame) return;	//If the game is over, return because we shouldn't allow any tiles to be flagged or filled.

		if (PicAssets.instance.history.Count == 0) return;	//If there is nothing to undo, return.

		for (int i = PicAssets.instance.history[PicAssets.instance.history.Count - 1].Count - 1; i >= 0; i--)	//Undo the entire last move, whether the last move only had 1 change or several changes. If it had several, itterate over them all.
		{	//A move might have multiple changes if you clicked on a gray row/column to flag multiple tiles at once.
			Vector3 toUndo = PicAssets.instance.history[PicAssets.instance.history.Count - 1][i];	//Grab the move we're undoing.
	
			if (toUndo.x == 1f)			PicAssets.instance.tiles[(int)toUndo.y][(int)toUndo.z].fill();	//If it was previously filled/unfilled, re-fill it.
			else						PicAssets.instance.tiles[(int)toUndo.y][(int)toUndo.z].flag();	//If it was previously flagged/unflagged, re-flag it.
		}

		PicAssets.instance.rehistory.Add(PicAssets.instance.history[PicAssets.instance.history.Count - 1]);	//Add this set of moves to the undone history.
		PicAssets.instance.history.RemoveAt(PicAssets.instance.history.Count - 1);	//Remove it from the undo history.
	}

	public void redo()	//Largely the exact same code as undo, except with undo and redo history swapped.
	{
		if (PicAssets.instance.gameStart) return;
		if (PicAssets.instance.afterGame) return;

		if (PicAssets.instance.rehistory.Count == 0) return;

		for (int i = PicAssets.instance.rehistory[PicAssets.instance.rehistory.Count - 1].Count - 1; i >= 0; i--)
		{
			Vector3 toRedo = PicAssets.instance.rehistory[PicAssets.instance.rehistory.Count - 1][i];
	
			if (toRedo.x == 1f)			PicAssets.instance.tiles[(int)toRedo.y][(int)toRedo.z].fill();
			else						PicAssets.instance.tiles[(int)toRedo.y][(int)toRedo.z].flag();
		}

		PicAssets.instance.history.Add(PicAssets.instance.rehistory[PicAssets.instance.rehistory.Count - 1]);
		PicAssets.instance.rehistory.RemoveAt(PicAssets.instance.rehistory.Count - 1);
	}

	public void selectFlag()	//When the player selects flag mode, this changes the corresponding value in PicAssets that is used in PicTile.
	{
		if (PicAssets.instance.gameStart) return;   //If the game is still in the process of starting, return, don't even try to undo.
		if (PicAssets.instance.afterGame) return;	//If the game is over, return because we shouldn't allow any tiles to be flagged or filled.

		PicAssets.instance.fillMode = false;	//Sets the bool.
		border.GetComponent<RectTransform>().anchoredPosition = new Vector3(120, 190, 0);	//Re-positions the blue border to be over this button instead of the other button.
	}

	public void selectFill()    //When the player selects fill mode, this changes the corresponding value in PicAssets that is used in PicTile.
	{
		if (PicAssets.instance.gameStart) return;   //If the game is still in the process of starting, return, don't even try to undo.
		if (PicAssets.instance.afterGame) return;	//If the game is over, return because we shouldn't allow any tiles to be flagged or filled.

		PicAssets.instance.fillMode = true; //Sets the bool.
		border.GetComponent<RectTransform>().anchoredPosition = new Vector3(-120, 190, 0);  //Re-positions the blue border to be over this button instead of the other button.
	}

	public void selectGameWonTooltip()	//When you click on the tooltip
	{
		tooltipYouWon.SetActive(false);	//It should go away.
	}

	void Update()
	{
		if (PicAssets.instance.gameWon)	//If we JUST won the game.
		{
			tooltipYouWon.SetActive(true);			//Turn on the tooltip.
			PicAssets.instance.gameWon = false;		//Then we are past the moment immediately after just winning.
			PicAssets.instance.afterGame = true;    //We are now waiting.
			PicAssets.instance.stopTimer = true;	//Stop the timer.
		}
	}
}
