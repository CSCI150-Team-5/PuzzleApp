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
            if(bodyList.Count > bodyLength)
            {
                //bodyList.RemoveAt(bodyLength - 1);
            }

            transform.position = new Vector3(gridPos.x, gridPos.y, 10);
            board.SnakeMoved(gridPos);
            moveTimer -= moveTimerBase;
        }
    }

    public void TurnLeft()
    {
        dir--;
        if(dir < 0) dir = 3;
        transform.Rotate(Vector3.back*90);
    }

    public void TurnRight()
    {
        dir++;
        if (dir > 3) dir = 0;
        transform.Rotate(Vector3.forward*90);
    }

    public Vector2Int getPos()
    {
        return gridPos;
    }

    public void Grow()
    {
        Debug.Log("GROW");
        bodyLength++;
        GameObject bodyPart = new GameObject("part", typeof(SpriteRenderer));
        bodyPart.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakebodySprite;
        bodyTransforms.Add(bodyPart.transform);
    }

    public List<Vector2Int> GetBodyList()
    {
        List<Vector2Int> snakeList = new List<Vector2Int>() { gridPos };
        if (bodyLength > 0) { snakeList.AddRange(bodyList); }
        return snakeList;
    }
}
