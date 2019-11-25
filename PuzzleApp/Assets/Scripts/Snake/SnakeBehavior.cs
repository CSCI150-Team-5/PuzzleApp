using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeBehavior : MonoBehaviour
{
    [SerializeField]
    private Image snakeHead;
    [SerializeField]
    private Image snakeBody;
    [SerializeField]
    private Image appleSprite;

    private int bodySize;
    private List<Vector2Int> bodyPositions = new List<Vector2Int>();
    
    Vector2 lowerLeft = new Vector2(-895, -400);
    Vector2 boxSize = new Vector2(51, 50);
    Vector2 gridSize = new Vector2(36, 17);

    Image player;
    Image apple;
    private List<Image> bodyParts = new List<Image>();

    private enum dir{LEFT,DOWN,RIGHT,UP};
    private Vector2Int[] movement = new Vector2Int[] {new Vector2Int(-1,0), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(0, 1) };

    private bool settingWrap = false;
    private float settingSpeed = 0.5f;
    private float speed;
    private float time;

    private bool pause = true;
    private bool hasMovedThisTurn = false;

    private Vector2Int curPos;
    private int curDir = (int)dir.DOWN;

    private Vector2Int applePos;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        CreateSnakeHead();
        CreateApple();
        Go();
    }

    // Update is called once per frame
    void Update()
    {
        if(!pause)
        {
            time += Time.deltaTime;
            if(time > speed)
            {
                time = 0;
                MoveSnake();
                CheckWallCol();
                CheckSnakeCol();
                CheckAppleCol();
            }
        }
    }

    private void Init()
    {
        pause = true;
        hasMovedThisTurn = false;
        speed = settingSpeed;
        time = 0;
        if (bodyParts.Count > 0) foreach (Image part in bodyParts) { Destroy(part); }
        bodyParts = new List<Image>();
        bodyPositions = new List<Vector2Int>();
        bodySize = 0;
    }

    private void Go()
    {
        pause = false;
    }

    private void CreateSnakeHead()
    {
        player = Instantiate(snakeHead, Vector3.zero, Quaternion.identity);
        player.rectTransform.SetParent(transform, false);
        player.transform.localScale = boxSize / 100f;//new Vector3(.51f, .5f, 1);
        curPos = new Vector2Int(Mathf.RoundToInt(gridSize.x / 2), Mathf.RoundToInt(gridSize.y / 2));
        player.rectTransform.anchoredPosition = lowerLeft + curPos * boxSize;
    }

    private void CreateApple()
    {
        apple = Instantiate(appleSprite, Vector3.zero, Quaternion.identity);
        apple.rectTransform.SetParent(transform, false);
        apple.transform.localScale = boxSize / 100f;//new Vector3(.51f, .5f, 1);
        PlaceApple();
    }

    public void TurnSnake(int d)
    {
        if (!pause && !hasMovedThisTurn)
        {
            hasMovedThisTurn = true;
            Vector3 turnType = Vector3.forward;
            if (d < 0) turnType = Vector3.back;
            Debug.Log("Before: " + curDir);
            curDir = (curDir + d) % 4;
            if (curDir < 0) curDir += 4;
            Debug.Log("After: " + curDir);
            player.transform.Rotate(turnType * 90);
            Debug.Log(curDir);
        }
    }

    private void MoveSnake()
    {
        bodyPositions.Add(curPos);
        if (bodyPositions.Count > bodySize) bodyPositions.RemoveAt(0);
        curPos += movement[curDir];
        hasMovedThisTurn = false;
        UpdateSnakeSprites();
    }

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

    private void CheckWallCol()
    {
        Vector2Int wrapOffset = new Vector2Int(0, 0);
        bool boundHit = false;
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
            Debug.Log("DID HIT");
            if(settingWrap)
            {
                Debug.Log("DID WRAP");
                curPos = wrapOffset;
                UpdateSnakeSprites();
            }
            else
            {
                GameOver();
            }
        }
    }

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

    void CheckAppleCol()
    {
        if(curPos == applePos)
        {
            Debug.Log("YUM!");
            bodySize++;
            Image newPart = Instantiate(snakeBody, new Vector3(10000,10000,0), Quaternion.identity);
            newPart.rectTransform.SetParent(transform, false);
            newPart.transform.localScale = boxSize / 100f;
            bodyParts.Add(newPart);
            PlaceApple();
        }
    }

    private void PlaceApple()
    {
        bool isValidLoc = true;
        do
        {
            applePos = new Vector2Int(Random.Range(0, (int)gridSize.x), Random.Range(0, (int)gridSize.y));
            if(applePos == curPos) isValidLoc = false;
            if(bodySize > 0)
            {
                foreach(Vector2Int bp in bodyPositions)
                {
                    if (applePos == bp) isValidLoc = false;
                }
            }
        } while (!isValidLoc);
        apple.rectTransform.anchoredPosition = lowerLeft + applePos * boxSize;
    }

    private void GameOver()
    {
        pause = true;
        Debug.Log("GAME OVER");
    }
}
