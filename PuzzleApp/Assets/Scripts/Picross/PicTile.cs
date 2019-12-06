using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicTile : MonoBehaviour
{
	public bool empty = false;		//This bool indicates whether this tile is empty or not.
	public bool filled = false;		//This bool can be turned on/off by the player to fill the tile.
	public bool flagged = false;	//This bool can be turned on/off by the player to flag the tile.

	public int xpos = -1;	//The coordinates of the tile.
	public int ypos = -1;	//We get them from the tile's coordinates in the world.

	int w = -1, h = -1;		//Width and height of the board, initialized with dummy values.

	public void fill()	//This function fills the tile then changes booleans.
	{
		GetComponent<SpriteRenderer>().sprite = filled ? (flagged ? PicAssets.instance.flaggedTexture : PicAssets.instance.unfilledTexture) : PicAssets.instance.filledTexture;
		filled = !filled;
		PicAssets.instance.rowObjects[ypos].GetComponent<Text>().color = PicAssets.instance.board.checkRow(ypos) == true ? Color.gray : Color.black;
		PicAssets.instance.columnObjects[xpos].GetComponent<Text>().color = PicAssets.instance.board.checkColumn(xpos) == true ? Color.gray : Color.black;

		PicAssets.instance.board.checkWon();
	}

	public void flag()	//This function flags the tile then changes booleans.
	{
		GetComponent<SpriteRenderer>().sprite = flagged ? (filled ? PicAssets.instance.filledTexture : PicAssets.instance.unfilledTexture) : PicAssets.instance.flaggedTexture;
		flagged = !flagged;
		PicAssets.instance.rowObjects[ypos].GetComponent<Text>().color = PicAssets.instance.board.checkRow(ypos) == true ? Color.gray : Color.black;
		PicAssets.instance.columnObjects[xpos].GetComponent<Text>().color = PicAssets.instance.board.checkColumn(xpos) == true ? Color.gray : Color.black;
	}

	public void justFill()	//This function ONLY fills.
	{
		GetComponent<SpriteRenderer>().sprite = PicAssets.instance.filledTexture;

		PicAssets.instance.rowObjects[ypos].GetComponent<Text>().color = PicAssets.instance.board.checkRow(ypos) == true ? Color.gray : Color.black;
		PicAssets.instance.columnObjects[xpos].GetComponent<Text>().color = PicAssets.instance.board.checkColumn(xpos) == true ? Color.gray : Color.black;
	}

	public void justFlag()	//This function ONLY flags.
	{
		GetComponent<SpriteRenderer>().sprite = PicAssets.instance.flaggedTexture;

		PicAssets.instance.rowObjects[ypos].GetComponent<Text>().color = PicAssets.instance.board.checkRow(ypos) == true ? Color.gray : Color.black;
		PicAssets.instance.columnObjects[xpos].GetComponent<Text>().color = PicAssets.instance.board.checkColumn(xpos) == true ? Color.gray : Color.black;
	}

	void Awake()	//When the tile is first awoken
	{
		empty = Random.value < PicAssets.instance.emptyFrequency;	//Each tile is initialized with a 15% chance of being empty.

		xpos = (int)transform.position.x;							//Storing the game world coordinates of the tile in the class variables.
		ypos = (int)transform.position.y;							//These will be used to identify the tile.

		w = PicAssets.instance.w;									//Grab a copy of the width and height for convenience.
		h = PicAssets.instance.h;									//

		PicAssets.instance.tiles[xpos][ypos] = this;                //Making the game board reference this object, using the coordinates to identify it.
		}

	void OnMouseOver()
	{
		if (PicAssets.instance.gameStart) return;	//If the game is still in the process of starting, return, don't even try to undo.
		if (PicAssets.instance.afterGame) return;	//If the game is over, return because we shouldn't allow any tiles to be flagged or filled.

		if (((PicAssets.instance.fillMode == false) && (Input.GetMouseButtonDown(0))) || Input.GetMouseButtonDown(1) || (Input.touchCount > 0 ? Input.GetTouch(0).deltaTime >= 0.8f : false))	//If we right clicked.
		{
			flag();                         //Flag the selected tile.

			PicAssets.instance.history.Add(new List<Vector3>() { new Vector3(0f, xpos, ypos) });	//Then update the move history.
			PicAssets.instance.rehistory.Clear();													//And overwrite the undo history.
			return;
		}

		if (Input.GetMouseButtonDown(0))        //If we left clicked or touched the screen
		{
			fill();							//Fill the selected tile.

			PicAssets.instance.history.Add(new List<Vector3>() { new Vector3(1f, xpos, ypos) });	//Then update the move history.
			PicAssets.instance.rehistory.Clear();													//And overwrite the undo history.
		}
	}
}
