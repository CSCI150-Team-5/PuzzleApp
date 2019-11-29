using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    private bool[,,] board = new bool[9, 9, 10];
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
    [SerializeField]
    private GameObject settingsPanel;
    [SerializeField]
    private Toggle noteToggle;
    [SerializeField]
    private Toggle highlightToggle;

    private int settingDiff = 1;
    private bool settingAllowNotes = false;
    private bool settingHighlightErrors = true;

    private int curSelection;
    private bool paused = false;

    private Vector3 outOfBounds = new Vector3(10000, 10000, 0);

    void Awake()
    {
        if(PlayerPrefs.HasKey("sudoku_notes"))
        {
            Debug.Log("Found Notes " + PlayerPrefs.GetInt("sudoku_notes"));
            settingAllowNotes = (PlayerPrefs.GetInt("sudoku_notes") == 1) ? true : false;
        }
        else
        {
            settingAllowNotes = false;
            PlayerPrefs.SetInt("sudoku_notes", 0);
        }
        noteToggle.isOn = settingAllowNotes;

        if (PlayerPrefs.HasKey("sudoku_highlight"))
        {
            Debug.Log("Found Highlights "+ PlayerPrefs.GetInt("sudoku_highlight"));
            settingHighlightErrors = (PlayerPrefs.GetInt("sudoku_highlight") == 1) ? true : false;
        }
        else
        {
            settingHighlightErrors = false;
            PlayerPrefs.SetInt("sudoku_highlight", 0);
        }
        highlightToggle.isOn = settingHighlightErrors;
        PlayerPrefs.Save();

        ClearAllPanels();
        CreateButtons();
    }
    
    public void NewGame(int difficulty)
    {
        settingDiff = difficulty;
        board = new bool[9, 9, 10];
        ClearAllPanels();
        RandomizeBoard();
        RandomRemove();
        DisplayEntireBoard();
    }

    private void ClearAllPanels()
    {
        winnerPanel.transform.localPosition = outOfBounds;
        confirmPanel.transform.localPosition = outOfBounds;
        settingsPanel.transform.localPosition = outOfBounds;
    }

    private void CreateButtons()
    {
        int xOffset = -446;
        int yOffset = 446;
        for (int y = 0; y < 9; ++y)
        {
            if (y == 3 || y == 6) yOffset -= 6;
            for (int x = 0; x < 9; ++x)
            {
                if (x == 3 || x == 6) xOffset += 6;

                gameSquares[x, y] = Instantiate(gameSquare, transform);
                Button b = gameSquares[x, y];
                b.transform.localPosition = new Vector3(xOffset, yOffset, 0);
                b.GetComponent<GameSquareBehavior>().SetLoc(new Vector2Int(x, y));
                b.GetComponent<GameSquareBehavior>().SetDisplay(new List<int>(), settingHighlightErrors);
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

    private bool RandomizeBoard()
    {
        Vector2Int emptyCell = GetNextEmpty();
        if (emptyCell.x == -1)
        {
            Debug.Log("Found Empty At " + emptyCell.x + "," + emptyCell.y);
            return true;
        }

        List<int> choices = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        while (choices.Count > 0)
        {
            int selection = Random.Range(0, choices.Count);
            int num = choices[selection];
            choices.RemoveAt(selection);
            if (isValid(emptyCell.x, emptyCell.y, num))
            {
                Debug.Log(emptyCell.x + "," + emptyCell.y + ": " + num + " is Valid");
                board[emptyCell.x, emptyCell.y,num] = true;
                if (RandomizeBoard())
                {
                    return true;
                }
                else
                {
                    board[emptyCell.x, emptyCell.y, num] = false; // replace it 
                }
            }
        }
        return false;
    }

    private void RandomRemove()
    {
        int toRemove = settingDiff;
        while(toRemove > 0)
        {
            Vector2Int cell = new Vector2Int(Random.Range(0, 9), Random.Range(0, 9));
            List<int> selected = GetListOfSelected(cell.x, cell.y);
            if(selected.Count > 0)
            {
                board[cell.x, cell.y, 0] = true;
                for (int i = 1; i < 10; i++)
                {
                    board[cell.x, cell.y, i] = false;
                }
                toRemove--;
            }
        }
        for(int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                Debug.Log("CELL " + x + "," + y + " is " + (board[x, y, 0] ? "true" : "false"));
            }
        }
    }

    private void DisplayEntireBoard()
    {
        for(int y = 0; y < 9; y++)
        {
            for(int x = 0; x < 9; x++)
            {
                DisplayCell(new Vector2Int(x, y));
            }
        }
    }

    private void DisplayCell(Vector2Int cell)
    {
        //Debug.Log("IS A LOCKED CELL: " + (board[cell.x,cell.y,0] ? "true" : "false"));
        //Debug.Log("STORED VALUES: "+cell.x+","+cell.y);
        //DisplayContentsOfListOfInts(GetListOfSelected(cell.x, cell.y));
        gameSquares[cell.x, cell.y].GetComponent<GameSquareBehavior>().SetDisplay(GetListOfSelected(cell.x, cell.y), settingHighlightErrors, board[cell.x, cell.y, 0]);
    }

    private Vector2Int GetNextEmpty()
    {
        bool isEmpty = true;
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (board[x, y, i] == true)
                    {
                        isEmpty = false;
                        break; //This spot has a number so skip
                    }
                }
                if(isEmpty) return new Vector2Int(x, y); //This spot had no number so is the next empty
                isEmpty = true;
            }
        }
        return new Vector2Int(-1, -1);
    }

    private bool IsValidInRow(int x, int y, int num)
    {
        for (int col = 0; col < 9; col++)// board.GetLength(0); d++)
        {
            if (board[x, col, num])
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
            if (board[row, y, num])
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
                if (board[row, col,num])
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

    private List<int> GetListOfSelected(int x, int y)
    {
        List<int> temp = new List<int>();
        for (int i = 1; i < 10; i++)
        {
            if (board[x, y, i]) temp.Add(i);
        }
        return temp;
    }

    /*
    private void SetFromListOfSelected(Vector2Int cell, List<int> selected)
    {
        for(int i = 1; i < 10; i ++)
        {
            board[cell.x, cell.y, i] = false;
        }
        if(selected.Count > 0)
        {
            foreach(int i in selected)
            {
                board[cell.x, cell.y, Mathf.Abs(i)] = true;
            }
        }
    }
    */
    public void SelectButtonClicked(int sel)
    {
        selectionSquares[curSelection-1].GetComponent<SelectionSquareBehavior>().SetActive(false);
        curSelection = sel;
        selectionSquares[curSelection-1].GetComponent<SelectionSquareBehavior>().SetActive(true);
    }

    public void GameButtonClicked(Vector2Int cell)
    {
        bool hasErrorsOrMultiples = false;
        Debug.Log("CurSel: " + curSelection);
        if (board[cell.x, cell.y, 0])//This is not one of the base squares
        {
            if (settingAllowNotes)
            {
                board[cell.x, cell.y, curSelection] = !board[cell.x, cell.y, curSelection];
            }
            else
            {
                for (int i = 1; i < 10; i++)
                {
                    board[cell.x, cell.y, i] = (i == curSelection) ? !board[cell.x, cell.y, i] : false;
                }
            }
            if (settingHighlightErrors)
            {
                hasErrorsOrMultiples = FindConflictsInPlayerBoard(true);
            }
            else
            {
                DisplayCell(cell);
                hasErrorsOrMultiples = FindConflictsInPlayerBoard(false);
            }
        }
        if (!hasErrorsOrMultiples) Debug.Log("WINNER!!!!!!");
    }

    private bool FindConflictsInPlayerBoard(bool alsoDisplay)
    {
        bool hasErrors = false;
        bool hasMultipesOrEmpties = false;
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (board[x, y, 0]) //This is not one of the base squares
                {
                    List<int> myValues = GetListOfSelected(x, y);
                    if (myValues.Count == 0 || myValues.Count > 1)
                    {
                        hasMultipesOrEmpties = true;
                        if (!alsoDisplay) return true;
                    }
                    if (myValues.Count > 0)
                    {
                        for (int col = 0; col < 9; col++)
                        {
                            if (y != col)
                            {
                                List<int> checkValues = GetListOfSelected(x, col);
                                for (int i = 0; i < myValues.Count; i++)
                                {
                                    if (checkValues.Contains(myValues[i]) || checkValues.Contains(myValues[i] * -1))
                                    {
                                        myValues[i] = Mathf.Abs(myValues[i]) * -1;
                                        hasErrors = true;
                                        if (!alsoDisplay) return true;
                                    }
                                }
                            }
                        }
                        for (int row = 0; row < 9; row++)
                        {
                            if (x != row)
                            {
                                List<int> checkValues = GetListOfSelected(row, y);
                                for (int i = 0; i < myValues.Count; i++)
                                {
                                    if (checkValues.Contains(myValues[i]) || checkValues.Contains(myValues[i] * -1))
                                    {
                                        myValues[i] = Mathf.Abs(myValues[i]) * -1;
                                        hasErrors = true;
                                        if (!alsoDisplay) return true;
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
                                    //Debug.Log("Checking in Box " + row + ", " + col);
                                    List<int> checkValues = GetListOfSelected(row, col);
                                    for (int i = 0; i < myValues.Count; i++)
                                    {
                                        if (checkValues.Contains(myValues[i]) || checkValues.Contains(myValues[i] * -1))
                                        {
                                            myValues[i] = Mathf.Abs(myValues[i]) * -1;
                                            hasErrors = true;
                                            if (!alsoDisplay) return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //if(settingHighlight)
                    if(alsoDisplay) gameSquares[x, y].GetComponent<GameSquareBehavior>().SetDisplay(myValues, settingHighlightErrors);
                    //Debug.Log("MY VALUES: "+x+","+y);
                    //DisplayContentsOfListOfInts(myValues);
                    //SetFromListOfSelected(new Vector2Int(x, y), myValues);
                }
            }
        }
        return (hasErrors || hasMultipesOrEmpties);
    }

    /*
    private void DisplayContentsOfListOfInts(List<int> l)
    {
        if (l.Count > 0)
        {
            string tempstring = "";
            foreach (int i in l)
            {
                tempstring += i + ", ";

            }
            Debug.Log("LIST: " + tempstring);
        }
        else
        {
            Debug.Log("LIST: NULL");
        }
    }
    */
    public void SetHighlightSetting(bool setting)
    {
        settingHighlightErrors = setting;
        PlayerPrefs.SetInt("sudoku_highlight", settingHighlightErrors ? 1 : 0);
        PlayerPrefs.Save();
        if(settingHighlightErrors) FindConflictsInPlayerBoard(true);
    }

    public void SetNoteSetting(bool setting)
    {
        settingAllowNotes = setting;
        PlayerPrefs.SetInt("sudoku_notes", settingAllowNotes ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
