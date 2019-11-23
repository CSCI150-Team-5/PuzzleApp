//Snake Game - Marc Herdman CSCI 150
//Class controll the snake game object
//Place this script on an Sprite Game Object in world with the snake head image 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int gridPos;                     //Hold the current x,y position of the snake head

    private float timeSinceLastMove;                //Time since last movement
    private float timeBetweenMoves;                 //Time to wait between movements
    private float speedUpAmount;                    //Amount to decrement timeBetweenMoves each time snake grows

    private enum cardinal{ LEFT, DOWN, RIGHT, UP};  //The four direction the snake can move 0-3
    private int dir;                                //The current direction the snake is facing
    private bool hasNotTurned;                      //If the snake has turned since the last time it moved

    private Board board;                            //Reference to the board class

    private int bodyLength;                         //Current length of the snakes body, not including the head
    private List<Vector2Int> bodyList;              //x,y coords of each piece of the snake's body
    private List<Transform> bodyTransforms;         //GameObject Transforms of each boady piece


    //Called by Board Class to pass a reference of itself to the snake
    public void Setup(Board b)
    {
        this.board = b;
    }

    void Awake()
    {
        //Initialize the snake's body to be empty                             
        bodyList = new List<Vector2Int>();
        bodyTransforms = new List<Transform>();
        Init();
    }

    //Initializations that are done each time the game restarts
    private void Init()
    {
        gridPos = new Vector2Int(GameAssets.instance.gridSize.x / 2, GameAssets.instance.gridSize.y / 2); //Put snake head at center
        
        //Set initial values for 
        speedUpAmount = 0.025f;                 //SHOULD BE SET BY A SLIDER IN THE MENU
        timeBetweenMoves = 0.4f;       
        timeSinceLastMove = timeBetweenMoves;   //To put the snake in motion right away
        dir = (int)cardinal.DOWN;
        hasNotTurned = true;
        transform.position = new Vector3(gridPos.x + 0.5f, gridPos.y + 0.5f, 10);
        transform.localRotation = Quaternion.identity;
       
        //Destroy any former body pieces
        foreach (Transform t in bodyTransforms)
        {
            Destroy(t.gameObject);
        }
        bodyLength = 0;
        bodyList.Clear();
        bodyTransforms.Clear();
        
        //Tell board to reset the apple
        board.SpawnApple();
    }

    void Update()
    {
        timeSinceLastMove += Time.deltaTime;        //Add delta time to the time since last movement
        if (timeSinceLastMove >= timeBetweenMoves)  //If enough time has passed...
        {
            hasNotTurned = true;                    //Allow turning again
            bodyList.Insert(0, gridPos);            //Grab this position temporalily in case it is needed

            //Update the gridPos based on direction currently facing
            if (dir == (int)cardinal.LEFT || dir == (int)cardinal.RIGHT)
            {
                //Left is 0 and Right is 2 so -1 makes either -1 or 1
                int dirOffset = dir - 1;
                gridPos.x += dirOffset;
            }
            else
            {
                //Down is 1 and Up is 3 so -2 makes either -1 or 1
                int dirOffset = dir - 2;
                gridPos.y += dirOffset;
            }



            //Move snake head then all body parts
            transform.position = new Vector3(gridPos.x, gridPos.y, -8);
            int counter = 0;
            foreach (Transform t in bodyTransforms)
            {
                t.position = new Vector3(bodyList[counter].x, bodyList[counter].y, -8);
                counter++;
            }

            //Inform game board of the move
            board.SnakeMoved(gridPos);

            //If didn't grow, throw away the stored position grabbed earlier
            if (bodyList.Count > bodyLength) bodyList.RemoveAt(bodyList.Count - 1);

           
            CheckBodyCol();
            
            timeSinceLastMove -= timeBetweenMoves; 
        }
    }

    private void CheckBodyCol()
    {
        foreach (Transform t in bodyTransforms)
        {
            if(gridPos.x == t.position.x && gridPos.y == t.position.y)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        Debug.Log("GAME OVER");
        GameAssets.instance.gameOver.enabled = true;
        timeBetweenMoves = Mathf.Infinity;
    }

    public void Restart()
    {
        GameAssets.instance.gameOver.enabled = false;
        Init();
    }

    public void TurnRight() 
    {
        if (hasNotTurned)
        {
            dir--;
            if (dir < 0) dir = 3;
            transform.Rotate(Vector3.back * 90);
            hasNotTurned = false;
        }
    }

    public void TurnLeft()
    {
        if (hasNotTurned)
        {
            dir++;
            if (dir > 3) dir = 0;
            transform.Rotate(Vector3.forward * 90);
            hasNotTurned = false;
        }
    }

    public Vector2Int getPos()
    {
        return gridPos;
    }

    public void WrapSnake(Vector2Int gridSize)
    {
        //THIS NEED TO RUNN FOR EACH BODY PART INDIVIDUALLY
        //THAT MEANS TAKING WALL COLLISION COMPLETELY OUT OF BOARD
        //HANDLE IT ENTIRELY WITHIN THE SNAKE
        Debug.Log("WRAP");
        Vector2Int offset = new Vector2Int(0,0);
        if(gridPos.x < 0)
        {
            offset.x = gridSize.x; 
        }
        else if(gridPos.x > gridSize.x -1)
        {
            offset.x = gridSize.x * -1;
        }
        else if(gridPos.y < 0)
        {
            offset.y = gridSize.y;
        }
        else
        {
            offset.y = gridSize.y * -1;
        }

        gridPos.x += offset.x;
        gridPos.y += offset.y;
        for (int i = 0; i < bodyList.Count; ++i)
        {
            Vector2Int v = bodyList[i];
            v.x += gridSize.x;
            v.y += gridSize.y;
            bodyList[i] = v;
        }
    }

    public void Grow()
    {
        Debug.Log("GROW");
        bodyLength++;
        GameObject bodyPart = new GameObject("renderer", typeof(SpriteRenderer));
        bodyPart.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakebodySprite;
        bodyTransforms.Add(bodyPart.transform);
        timeBetweenMoves -= speedUpAmount;
    }

    //THIS FUNCTION WILL NOT BE NECESSARY WHEN WALL COLISION IS HANDLED ENTIRELY IN SNAKE
    public List<Vector2Int> GetBodyList()
    {
        List<Vector2Int> snakeList = new List<Vector2Int>() { gridPos };
        if (bodyLength > 0) { snakeList.AddRange(bodyList); }
        return snakeList;
    }
}
