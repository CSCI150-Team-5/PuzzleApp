using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicAssets : MonoBehaviour	//This class contains all the global variables we need for this game.
{
	public static PicAssets instance;   //The static instance of PicAssets. There can only be one of these, this makes coding easy but not having to have everything else be static. Just use instance's variables and properties.

	public Camera cam;  //The variable storing the UI component camera, we move the camera in the UI by editing this value.
	public Canvas can;  //The variable storing the UI component canvas, we move the canvas in the UI by editing this value.

	public Sprite unfilledTexture;		//Variables for storing textures to be used by the tiles.
	public Sprite filledTexture;		//
	public Sprite flaggedTexture;		//
	public Sprite transparentTexture;	//

	public bool gameStart = false;		//Variables associated with if we have started the game, and if we have, if we won or not yet.
	public bool gameWon = false;		//
	public bool afterGame = false;      //
	public bool stopTimer = true;		//

	public bool fillMode = true;		//For touch cocntrols, selecting if we want to fill a tile we touch or flag a tile we touch.

	public List<List<PicTile>> tiles = new List<List<PicTile>>();				//A container of all the picross tiles, their constructors place themselves in here. They are identified by their coordinates in the world.
	public List<List<GameObject>> tileObjects = new List<List<GameObject>>();   //A contained of all the picross tile sprites, these graphics are kept track of and deleted independently of the class objects.
	public PicBoard board;                                                      //The instance's copy of the board.
	public List<List<int>> rows = new List<List<int>>();						//The rows of numbers that appear left of the board.
	public List<List<int>> columns = new List<List<int>>();						//The columns of numbers that appear above the board.
	public List<GameObject> rowObjects = new List<GameObject>();				//The actual row objects.
	public List<GameObject> columnObjects = new List<GameObject>();				//The column objects.
	public List<GameObject> lines = new List<GameObject>();						//The bold lines dividing the tiles into 5x5 squares, for game modes greater than 5x5.

	public List<List<Vector3>> history = new List<List<Vector3>>();				//The first integer represents the type of move we made, like flagging or filling. The second and third integer represents the x and y coordinate respectively.
	public List<List<Vector3>> rehistory = new List<List<Vector3>>();			//The first integer represents the type of move we made, like flagging or filling. The second and third integer represents the x and y coordinate respectively.

	public double emptyFrequency = 0.625;   //The likelihood that a tile should be left empty. It is 0.625 by default, and permitted values range from 0.50 to 0.75. The user only sees a slider, never the exact value of this variable.

	public int w;   //The width of the board. It is assigned too in the button functions in PicFunctionality.
	public int h;   //The height of hte board. It is assigned too in the button functions in PicFunctionality.

	public Text timerText;      //The value of the UI element that stores the amount of time that has passed since the game began.
	public float timerInt = 0;  //The integer value that stores the amount of time that has passed since the game began.


	void Awake()
	{
		Time.timeScale = 1.0f;	//Ensuring that the time scale is set correctly, will be used later incrementing the timer.

		instance = this;		//Assigning this to the static instance of this class, so we can always grab it.
	}

	void Update()
	{
		if (PicAssets.instance.stopTimer == false)	//If the timer is allowed to go.
		{
			timerInt += Time.deltaTime;				//Increment the time, it only adds the difference in time, scaling with Time, so it should match up with seconds, not frames.
			timerText.text = "" + (int)timerInt;	//Casting the time to an integer, and then casting it to a string and storing it in the UI component's variable.
		}
	}
}
