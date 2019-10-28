using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Snake snake;
    private Board board;
    
    void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        GameAssets.instance.cam.orthographicSize = GameAssets.instance.gridSize.y / 2;
        GameAssets.instance.cam.transform.position = new Vector3((GameAssets.instance.gridSize.x / 2) - 0.5f, (GameAssets.instance.gridSize.y / 2) - 0.5f, -10);
        board = new Board(GameAssets.instance.gridSize);

        snake.Setup(board);
        board.Setup(snake);
    }

}
