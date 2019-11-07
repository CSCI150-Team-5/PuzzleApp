using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< Updated upstream

public class PicTile : MonoBehaviour
{
	public bool empty = false;      //This bool indicates whether this tile is empty or not.
	public bool filled = false;     //This bool can be turned on/off by the player to fill the tile.
	public bool flagged = false;    //This bool can be turned on/off by the player to flag the tile.

	public int xpos = -1;   //The coordinates of the tile.
	public int ypos = -1;   //We get them from the tile's coordinates in the world.
=======
using UnityEngine.UI;

public class PicTile : MonoBehaviour
{
	public bool empty = false;		//This bool indicates whether this tile is empty or not.
	public bool filled = false;		//This bool can be turned on/off by the player to fill the tile.
	public bool flagged = false;	//This bool can be turned on/off by the player to flag the tile.

	public int xpos = -1;	//The coordinates of the tile.
	public int ypos = -1;	//We get them from the tile's coordinates in the world.
>>>>>>> Stashed changes

	public PicBoard board = null;

	int w = -1, h = -1;

	public PicTile(bool b) { }

	public void fill()
	{
<<<<<<< Updated upstream
		GetComponent<SpriteRenderer>().sprite = filled ? PicAssets.instance.unfilledTexture : PicAssets.instance.filledTexture;
		filled = !filled;
		flagged = !filled;
=======
		GetComponent<SpriteRenderer>().sprite = filled ? (flagged ? PicAssets.instance.flaggedTexture : PicAssets.instance.unfilledTexture) : PicAssets.instance.filledTexture;
		filled = !filled;
		PicAssets.instance.rowObjects[ypos].GetComponent<Text>().color = PicAssets.instance.board.checkRow(ypos) == true ? Color.gray : Color.black;
		PicAssets.instance.columnObjects[xpos].GetComponent<Text>().color = PicAssets.instance.board.checkColumn(xpos) == true ? Color.gray : Color.black;
>>>>>>> Stashed changes
	}

	public void flag()
	{
<<<<<<< Updated upstream
		GetComponent<SpriteRenderer>().sprite = flagged ? PicAssets.instance.unfilledTexture : PicAssets.instance.flaggedTexture;
		flagged = !flagged;
		filled = !flagged;
=======
		GetComponent<SpriteRenderer>().sprite = flagged ? (filled ? PicAssets.instance.filledTexture : PicAssets.instance.unfilledTexture) : PicAssets.instance.flaggedTexture;
		flagged = !flagged;
		PicAssets.instance.rowObjects[ypos].GetComponent<Text>().color = PicAssets.instance.board.checkRow(ypos) == true ? Color.gray : Color.black;
		PicAssets.instance.columnObjects[xpos].GetComponent<Text>().color = PicAssets.instance.board.checkColumn(xpos) == true ? Color.gray : Color.black;
>>>>>>> Stashed changes
	}

	//	void Start()
	void Awake()
<<<<<<< Updated upstream
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
=======
//	public PicTile()
	{
		empty = Random.value < PicAssets.instance.emptyFrequency;	//Each tile is initialized with a 15% chance of being empty.

		xpos = (int)transform.position.x;							//Storing the game world coordinates of the tile in the class variables.
		ypos = (int)transform.position.y;							//These will be used to identify the tile.

		w = PicAssets.instance.w;
		h = PicAssets.instance.h;
//		PicAssets.instance.tiles[xpos, ypos] = this;                //Making the game board reference this object, using the coordinates to identify it.
//		PicAssets.instance.tiles[xpos].Add(this);                //Making the game board reference this object, using the coordinates to identify it.
		PicAssets.instance.tiles[xpos][ypos] = this;                //Making the game board reference this object, using the coordinates to identify it.
//		Debug.Log("" + (PicAssets.instance.tiles[xpos][ypos].empty == empty) + "	" + empty);

		board = PicAssets.instance.board;
//		if(empty) { GetComponent<SpriteRenderer>().sprite = PicAssets.instance.filledTexture; filled = true; }///
		}
>>>>>>> Stashed changes

	void OnMouseOver()
	{
		if (PicAssets.instance.gameStart) return;
		if (PicAssets.instance.gameWon) return; //If the game is over, return because we shouldn't allow any tiles to be flagged or filled.

<<<<<<< Updated upstream
		if (Input.GetMouseButtonDown(1) || (Input.touchCount > 0 ? Input.GetTouch(0).deltaTime >= 0.8f : false))    //If we right clicked.
		{
			flag();                         //Flag the selected tile.
											///			PicBoard.checkCompletion();		//Then check if we flagged the last empty spot, and if we did, tell the user that they won.
=======
		if (Input.GetMouseButtonDown(1) || (Input.touchCount > 0 ? Input.GetTouch(0).deltaTime >= 0.8f : false))	//If we right clicked.
		{
			flag();                         //Flag the selected tile.
///			PicBoard.checkCompletion();		//Then check if we flagged the last empty spot, and if we did, tell the user that they won.
			PicAssets.instance.history.Add(new Vector3(0f,xpos,ypos));
			PicAssets.instance.rehistory.Clear();
>>>>>>> Stashed changes
			return;
		}

		if (Input.GetMouseButtonDown(0))        //If we left clicked or touched the screen
		{
			//MineAssets.instance.board.check(xpos, ypos);
			fill();
<<<<<<< Updated upstream
=======
			PicAssets.instance.history.Add(new Vector3(1f, xpos, ypos));
			PicAssets.instance.rehistory.Clear();
//			Debug.Log(PicAssets.instance.board.checkRow(ypos));
//			PicAssets.instance.board.checkRow(ypos) == true ? PicAssets.instance.rows[i]
>>>>>>> Stashed changes
		}

	}

}
