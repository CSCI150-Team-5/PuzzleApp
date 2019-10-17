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

	void Start()
	{
		mine = Random.value < MineAssets.instance.mineFrequency;	//Each tile is initialized with a 15% chance of being a mine.

		xpos = (int)transform.position.x;							//Storing the game world coordinates of the tile in the class variables.
		ypos = (int)transform.position.y;							//These will be used to identify the tile.

		MineAssets.instance.tiles[xpos, ypos] = this;				//Making the game board reference this object, using the coordinates to identify it.
	}

	public void fill(int count)
	{
		if (flagged) //If the tile is filled,
		{
			if (mine) GetComponent<SpriteRenderer>().sprite = MineAssets.instance.flaggedMineTexture; //flag it if it's a mine, this happens when all the mines are revealed during loss.
			return;	//But if it is not a mine, return because we should never be allowed to fill a flag manually.
		}
		if (count == -2) GetComponent<SpriteRenderer>().sprite = MineAssets.instance.firstMineTexture;	//Special case.
		else if (mine) GetComponent<SpriteRenderer>().sprite = MineAssets.instance.mineTexture;			//Handles the texture for mines.
		else GetComponent<SpriteRenderer>().sprite = MineAssets.instance.filledTexture[count];			//Handles the textures for all other cases, the number on the tile is passed in to the function.

		filled = true;		//Set the tile to filled.
		flagged = false;	//And not flagged, in case it was flagged.
	}

	public void flag()			//This function handles flagging, what happens when we right click on the PC version.
	{
		if (filled) return;		//If the tile is filled, return because you're not allowed to flag a filled tile.
		if (flagged)			//If the tile is flagged,
		{
			flagged = false;	//unflag it.
			GetComponent<SpriteRenderer>().sprite = MineAssets.instance.unfilledTexture;
		}
		else
		{
			flagged = true;		//Otherwise, set it to flagged.
			GetComponent<SpriteRenderer>().sprite = MineAssets.instance.flaggedTexture;

///			MineBoard.checkCompletion();	//And check if we won, for the scenario where we just flagged the last mine.
		}
	}

//	void Update()
//	void OnMouseDown()
	void OnMouseOver()
	{
//		Debug.Log("TestingOnMouseOver");
		if (MineAssets.instance.gameWon | MineAssets.instance.gameLost) return; //If the game is over, return because we shouldn't allow any tiles to be flagged or filled.
		if (Input.GetMouseButtonDown(0))		//If we left clicked.
		{
//			Debug.Log("TestingLeftClick");
			if (MineAssets.instance.firstClick)	//If it's the very first click of the game, we have special rules.
			{
				
				this.mine = false;				//We never want the first click to be a mine, so set wherever we clicked to false.
				int w = MineAssets.instance.w;	//Storing the width and height in a local variable so they're easier to access.
				int h = MineAssets.instance.h;	//

				//Not only do we make sure the clicked tile isn't a mine, we make sure all adjacent tiles aren't mines either.
				//This effectively forces a bubble wherever the player decides to start, which should signficantly
				//reduce chances of the player having to make guesses.
				//We first check if a tile is on the board, before setting them to not be mines.
				if ((xpos - 1 >= 0) && (ypos - 1 >= 0))	{ MineAssets.instance.tiles[xpos - 1, ypos - 1].mine = false;	Debug.Log("("+(xpos)+","+") - ("+(xpos - 1	)+","+(ypos - 1	)+")"); }
				if (ypos - 1 >= 0)						{ MineAssets.instance.tiles[xpos, ypos - 1].mine = false;		Debug.Log("("+(xpos)+","+") - ("+(xpos + 0	)+","+(ypos - 1	)+")"); }
				if ((xpos + 1 < w) && (ypos - 1 >= 0))	{ MineAssets.instance.tiles[xpos + 1, ypos - 1].mine = false;	Debug.Log("("+(xpos)+","+") - ("+(xpos + 1	)+","+(ypos - 1	)+")"); }

				if (xpos - 1 >= 0)						{ MineAssets.instance.tiles[xpos - 1, ypos].mine = false;		Debug.Log("("+(xpos)+","+") - ("+(xpos - 1	)+","+(ypos + 0	)+")"); }
				if (xpos + 1 < w)						{ MineAssets.instance.tiles[xpos + 1, ypos].mine = false;		Debug.Log("("+(xpos)+","+") - ("+(xpos + 1	)+","+(ypos + 0	)+")"); }

				if ((xpos - 1 >= 0) && (ypos + 1 < h))	{ MineAssets.instance.tiles[xpos - 1, ypos + 1].mine = false;	Debug.Log("("+(xpos)+","+") - ("+(xpos - 1	)+","+(ypos + 1	)+")"); }
				if (ypos + 1 < h)						{ MineAssets.instance.tiles[xpos, ypos + 1].mine = false;		Debug.Log("("+(xpos)+","+") - ("+(xpos + 0	)+","+(ypos + 1	)+")"); }
				if ((xpos + 1 < w) && (ypos + 1 < h))	{ MineAssets.instance.tiles[xpos + 1, ypos + 1].mine = false;	Debug.Log("("+(xpos)+","+") - ("+(xpos + 1	)+","+(ypos + 1	)+")"); }


///				MineCountDisplay.initialize();

				MineAssets.instance.firstClick = false;   //We then set first click to be false, since we only want to do this once at the beginning of the game.
			}
///			if (!flagged && mine) { MineBoard.loss(this); return; }	//If we hit a mine, then we lost.
//			{
//				MineBoard.revealAllMines(); //Reveal all the mines because we lost.
//				GetComponent<SpriteRenderer>().sprite = firstMineTexture;   //Make the mine we clicked on be a red mine, to highlight it separately from the other mines.
//				MineBoard.gameLost = true;  //Set the game to lost.
//				print("You lost.");
//				return;
//			}
			//fill(MineBoard.adjacentMines(xpos, ypos));
///			if (MineBoard.bulkMode) { MineBoard.bulkCheck(xpos, ypos, MineBoard.countMines(xpos, ypos)); MineBoard.popEmpties(); }
///			else MineBoard.popBubble(xpos, ypos, new bool[MineBoard.w, MineBoard.h]);    //If it's not a mine, then fill the current tile, and handle the case where it's a bubble.
		}

		if (Input.GetMouseButtonDown(1)) flag();    //If we right clicked, flag the tile.
//		if (Input.GetMouseButtonDown(1)) MineBoard.bulkCheck(xpos, ypos, MineBoard.countMines(xpos, ypos));
	}

}
