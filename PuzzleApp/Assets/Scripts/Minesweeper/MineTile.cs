using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTile : MonoBehaviour
{
	public bool mine;               //This bool indicates whether this tile is a mine or not.
	public bool filled = false;     //This bool is set to true forever once the player tries to fill it.
	public bool flagged = false;    //This bool can be turned on/off by the player to flag the tile so long as tile isn't filled.

	public int xpos;    //The coordinates of the tile.
	public int ypos;    //We get them from the tile's coordinates in the world.

	int w, h;	//Keeping a copy of instance's width and height for convenience.

	void Start()
	{
		mine = Random.value < MineAssets.instance.mineFrequency;    //Each tile is initialized with a 15% chance of being a mine. Changing mineFrequency changes this value from anywhere from 0.05 to 0.25. We compare the value to a random number between 0 and 1, and turn that into a bool.

		xpos = (int)transform.position.x;                           //Storing the game world coordinates of the tile in the class variables.
		ypos = (int)transform.position.y;                           //These will be used to identify the tile.

		w = MineAssets.instance.w;									//Keeping track of the width of the board from instance.
		h = MineAssets.instance.h;									//Keeping track of the height of the board from instance.
		MineAssets.instance.tiles[xpos, ypos] = this;               //Making the game board reference this object, using the coordinates to identify it.
	}

	public bool fill(int count)	//This function fills the current tile, and the value you pass it lets the tile know what it should become when filled.
	{
		if (flagged) //If the tile is flagged,
		{
			if (mine) GetComponent<SpriteRenderer>().sprite = MineAssets.instance.flaggedMineTexture; //fill it if it's a mine, this happens ONLY when all the mines are revealed during loss. We never even reach this function in other cases.
			return mine;    //But if it is not a mine, return because we should never be allowed to fill a flag manually.
		}
		if (count == -2) GetComponent<SpriteRenderer>().sprite = MineAssets.instance.firstMineTexture;  //Special case.
		else if (mine) GetComponent<SpriteRenderer>().sprite = MineAssets.instance.mineTexture;         //Handles the texture for mines.
		else GetComponent<SpriteRenderer>().sprite = MineAssets.instance.filledTexture[count];          //Handles the textures for all other cases, the number on the tile is passed in to the function.

		filled = true;      //Set the tile to filled.

		return mine;
	}

	public void flag()          //This function handles flagging, what happens when we right click on the PC version.
	{
		if (filled) return;     //If the tile is filled, return because you're not allowed to flag a filled tile.
		if (flagged)            //If the tile is flagged,
		{
			flagged = false;    //unflag it.
			GetComponent<SpriteRenderer>().sprite = MineAssets.instance.unfilledTexture;
		}
		else
		{
			flagged = true;     //Otherwise, set it to flagged.
			GetComponent<SpriteRenderer>().sprite = MineAssets.instance.flaggedTexture;
		}

		if (MineAssets.instance.firstClick == true) MineAssets.instance.minesLeftText.text = "?";	//If the game isn't started, the mines left text should be '?'.
		else if (MineAssets.instance.gameLost == false) MineAssets.instance.minesLeftText.text = MineAssets.instance.board.totalUnflaggedMines().ToString();	//Otherwise it should be how many flags are now left. It doesn't care if something flagged is a mine or not, it just keeps track of the amount of flags you still need.
	}

	void OnMouseOver()
	{
		if (MineAssets.instance.gameWon | MineAssets.instance.gameLost) return; //If the game is over, return because we shouldn't allow any tiles to be flagged or filled.

		if (Input.GetMouseButtonDown(1) || (Input.touchCount > 0 ? Input.GetTouch(0).deltaTime >= 0.8f : false))	//If we right clicked.
		{
			flag();                         //Flag the selected tile.
			MineBoard.checkCompletion();	//Then check if we flagged the last mine, and if we did, tell the user that they won.
			return;
		}

		if (Input.GetMouseButtonDown(0))// || Input.touchCount > 0)        //If we left clicked or touched the screen
		{
			if (MineAssets.instance.firstClick) //If it's the very first click of the game, we have special rules.
			{
				this.mine = false;              //We never want the first click to be a mine, so set wherever we clicked to false.

				//Not only do we make sure the clicked tile isn't a mine, we make sure all adjacent tiles aren't mines either.
				//This effectively forces a bubble wherever the player decides to start, which should signficantly
				//reduce chances of the player having to make guesses.
				//We first check if a tile is on the board, before setting them to not be mines.
				if ((xpos - 1 >= 0) && (ypos - 1 >= 0)) { MineAssets.instance.tiles[xpos - 1, ypos - 1].mine = false; Debug.Log("(" + (xpos) + "," + (ypos) + ") - (" + (xpos - 1) + "," + (ypos - 1) + ")"); }
				if (ypos - 1 >= 0) { MineAssets.instance.tiles[xpos, ypos - 1].mine = false; Debug.Log("(" + (xpos) + "," + (ypos) + ") - (" + (xpos + 0) + "," + (ypos - 1) + ")"); }
				if ((xpos + 1 < w) && (ypos - 1 >= 0)) { MineAssets.instance.tiles[xpos + 1, ypos - 1].mine = false; Debug.Log("(" + (xpos) + "," + (ypos) + ") - (" + (xpos + 1) + "," + (ypos - 1) + ")"); }

				if (xpos - 1 >= 0) { MineAssets.instance.tiles[xpos - 1, ypos].mine = false; Debug.Log("(" + (xpos) + "," + (ypos) + ") - (" + (xpos - 1) + "," + (ypos + 0) + ")"); }
				if (xpos + 1 < w) { MineAssets.instance.tiles[xpos + 1, ypos].mine = false; Debug.Log("(" + (xpos) + "," + (ypos) + ") - (" + (xpos + 1) + "," + (ypos + 0) + ")"); }

				if ((xpos - 1 >= 0) && (ypos + 1 < h)) { MineAssets.instance.tiles[xpos - 1, ypos + 1].mine = false; Debug.Log("(" + (xpos) + "," + (ypos) + ") - (" + (xpos - 1) + "," + (ypos + 1) + ")"); }
				if (ypos + 1 < h) { MineAssets.instance.tiles[xpos, ypos + 1].mine = false; Debug.Log("(" + (xpos) + "," + (ypos) + ") - (" + (xpos + 0) + "," + (ypos + 1) + ")"); }
				if ((xpos + 1 < w) && (ypos + 1 < h)) { MineAssets.instance.tiles[xpos + 1, ypos + 1].mine = false; Debug.Log("(" + (xpos) + "," + (ypos) + ") - (" + (xpos + 1) + "," + (ypos + 1) + ")"); }


				MineAssets.instance.board.check(xpos, ypos);	//Fill the current tile, this'll also pop the bubble around it.

				MineAssets.instance.minesLeftText.text = MineAssets.instance.board.totalUnflaggedMines().ToString();

				MineAssets.instance.firstClick = false;   //We then set first click to be false, since we only want to do this once at the beginning of the game.

				return;
			}

			//If we aren't just starting the game.
			MineAssets.instance.board.check(xpos, ypos);	//Fill the current tile, this'll also pop any adjacent bubbles if there are any.
		}

	}

}
