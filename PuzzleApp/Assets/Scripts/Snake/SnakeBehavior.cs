using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SnakeBehavior : MonoBehaviour
{
    //References to game assests for spawning
    [SerializeField]
    private Image square;
    [SerializeField]
    private Image snakeHead;
    [SerializeField]
    private Image snakeBody;
    [SerializeField]
    private Image appleSprite;
    [SerializeField]
    private Text appleText;
    [SerializeField]
    private GameObject gameoverPanel;
    [SerializeField]
    private Toggle wrapSwitch;
    [SerializeField]
    private Slider speedSlider;

    private int bodySize; //Length of body, not including head
    private List<Vector2Int> bodyPositions = new List<Vector2Int>(); //Locations of each body part
    
    Vector2 lowerLeft = new Vector2(-895, -400); //Where the lower left corner of the play area is
    Vector2 boxSize = new Vector2(51, 50); //Size of each grid square
    Vector2 gridSize = new Vector2(36, 17); //number of squares over all

    Image player; //Reference to the spawned snakehead
    Image apple; //Reference to the spawned apple
    private List<Image> bodyParts = new List<Image>(); //References to the spawned body parts

    //To convert simple cardninal directions to vector offsets
    private enum dir{LEFT,DOWN,RIGHT,UP};
    private Vector2Int[] movement = new Vector2Int[] {new Vector2Int(-1,0), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(0, 1) };

    //User setting flags
    private bool settingWrap = false; //True, rap around edges : False, killed by edges
    private float settingSpeed = 0.5f; //Speed snake starts at (time to elapse between updates, smaller = faster) 
    
    private float speed; //Amount of time to make pass before moving snake (smaller = faster)
    private float timeSinceLastMove; //time since last snake movement

    private bool pause = true; //Is game paused
    private bool hasMovedThisTurn = false; //Has the user already made a rotation choice since the last time the snake moved

    private Vector2Int curPos; //Snake's curent location
    private int curDir = (int)dir.DOWN; //Direction snake is currently pointing

    private Vector2Int applePos; //Apples current location

    // Awake is called when the scene becomes active
    void Awake()
    {
        pause = true;
        SetScreenOrientaion();
    }

    // Update is called once per frame
    void Update()
    {
        if(!pause)
        {
            timeSinceLastMove += Time.deltaTime; //add time since last update call to the time since last move...
            if(timeSinceLastMove > speed) //...if that is larger than current speed...
            {
                //... allow snake to move, check various collisions and reset time
                MoveSnake();
                CheckWallCol();
                CheckSnakeCol();
                CheckAppleCol();
                timeSinceLastMove = 0;
            }
        }
    }

    //Initialize a new game
    public void Init()
    {
        hasMovedThisTurn = false; //Reset flag
        GetOrCreateSettings(); //Get the users preferences
        speed = settingSpeed; //Set speed to user's prefered start speed
        timeSinceLastMove = 0; //Reset time
        
        //Set snake position to middle of screen facing down
        curPos = new Vector2Int(Mathf.RoundToInt(gridSize.x / 2), Mathf.RoundToInt(gridSize.y / 2)); 
        curDir = (int)dir.DOWN;
        player.transform.rotation = Quaternion.Euler(0,0,0);
        
        //Delete any existing body parts and empty lists
        if (bodyParts.Count > 0) foreach (Image part in bodyParts) { Destroy(part); }
        bodyParts = new List<Image>();
        bodyPositions = new List<Vector2Int>();
        bodySize = 0;

        appleText.text = bodySize.ToString(); //Set apple display to 0
        gameoverPanel.transform.position = new Vector3(10000, 10000, 0); //Move game over panel off screen 
    }

    //Look for previous user preference settings or create the if they don't exist
    //PlayerPrefs does not support booleans so true is converted to 1 and false to 0 
    private void GetOrCreateSettings()
    {
        if(PlayerPrefs.HasKey("Snake_Wrap"))
        {
            int setting = PlayerPrefs.GetInt("Snake_Wrap");
            settingWrap = (setting == 0) ? false : true; 
        }
        else
        {
            settingWrap = false;
            PlayerPrefs.SetInt("Snake_Wrap", 0);
        }
        if(PlayerPrefs.HasKey("Snake_Speed"))
        {
            settingSpeed = PlayerPrefs.GetFloat("Snake_Speed");
        }
        else
        {
            settingSpeed = 0.5f;
            PlayerPrefs.SetFloat("Snake_Speed", settingSpeed);
        }
        PlayerPrefs.Save(); //Save the prefs
        
        //Make sure in game toggles represent the current settings
        wrapSwitch.isOn = settingWrap;
        speedSlider.value = settingSpeed;
    }

    //Handle the Go button being clicked
    public void Go()
    {
        CreateGrid(); //Generate the background grid
        CreateSnakeHead(); //Generate the snake head asset
        CreateApple(); //Generate the apple asset
        Init(); //Run new game initialization
        pause = false; //unpause the game
    }

    //Pause the game
    public void PauseGame()
    {
        pause = true;
    }

    //Unpause the game. Must reset the game
    public void UnpauseGame()
    {
        pause = false;
        Init();
    }

    //Instantiate the snake head asset and place it in middle of the screen
    private void CreateSnakeHead()
    {
        player = Instantiate(snakeHead, Vector3.zero, Quaternion.identity);
        player.rectTransform.SetParent(transform, false);
        player.transform.localScale = boxSize / 100f;//new Vector3(.51f, .5f, 1);
        curPos = new Vector2Int(Mathf.RoundToInt(gridSize.x / 2), Mathf.RoundToInt(gridSize.y / 2));
        player.rectTransform.anchoredPosition = lowerLeft + curPos * boxSize;
    }

    //Instantiate the apple and randomly place it
    private void CreateApple()
    {
        apple = Instantiate(appleSprite, Vector3.zero, Quaternion.identity);
        apple.rectTransform.SetParent(transform, false);
        apple.transform.localScale = boxSize / 100f;//new Vector3(.51f, .5f, 1);
        PlaceApple();
    }

    //Recieve a 1 or -1 and urn player clockwise (1) or counter-clockwise (-1) 
    public void TurnSnake(int d)
    {
        //Only accept new inputs if unpaused and has not accepted new input this turn
        if (!pause && !hasMovedThisTurn) 
        {
            hasMovedThisTurn = true; //Reset has moved flag
            
            //Decide clockwise or counterclockwise
            Vector3 turnType = Vector3.forward; 
            if (d < 0) turnType = Vector3.back;

            //Adjust direction integer based on input, keep it between 0 and 3
            curDir = (curDir + d) % 4;
            if (curDir < 0) curDir += 4;

            //Rotate the player 90* either clockwise or counter-clockwise
            player.transform.Rotate(turnType * 90);
        }
    }

    //Move snakehead asset based on direction player is currently facing
    private void MoveSnake()
    {
        bodyPositions.Add(curPos); //Add the current position to the body list
        if (bodyPositions.Count > bodySize) bodyPositions.RemoveAt(0); //Remove the oldest position unless body size has grown
        curPos += movement[curDir]; //Add the proper movement offset to the current position 
        hasMovedThisTurn = false; //Reset has moved flag
        UpdateSnakeSprites(); //Update the position of the snake assets
    }

    //Update the potion of the snake head and all body parts
    private void UpdateSnakeSprites()
    {
        player.rectTransform.anchoredPosition = lowerLeft + curPos * boxSize;
        if(bodySize > 0)
        {
            for(int i = 0; i < bodySize; i++)
            {
                bodyParts[i].rectTransform.anchoredPosition = lowerLeft + bodyPositions[i] * boxSize;
            }
        }
    }

    //Check if player went out of bounds
    private void CheckWallCol()
    {

        Vector2Int wrapOffset = new Vector2Int(0, 0); //For deciding where player ends up if wrap is on
        bool boundHit = false; //Flag if out of bounds found

        //If out of bounds in any direction set bound hit and the proper offset
        if (curPos.x < 0)
        {
            boundHit = true;
            wrapOffset = new Vector2Int((int)gridSize.x-1, curPos.y);
        }
        else if (curPos.x > gridSize.x - 1)
        {
            boundHit = true;
            wrapOffset = new Vector2Int(0, curPos.y);
        }
        else if(curPos.y < 0)
        {
            boundHit = true;
            wrapOffset = new Vector2Int(curPos.x, (int)gridSize.y-1);
        }
        else if (curPos.y > gridSize.y - 1)
        {
            boundHit = true;
            wrapOffset = new Vector2Int(curPos.x, 0);
        }

        if(boundHit)
        {
            if(settingWrap)
            {
                //Move the player if bound was hit and wrap was on
                curPos = wrapOffset;
                UpdateSnakeSprites();
            }
            else
            {
                //End game if bound was hit and wrap was off
                GameOver();
            }
        }
    }

    //Check if the snake head is on the same square as any body part
    void CheckSnakeCol()
    {
        if(bodySize > 0)
        {
            foreach(Vector2Int pos in bodyPositions)
            {
                if(pos == curPos)
                {
                    GameOver();
                }
            }
        }
    }

    //Check if the snake head is on the same square as the apple
    //If so grow snake, increase speed, and place new apple
    void CheckAppleCol()
    {
        if(curPos == applePos)
        {
            bodySize++;
            speed = Mathf.Clamp(speed - 0.02f, 0.02f, 1f);
            appleText.text = bodySize.ToString();
            Image newPart = Instantiate(snakeBody, new Vector3(10000,10000,0), Quaternion.identity);
            newPart.rectTransform.SetParent(transform, false);
            newPart.transform.localScale = boxSize / 100f;
            bodyParts.Add(newPart);
            PlaceApple();
        }
    }

    //Randomly place apple making sure it is not on any part of the snake
    private void PlaceApple()
    {
        bool isValidLoc = true;
        do
        {
            applePos = new Vector2Int(Random.Range(0, (int)gridSize.x), Random.Range(0, (int)gridSize.y)); //Pick random location
            if (applePos == curPos) isValidLoc = false; //If on the head, set not valid
            if(bodySize > 0)
            {
                foreach(Vector2Int bp in bodyPositions)
                {
                    if (applePos == bp) isValidLoc = false; //If on any part of the body, set not valid
                }
            }
        } while (!isValidLoc);
        apple.rectTransform.anchoredPosition = lowerLeft + applePos * boxSize; //Place the apple asset
    }

    //Instantiate squares for background grid
    private void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Image s = Instantiate(square, Vector3.zero, Quaternion.identity);
                s.rectTransform.SetParent(transform, false);
                s.transform.localScale = boxSize / 100;//new Vector3(.51f, .5f, 1);
                s.rectTransform.anchoredPosition = lowerLeft + boxSize * new Vector2(x, y);// new Vector2(x * 51, y * 50);
            }
        }
    }

    //Allow game to be displayed in either horizontal direction but not either portrait direction
    private void SetScreenOrientaion()
    {
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    //When game is over, pause and bring up game over panel
    private void GameOver()
    {
        pause = true;
        gameoverPanel.transform.localPosition = Vector3.zero;
    }

    //Handle inputs from the Player Wrap toggle asset
    public void SetWrap(bool b)
    {
        settingWrap = b; //Change setting
        int i = b ? 1 : 0; //Convert boolean to int
        PlayerPrefs.SetInt("Snake_Wrap", i); //Store the new setting
        PlayerPrefs.Save(); //Save settings
    }
    
    //Return the wrap setting
    public bool GetWarp()
    {
        return settingWrap;
    }

    //Handle input from the speed slider asset
    public void SetBaseSpeed(float s)
    {
        settingSpeed = s; //Set the speed setting
        PlayerPrefs.SetFloat("Snake_Speed", settingSpeed); //Store the new setting
        PlayerPrefs.Save(); //Save settings
    }

    //Return the speed setting
    public float GetBaseSpeed()
    {
        return settingSpeed;
    }

    //Handle the main menu buttons
    public void exitGame()
    {
        SceneManager.LoadScene(0);
    }
}
