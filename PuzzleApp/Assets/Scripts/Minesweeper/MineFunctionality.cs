using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineFunctionality : MonoBehaviour
{
	//Assign these Game Objects manually in the editor if they are left unassigned.
	public GameObject[] menuObjects;    //0: Select Difficulty Label	1: 5x5 Difficulty Button	2: 10x10 Difficulty Button	3: 10x15 Difficulty Button	4: Select Mine Frequency Label	5: Mine Frequency Slider	6: Low Frequency Label	7: High Frequency Label	
	public GameObject[] gameUIObjects;  //0: Mines Left Label	1: Mines Left Count	2: Time left Label	3: Time Left Count	4: New Game Button	5: Return Button
	public Slider sliderMineFrequency;  //The Mine Frequency Slider, it's in menuObjects but I also have it individually, it needs to be assigned to both.
	public GameObject tooltipYouWon;    //The winner tooltip, it's in gameUIObjects but I also have it individually, it needs to be assigned to both.

	public void deactivateMenu(bool menuState)	//Depending on if we're loading the menu or game, it activates the menu objects or the UI objects.
	{
		for (int i = 0; i < menuObjects.Length; i++) menuObjects[i].SetActive(!menuState);
		for (int i = 0; i < gameUIObjects.Length; i++) gameUIObjects[i].SetActive(menuState);
	}


	public void generateBoard()	//The function for generating the board. We're assuming the width and height was already assigned to instance.
	{
		MineAssets.instance.board = new MineBoard(MineAssets.instance.w, MineAssets.instance.h);	//Assigning the board to instance.

		//The following is some cleanup to move the camera to the correct location and setting the scale. The camera scales with the width/height we chose.
		Screen.orientation = ScreenOrientation.Portrait;
		MineAssets.instance.cam.orthographicSize = MineAssets.instance.w;
		MineAssets.instance.cam.transform.position = new Vector3(((float)MineAssets.instance.w / 2.0f) - 0.5f, ((float)MineAssets.instance.h / 2.0f) - 0.5f, -10);

		//This section is for generating all the tiles themselves, graphically in the game world, and assigning them corresponding tile scripts as well.
		for (int i = 0; i < MineAssets.instance.w; i++)
		{
			for (int j = 0; j < MineAssets.instance.h; j++)
			{
				GameObject tile = new GameObject("Tile (" + i + "," + j + ")", typeof(SpriteRenderer));	//Name of the tile so it's easily identifiable. X comma y coordinates in the name.
				tile.GetComponent<SpriteRenderer>().sprite = MineAssets.instance.unfilledTexture;		//The default texture is unfilled.
				tile.transform.position = new Vector3(i, j);											//Setting its position relative to the world.
				tile.tag = "MineObj";																	//A tag I use for keeping track of the object.
				tile.AddComponent<BoxCollider2D>();														//Giving it collision.
				tile.AddComponent<MineTile>();															//Assigning it the Mine Tile script.
			}
		}

		tooltipYouWon.transform.SetSiblingIndex(1000);  //Ensuring that the tooltip is above everything else.
	}

	//The following functions are for the buttons, they are all identical other than the size that is assigned.
	public void select5x5()
	{
		deactivateMenu(true);		//Deactivate the menu objects and activate the UI objects.

		MineAssets.instance.w = 5;	//Assign the width and height to instance.
		MineAssets.instance.h = 5;	//These are used in the generateBoard function as well.

		generateBoard();			//Now generate the board.
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

	public void setMineFrequency()	//This funcction is invoked when you move the slider for mine frequency in the menu.
	{
		MineAssets.instance.mineFrequency = (float)sliderMineFrequency.value /100f;	//The value the slider is set at is turned into a percentage and stored in instance's mineFrequency.
	}

	public void newGame()	//This function deletes the previous instance of the game and creates a new one, resetting everything as needed.
	{
		foreach (GameObject tile in GameObject.FindGameObjectsWithTag("MineObj"))	//Grabs every single tile, I assigned all tiles the MineObj tag on creation so this is how we find them.
			Destroy(tile);  //Delete those game objects.

		tooltipYouWon.SetActive(false);         //Removing the you won tooltip since we're starting a new game.

		MineAssets.instance.firstClick = true;	//Reset all the booleans.
		MineAssets.instance.gameWon = false;	//
		MineAssets.instance.gameLost = false;	//

		MineAssets.instance.minesLeftText.text = "?";	//Reset all the UI values.
		MineAssets.instance.timerText.text = "?";		//
		MineAssets.instance.timerInt = 0;				//

		generateBoard();						//Generate a new game, using the same width/height.
	}

	public void returnToMainMenu()	//This function deletes the previous instance of the game and returns to the minesweeper menu.
	{
		foreach (GameObject tile in GameObject.FindGameObjectsWithTag("MineObj"))   //Grabs every single tile, I assigned all tiles the MineObj tag on creation so this is how we find them.
			Destroy(tile);  //Delete those game objects.

		tooltipYouWon.SetActive(false);         //Removing the you won tooltip since we're starting a new game.

		MineAssets.instance.firstClick = true;  //Reset all the booleans.
		MineAssets.instance.gameWon = false;	//
		MineAssets.instance.gameLost = false;	//

		MineAssets.instance.minesLeftText.text = "?";	//Reset all the UI values.
		MineAssets.instance.timerText.text = "?";		//
		MineAssets.instance.timerInt = 0;				//

		MineAssets.instance.board = null;		//Set the board to null, this lets the code know that the previous board is okay to delete since nothing points to it anymore.

		deactivateMenu(false);					//Turn all the UI objects off and turn back on the menu objects to return to the picross menu.
	}

	public void selectGameWonTooltip()  //When you click on the tooltip
	{
		tooltipYouWon.SetActive(false); //It should go away.
	}

	void Awake()
	{
		deactivateMenu(false);	//When the game starts, this forces the game to be in menu mode.
	}

	void Update()
	{
		if (MineAssets.instance.gameJustWon) //If we JUST won the game.
		{
			tooltipYouWon.SetActive(true);          //Turn on the tooltip.
			MineAssets.instance.gameJustWon = false;     //Then we are past the moment immediately after just winning.
			MineAssets.instance.gameWon = true;
		}
	}
}
