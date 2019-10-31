//Snake Game - Marc Herdman CSCI 150
//Class to build and manage the game board
//This script is not intended to be assigned to an object

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Board
{
    private Vector2Int applePos;    //To hole the current position of the apple
    private GameObject appleGO;     //To hold a reference to the apple game object
    private Vector2Int gridSize;    //To hold the x,y number of blocks that make the grid
    private Snake snake;            //To hold a reference to the snake class

    public Board(Vector2Int gs)     //Constructor
    {
        gridSize = gs;              //Set the grid size

        //Build the grid
        for (int y = 0; y < gridSize.y; ++y)
        {
            for (int x = 0; x < gridSize.x; ++x)
            {
                GameObject tile = new GameObject("tile", typeof(SpriteRenderer));
                tile.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.tileSprite;
                tile.transform.position = new Vector3(x, y, 0);
            }
        }

        //Create and hide the apple game bbject
        appleGO = new GameObject("Food", typeof(SpriteRenderer));
        appleGO.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.appleSprite;
        HideApple();

    }

    //Called by the snake class to pass a reference of itself to this class
    public void Setup(Snake s)  
    {
        this.snake = s;
    }

    //Move the apple off screen during initial construction and game over screen
    public void HideApple()
    {
        appleGO.transform.position = new Vector3(-500, -500, 0);
    }

    //Select a random location, not occupied by the snake, and move the apple object ot it
    public void SpawnApple()
    {
        do
        {
            applePos = new Vector2Int(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y));
        } while (snake.GetBodyList().IndexOf(applePos) != -1);
        appleGO.transform.position = new Vector3(applePos.x, applePos.y,-6);
        
    }

    //Called by the snake after it has moved
    //Checks for collision with apple or wall
    public void SnakeMoved(Vector2Int movedTo)
    {
        bool wallCol = GameAssets.instance.wallCol;     //Get wether wall collision is activated
        
        //Check if snake has gone out of bounds.
        if(movedTo.x < 0 || movedTo.x > gridSize.x -1 || movedTo.y < 0 || movedTo.y > gridSize.y - 1)
        {
            if(wallCol) //If yes and wall collision is on, game over
            {
                snake.Die();
            }
            else       //If yes and wallcollision is off, tell snake to wrap.
            {
                snake.WrapSnake(gridSize);
            }
        }
        //If not off the board check if on the apple
        else if (movedTo == applePos)
        {
            SpawnApple();   //If yes, respawn the apple in a new location
            snake.Grow();   //and tell the snake to grow
        }
    }
}
