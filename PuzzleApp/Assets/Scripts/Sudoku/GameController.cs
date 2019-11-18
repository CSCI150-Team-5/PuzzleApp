using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private int[,] gameBoard = new int[9, 9];
    private bool[,,] playerBoard = new bool[9, 9, 10];

    [SerializeField]
    private Button gameSquare;
    private Button[,] gameSquares = new Button[9, 9];
    [SerializeField]
    private Button selectionSquare;
    private Button[] selectionSquares = new Button[9];

    [SerializeField]
    private GameObject winnerPanel;
    [SerializeField]
    private GameObject confirmPanel;


    private int settingDiff = 50;

    private int curSelection;

    private bool paused = false;

    private string confirmPanelCaller = "";
    // Start is called before the first frame update
    void Awake()
    {
        Random.InitState(Mathf.RoundToInt(Time.realtimeSinceStartup));
        CreateGameBoard();
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                Debug.Log(x + "," + y + " = " + gameBoard[x, y]);
            }
        }
        SetupPlayerBoard();
        CreateVisuals();
    }

    private void SetupPlayerBoard()
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                Debug.Log("GB: " + gameBoard[x, y]);
                playerBoard[x, y, gameBoard[x, y]] = true;
            }
        }
        for(int i = 0; i < settingDiff; i++)
        {
            Vector2Int randCell = new Vector2Int(Random.Range(0, 9), Random.Range(0, 9));
            while(getListOfSelected(randCell.x, randCell.y).Count == 0)
            {
                randCell = new Vector2Int(Random.Range(0, 9), Random.Range(0, 9));
            }
            for (int j = 0; j < 10; j++) playerBoard[randCell.x, randCell.y, j] = false; 
        }
    }

    public void GameButtonClicked(Vector2Int loc)
    { 
        if (!paused)
        { 
            Debug.Log("Heard Game Button " + loc.x + ", " + loc.y);
            playerBoard[loc.x, loc.y, curSelection] = !playerBoard[loc.x, loc.y, curSelection];
            // gameSquares[loc.x, loc.y].GetComponent<GameSquareBehavior>().SetDisplay(getListOfSelected(loc.x, loc.y));
            //bool hasErrors = FindAndDisplayConflictsInPlayerBoard();
            if (!FindAndDisplayConflictsInPlayerBoard())
            {
                Debug.Log("WINNER");
                Win();
            }
        }
    }

    public void SelectButtonClicked(int buttonNum)
    {
        if (!paused)
        {
            if (buttonNum != curSelection)
            {
                curSelection = buttonNum;
                for (int i = 0; i < 9; i++)
                {
                    selectionSquares[i].GetComponent<SelectionSquareBehavior>().SetActive(i == curSelection - 1);
                }
            }
        }
    }

    public void NewGameButtonClicked()
    {
        if(paused) //Coming from a game over screen
        {
            NewGame();
        }
        else //Ask are you sure
        {
            paused = true;
            confirmPanelCaller = "NewGameButton";
            confirmPanel.transform.localPosition = new Vector2(0, -40f);
        }
    }
    public void MainMenuButtonClicked()
    {
        paused = true;
        confirmPanelCaller = "MainMenuButton";
        confirmPanel.transform.localPosition = new Vector2(0, -40f);
    }

    public void YesButtonClicked()
    {
        if(confirmPanelCaller == "NewGameButton")
        {
            NewGame();

        }
        else if(confirmPanelCaller == "MainMenuButton")
        {
            SceneManager.LoadScene(0);
        }
    }

    public void NoButtonPressed()
    {
        confirmPanel.transform.position = new Vector2(-1500f, 480.4f);
        paused = false;
    }

    private void Win()
    {
        paused = true;
        winnerPanel.transform.localPosition = new Vector2(0,-40f);
    }

    private void NewGame()
    {
        //Chage to proper refresh code;
        SceneManager.LoadScene(0);
    }

    private bool CreateGameBoard()
    {
        winnerPanel.transform.position = new Vector2(-1500f, 480.4f);
        confirmPanel.transform.position = new Vector2(-1500f, 480.4f);
        Vector2Int emptyCell = GetNextEmpty();
        if (emptyCell.x == -1) return true;

        List<int> choices = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        while (choices.Count > 0)
        {
            int selection = Random.Range(0, choices.Count);
            int num = choices[selection];
            choices.RemoveAt(selection);
            if (isValid(emptyCell.x, emptyCell.y, num))
            {
                gameBoard[emptyCell.x, emptyCell.y] = num;
                if (CreateGameBoard())
                {
                    return true;
                }
                else
                { 
                    gameBoard[emptyCell.x, emptyCell.y] = 0; // replace it 
                }
            }
        }
        paused = false;
        return false;
    }

    private Vector2Int GetNextEmpty()
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (gameBoard[x, y] == 0) return new Vector2Int(x, y);
            }
        }
        return new Vector2Int(-1, -1);
    }

    private bool IsValidInRow(int x, int y, int num)
    {
        for (int col = 0; col < 9; col++)// board.GetLength(0); d++)
        {
            if (gameBoard[x, col] == num)
            {
                return false;
            }
        }
        return true;
    }
    private bool IsValidInCol(int x, int y, int num)
    {
        for (int row = 0; row < 9; row++)// board.GetLength(0); d++)
        {
            if (gameBoard[row, y] == num)
            {
                return false;
            }
        }
        return true;
    }

    private bool IsValidInBox(int x, int y, int num)
    {
        Vector2Int boxUpperLeft = new Vector2Int(x - x % 3, y - y % 3); 
        for (int col = boxUpperLeft.y; col < boxUpperLeft.y + 3; col++)
        {
            for (int row = boxUpperLeft.x; row < boxUpperLeft.x + 3; row++)
            {
                if (gameBoard[row, col] == num)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool isValid(int x, int y, int num)
    {
        return (IsValidInRow(x, y, num) && IsValidInCol(x, y, num) && IsValidInBox(x, y, num));
    }

    private void CreateVisuals()
    {
        int xOffset = -446;
        int yOffset = 446;
        for (int y = 0; y < 9; ++y)
        {
            if (y == 3 || y == 6) yOffset -= 6;
            for (int x = 0; x < 9; ++x)
            {
                if (x == 3 || x == 6) xOffset += 6;

                gameSquares[x,y] = Instantiate(gameSquare, transform);
                Button b = gameSquares[x,y];
                b.transform.localPosition = new Vector3(xOffset, yOffset, 0);
                b.GetComponent<GameSquareBehavior>().SetDisplay(getListOfSelected(x,y));
                b.GetComponent<GameSquareBehavior>().SetLoc(new Vector2Int(x, y));
                xOffset += 110;
            }
            yOffset -= 110;
            xOffset = -446;
        }

        xOffset = -446;
        for (int i = 0; i < 9; i++)
        {
            if (i == 3 || i == 6) xOffset += 6;
            selectionSquares[i] = Instantiate(selectionSquare, transform);
            Button b = selectionSquares[i];
            b.transform.localPosition = new Vector3(xOffset, -600, 0);
            b.GetComponent<SelectionSquareBehavior>().SetMyNum(i + 1);
            b.GetComponent<SelectionSquareBehavior>().SetActive(i == 0);
            xOffset += 110;
        }
        curSelection = 1; 
    }

    private List<int> getListOfSelected(int x, int y)
    {
        List<int> temp = new List<int>();
        for(int i = 0; i < 10; i++)
        {
            if (playerBoard[x, y, i]) temp.Add(i);
        }
        return temp;
    }

    private bool FindAndDisplayConflictsInPlayerBoard()
    {
        bool hasErrors = false;
        bool hasMultipesOrEmpties = false;
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                List<int> myValues = getListOfSelected(x, y);
                if (myValues.Count == 0 || myValues.Count > 1) hasMultipesOrEmpties = true;
                if (myValues.Count > 0)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        if (y != col)
                        {
                            List<int> checkValues = getListOfSelected(x, col);
                            for (int i = 0; i < myValues.Count; i++)
                            {
                                if (checkValues.Contains(myValues[i]) || checkValues.Contains(myValues[i] * -1))
                                {
                                    myValues[i] = Mathf.Abs(myValues[i]) * -1;
                                    hasErrors = true;
                                }
                            }
                        }
                    }
                    for (int row = 0; row < 9; row++)
                    {
                        if (x != row)
                        {
                            List<int> checkValues = getListOfSelected(row, y);
                            for (int i = 0; i < myValues.Count; i++)
                            {
                                if (checkValues.Contains(myValues[i]) || checkValues.Contains(myValues[i] * -1))
                                {
                                    myValues[i] = Mathf.Abs(myValues[i]) * -1;
                                    hasErrors = true;
                                }
                            }
                        }
                    }
                    Vector2Int boxUpperLeft = new Vector2Int(x - x % 3, y - y % 3);
                    for (int col = boxUpperLeft.y; col < boxUpperLeft.y + 3; col++)
                    {
                        for (int row = boxUpperLeft.x; row < boxUpperLeft.x + 3; row++)
                        {
                            if (x != row && y != col)
                            {
                                Debug.Log("Checking in Box " + row + ", " + col);
                                List<int> checkValues = getListOfSelected(row, col);
                                for (int i = 0; i < myValues.Count; i++)
                                {
                                    if (checkValues.Contains(myValues[i]) || checkValues.Contains(myValues[i] * -1))
                                    {
                                        myValues[i] = Mathf.Abs(myValues[i]) * -1;
                                        hasErrors = true;
                                    }
                                }
                            }
                        }
                    }
                }
                gameSquares[x, y].GetComponent<GameSquareBehavior>().SetDisplay(myValues);
            }
        }
        return (hasErrors || hasMultipesOrEmpties);
    }
}
