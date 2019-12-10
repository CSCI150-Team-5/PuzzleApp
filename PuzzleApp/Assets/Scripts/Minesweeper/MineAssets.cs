using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineAssets : MonoBehaviour //This class contains all the global variables we need for this game.
{
 	public static MineAssets instance;	//The static instance of MineAssets. There can only be one of these, this makes coding easy but not having to have everything else be static. Just use instance's variables and properties.

	public Camera cam;	//The variable storing the UI component camera, we move the camera in the UI by editing this value.

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

	public bool firstClick = true;		//A bool that's true by default, indicating we haven't filled a tile yet. After we fill a tile, this is set to false. We use this to ensure the first click is always safe and not a mine.
	public bool gameWon = false;		//A bool we use to represent if the game is won. If it is set to true, then we cannot fill or flag any more tiles.
	public bool gameJustWon = false;	//A bool we use to represent if a won game is being processed.
	public bool gameLost = false;		//A bool we use to represent if the game is lost. If it is set to true, then we cannot fill or flag any more tiles.

	public bool bulkMode = true;	//A bool for turning on/off bulk mode controls. We actually aren't supporting turning them off yet, so this is always true.

	public MineTile[,] tiles;			//A container of all the minesweeper tiles, their constructors place themselves in here. They are identified by their coordinates in the world.
	public MineBoard board;				//The instance's copy of the board.
	public GameObject[,] tileSprites;	//A contained of all the minesweeper tile sprites, these graphics are kept track of and deleted independently of the class objects.

	public double mineFrequency = 0.15;	//The likelihood that a tile will be a mine. It is 0.15 by default, and permitted values range from 0.05 to 0.25. The user only sees a slider, never the exact value of this variable.

	public int w;	//The width of the board. It is assigned too in the button functions in MineFunctionality.
	public int h;   //The height of hte board. It is assigned too in the button functions in MineFunctionality.

	public Text minesLeftText;	//The value of the UI element that stores the amount of unflagged mines left.
	public int minesTotal = 0;	//The integer value that stores the amount of unflagged mines left. Note that the value changes when you flag, whether or not the tile was a mine. It lets you fumble.

	public Text timerText;		//The value of the UI element that stores the amount of time that has passed since the game began.
	public float timerInt = 0;	//The integer value that stores the amount of time that has passed since the game began.

	void Awake()	//This runs when this class is first awoken.
    {
		filledTexture[0] = filledTexture0;	//Assigning the texture to an array of textures just for ease of programming.
		filledTexture[1] = filledTexture1;	//Assigning the texture to an array of textures just for ease of programming.
		filledTexture[2] = filledTexture2;	//Assigning the texture to an array of textures just for ease of programming.
		filledTexture[3] = filledTexture3;	//Assigning the texture to an array of textures just for ease of programming.
		filledTexture[4] = filledTexture4;	//Assigning the texture to an array of textures just for ease of programming.
		filledTexture[5] = filledTexture5;	//Assigning the texture to an array of textures just for ease of programming.
		filledTexture[6] = filledTexture6;	//Assigning the texture to an array of textures just for ease of programming.
		filledTexture[7] = filledTexture7;	//Assigning the texture to an array of textures just for ease of programming.
		filledTexture[8] = filledTexture8;	//Assigning the texture to an array of textures just for ease of programming.

		Time.timeScale = 1.0f;	//Ensuring that the time scale is set correctly, will be used later incrementing the timer.

		instance = this;		//Assigning this to the static instance of this class, so we can always grab it.
	}

	void Update()
	{
		if (MineAssets.instance.firstClick == false)	//If first click is false, meaning we have already done the first click.
		{
			if ((MineAssets.instance.gameLost == false) && (MineAssets.instance.gameWon == false))	//If the game is not yet won or lost
			{
				timerInt += Time.deltaTime;				//Increment the time, it only adds the difference in time, scaling with Time, so it should match up with seconds, not frames.
				timerText.text = "" + (int)timerInt;	//Casting the time to an integer, and then casting it to a string and storing it in the UI component's variable.
			}
//			else if (MineAssets.instance.gameJustWon == true)	//If we JUST won the game.
//			{
//				tooltipYouWon.SetActive(true);      //Turn on the tooltip.
//				PicAssets.instance.gameJustWon = false; //Then we are past the moment immediately after just winning.
//				PicAssets.instance.gameWon = true; //Then we are past the moment immediately after just winning.
//			}
		}
	}
}
