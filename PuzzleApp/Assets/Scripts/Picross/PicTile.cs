using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicTile : MonoBehaviour
{
	public bool empty = false;      //This bool indicates whether this tile is empty or not.
	public bool filled = false;     //This bool can be turned on/off by the player to fill the tile.
	public bool flagged = false;    //This bool can be turned on/off by the player to flag the tile.

	public int xpos = -1;   //The coordinates of the tile.
	public int ypos = -1;   //We get them from the tile's coordinates in the world.

	public PicBoard board = null;

	int w = -1, h = -1;

	public PicTile(bool b) { }

	public void fill()
	{
		GetComponent<SpriteRenderer>().sprite = filled ? PicAssets.instance.unfilledTexture : PicAssets.instance.filledTexture;
		filled = !filled;
		flagged = !filled;
	}

	public void flag()
	{
		GetComponent<SpriteRenderer>().sprite = flagged ? PicAssets.instance.unfilledTexture : PicAssets.instance.flaggedTexture;
		flagged = !flagged;
		filled = !flagged;
	}

	//	void Start()
	void Awake()
	//	public PicTile()
	{
		empty = Random.value < PicAssets.instance.emptyFrequency;   //Each tile is initialized with a 15% chance of being empty.

		xpos = (int)transform.position.x;                           //Storing the game world coordinates of the tile in the class variables.
		ypos = (int)transform.position.y;                           //These will be used to identify the tile.

		w = PicAssets.instance.w;
		h = PicAssets.instance.h;
		//		PicAssets.instance.tiles[xpos, ypos] = this;                //Making the game board reference this object, using the coordinates to identify it.
		//		PicAssets.instance.tiles[xpos].Add(this);                //Making the game board reference this object, using the coordinates to identify it.
		PicAssets.instance.tiles[xpos][ypos] = this;                //Making the game board reference this object, using the coordinates to identify it.
																	//		Debug.Log("" + (PicAssets.instance.tiles[xpos][ypos].empty == empty) + "	" + empty);

		board = PicAssets.instance.board;
		//		if(empty) { GetComponent<SpriteRenderer>().sprite = PicAssets.instance.filledTexture; filled = true; }///
	}

	void OnMouseOver()
	{
		if (PicAssets.instance.gameStart) return;
		if (PicAssets.instance.gameWon) return; //If the game is over, return because we shouldn't allow any tiles to be flagged or filled.

		if (Input.GetMouseButtonDown(1) || (Input.touchCount > 0 ? Input.GetTouch(0).deltaTime >= 0.8f : false))    //If we right clicked.
		{
			flag();                         //Flag the selected tile.
											///			PicBoard.checkCompletion();		//Then check if we flagged the last empty spot, and if we did, tell the user that they won.
			return;
		}

		if (Input.GetMouseButtonDown(0))        //If we left clicked or touched the screen
		{
			//MineAssets.instance.board.check(xpos, ypos);
			fill();
		}

	}

}
