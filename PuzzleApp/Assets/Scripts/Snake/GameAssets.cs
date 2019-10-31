//Snake Game - Marc Herdman CSCI 150
//Class to hold elements that are available to all other scripts
//Place this script on an empty game object in the world
//Set this script to run before default time in Edit\Project Settings\Script Order

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets instance;  //Reference to this class

    public Camera cam;                  //Reference to the Main Camera
    public Canvas gameOver;             //Reference to the Game Over Canvas
    public Sprite tileSprite;           //Reference to the image to build the board with
    public Sprite snakeheadSprite;      //Reference to the image of the snake head
    public Sprite snakebodySprite;      //Reference to the image of the snake body
    public Sprite appleSprite;          //Reference to the image of the apple
    public Vector2Int gridSize;         //Reference to the x,y size the grid will be
    public bool wallCol;                //Reference to the wall collision setting

    private void Awake()
    {
        instance = this;
    }
}
