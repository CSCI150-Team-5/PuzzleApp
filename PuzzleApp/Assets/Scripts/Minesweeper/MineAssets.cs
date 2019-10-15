using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineAssets : MonoBehaviour
{
 	public static MineAssets instance;

	public Sprite unfilledTexture;					//A bunch of variables for storing textures to be used by the mines.
	public Sprite mineTexture;						//We get the values externally through unity.
	public Sprite firstMineTexture;					//
	public Sprite flaggedTexture;					//
	public Sprite flaggedMineTexture;				//
	public Sprite filledTexture0;					//
	public Sprite filledTexture1;					//
	public Sprite filledTexture2;					//
	public Sprite filledTexture3;					//
	public Sprite filledTexture4;					//
	public Sprite filledTexture5;					//
	public Sprite filledTexture6;					//
	public Sprite filledTexture7;					//
	public Sprite filledTexture8;					//
	public Sprite[] filledTexture = new Sprite[9];  //

	public bool firstClick = true;	//A bool that's true by default, indicating we haven't filled a tile yet. After we fill a tile, this is set to false. We use this to ensure the first click is always safe and not a mine.
	public bool gameWon = false;	//A bool we use to represent if the game is won. If it is set to true, then we cannot fill or flag any more tiles.
	public bool gameLost = false;	//A bool we use to represent if the game is lost. If it is set to true, then we cannot fill or flag any more tiles.

	public bool bulkMode = true;//false;

	public int w;					//The width and height of the board.
	public int h;					//
	public MineTile[,] tiles;       //A container of all the minesweeper tiles, their constructors place themselves in here. They are identified by their coordinates in the world.

	public double mineFrequency = 0.15;

	public MineBoard board;

	void Awake()
    {
		filledTexture[0] = filledTexture0;
		filledTexture[1] = filledTexture1;
		filledTexture[2] = filledTexture2;
		filledTexture[3] = filledTexture3;
		filledTexture[4] = filledTexture4;
		filledTexture[5] = filledTexture5;
		filledTexture[6] = filledTexture6;
		filledTexture[7] = filledTexture7;
		filledTexture[8] = filledTexture8;	

		instance = this;
    }
}
