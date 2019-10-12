using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    private Vector2Int applePos;
    private GameObject appleGO;
    private Vector2Int gridSize;
    private Snake snake;

    public Board(Vector2Int gs)
    {
        gridSize = gs;
        CreateGrid();
        
    }

    public void Setup(Snake s)
    {
        this.snake = s;
        //SpawnApple();
    }

    private void CreateGrid()
    {
        for (int y = 0; y < gridSize.y; ++y)
        {
            for (int x = 0; x < gridSize.x; ++x)
            {
                GameObject tile = new GameObject("tile", typeof(SpriteRenderer));
                tile.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.tileSprite;
                tile.transform.position = new Vector3(x, y, 0);
            }
        }
        appleGO = new GameObject("Food", typeof(SpriteRenderer));
        appleGO.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.appleSprite;
        HideApple();
    }

    public void HideApple()
    {
        appleGO.transform.position = new Vector3(-500, -500, 0);
    }

    public void SpawnApple()
    {
        do
        {
            applePos = new Vector2Int(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y));
        } while (snake.GetBodyList().IndexOf(applePos) != -1);
        appleGO.transform.position = new Vector3(applePos.x, applePos.y,-6);
        
    }

    public void SnakeMoved(Vector2Int movedTo)
    {
        bool wallCol = GameAssets.instance.wallCol;
        if(movedTo == applePos)
        {
            SpawnApple();
            snake.Grow(); 
        }
        if(movedTo.x < 0 || movedTo.x > gridSize.x -1 || movedTo.y < 0 || movedTo.y > gridSize.y - 1)
        {
            if(wallCol)
            {
                snake.Die();
            }
            else
            {
                snake.WrapSnake(gridSize);
            }
        }
    }
}
