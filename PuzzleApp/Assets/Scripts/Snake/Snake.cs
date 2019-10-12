using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int gridPos;

    private float moveTimer;
    private float moveTimerBase;

    private enum cardinal{ LEFT, DOWN, RIGHT, UP};
    private int dir;
    private bool hasNotTurned;

    private Board board;

    private int bodyLength;
    private List<Vector2Int> bodyList;
    private List<Transform> bodyTransforms;


    public void Setup(Board b)
    {
        this.board = b;
    }

    void Awake()
    {
        gridPos = new Vector2Int(GameAssets.instance.gridSize.x / 2, GameAssets.instance.gridSize.y / 2);
        moveTimerBase = 0.5f;
        moveTimer = moveTimerBase;
        dir = (int)cardinal.DOWN;
        hasNotTurned = true;
        transform.position = new Vector3(gridPos.x +0.5f, gridPos.y + 0.5f, 10);
        bodyLength = 0;
        bodyList = new List<Vector2Int>();
        bodyTransforms = new List<Transform>();
    }

    void Update()
    {
        moveTimer += Time.deltaTime;
        if (moveTimer >= moveTimerBase)
        {
            hasNotTurned = true;
            board.SnakeMoved(gridPos);
            bodyList.Insert(0, gridPos);

            if (dir == (int)cardinal.LEFT || dir == (int)cardinal.RIGHT)
            {
                int dirOffset = dir - 1;
                gridPos.x += dirOffset;
            }
            else
            {
                int dirOffset = dir - 2;
                gridPos.y += dirOffset;
            }

            
            if (bodyList.Count > bodyLength) bodyList.RemoveAt(bodyList.Count - 1);

            transform.position = new Vector3(gridPos.x, gridPos.y, -8);
            int counter = 0;
            foreach (Transform t in bodyTransforms)
            {
                t.position = new Vector3(bodyList[counter].x, bodyList[counter].y, -8);
                counter++;
            }
            CheckBodyCol();
            
            moveTimer -= moveTimerBase;
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
    }

    public void TurnLeft() 
    {
        if (hasNotTurned)
        {
            dir--;
            if (dir < 0) dir = 3;
            transform.Rotate(Vector3.back * 90);
            hasNotTurned = false;
        }
    }

    public void TurnRight()
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
        //bodyPart.transform.position = this.transform.position;
    }

    public List<Vector2Int> GetBodyList()
    {
        List<Vector2Int> snakeList = new List<Vector2Int>() { gridPos };
        if (bodyLength > 0) { snakeList.AddRange(bodyList); }
        return snakeList;
    }
}
