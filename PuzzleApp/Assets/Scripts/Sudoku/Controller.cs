using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    //Player's choices in a 9x9 grid of 10 booleans
    //Element 0: True, Cell is locked ; False, Cell is unlocked
    //Elements 1-9 = True, User placed that number in Cell
    private bool[,,] board = new bool[9, 9, 10];
    
    [SerializeField]
    private Button gameSquare; //Base game object to be replicated 81 times. The primare board interface
    private Button[,] gameSquares = new Button[9, 9]; //Array to hold references to the 81 game objects

    [SerializeField]
    private Button selectionSquare; //Base game object to be replicated 9 times. To select a value
    private Button[] selectionSquares = new Button[9]; //Array to hold references to the 9 game objects

    [SerializeField]
    private GameObject winnerPanel;

    
    [SerializeField]
    private Toggle noteToggle; //Reference to the Toggle Game Object for Notes
    [SerializeField]
    private Toggle highlightToggle; //Reference to the Toggle Game Object for Highlights

    private int settingDiff; //The number of elements to remove from the complete board before displaying to the player
    private bool settingAllowNotes = false; //Whether to allow multiple entries in a single cell
    private bool settingHighlightErrors = true; //Whether to highlight errors in the board

    private int curSelection; //The current value selected
    private bool paused = false; //Whether the game is paused or not

    private Vector3 outOfBounds = new Vector3(10000, 10000, 0); //A place to put elements when not active

    void Awake()
    {
        AccessOrCreateUserSettings();
        ClearAllPanels();
        CreateButtons();
        paused = true;
    }

    //Discover existing user settings saved locally or create them if they don't exist
    //PlayerPrefs does not support booleans so this functions convets to 1 for true and 0 for false
    private void AccessOrCreateUserSettings()
    {
        if (PlayerPrefs.HasKey("sudoku_notes"))
        {
            //Debug.Log("Found Notes " + PlayerPrefs.GetInt("sudoku_notes"));
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
            Debug.Log("Found Highlights " + PlayerPrefs.GetInt("sudoku_highlight"));
            settingHighlightErrors = (PlayerPrefs.GetInt("sudoku_highlight") == 1) ? true : false;
        }
        else
        {
            settingHighlightErrors = false;
            PlayerPrefs.SetInt("sudoku_highlight", 0);
        }
        highlightToggle.isOn = settingHighlightErrors;
        PlayerPrefs.Save();
    }
    
    //Destroy any game currently in progress and establish a new game
    public void NewGame(int difficulty)
    {
        settingDiff = difficulty;
        board = new bool[9, 9, 10];
        ClearAllPanels();
        RandomizeBoard();
        RandomRemove();
        DisplayEntireBoard();
        paused = false;
    }

    //Move all panels out of player's view
    private void ClearAllPanels()
    {
        winnerPanel.transform.localPosition = outOfBounds;
        //confirmPanel.transform.localPosition = outOfBounds;
        //settingsPanel.transform.localPosition = outOfBounds;
    }

    //Instantiate the 81 buttons of the player's board and the 9 value selection buttons
    private void CreateButtons()
    {
        //Create the 81 main games buttons
        Vector2Int offset = new Vector2Int(-446,446); //Starting position for laying out 81 buttons
        for (int y = 0; y < 9; ++y)
        {
            if (y == 3 || y == 6) offset.y -= 6; //Add gap every 3 buttons
            for (int x = 0; x < 9; ++x)
            {
                if (x == 3 || x == 6) offset.x += 6; //Add gap every 3 buttons

                //Create button, place it, assign it's location value, and clear its display
                gameSquares[x, y] = Instantiate(gameSquare, transform);
                Button b = gameSquares[x, y];
                b.transform.localPosition = new Vector3(offset.x, offset.y, 0);
                b.GetComponent<GameSquareBehavior>().SetLoc(new Vector2Int(x, y));
                b.GetComponent<GameSquareBehavior>().SetDisplay(new List<int>(), settingHighlightErrors);

                //Move the offset to the next button's location
                offset.x += 110;
            }

            //Reset the offsets
            offset.y -= 110;
            offset.x = -446;
        }

        offset.x = -446; // Reset the x offset

        //Create the 9 value selection buttons
        for (int i = 0; i < 9; i++)
        {
            if (i == 3 || i == 6) offset.x += 6; //Add gap every 3 buttons

            //Create button, place it, assign it's value and set its display
            selectionSquares[i] = Instantiate(selectionSquare, transform);
            Button b = selectionSquares[i];
            b.transform.localPosition = new Vector3(offset.x, -600, 0);
            b.GetComponent<SelectionSquareBehavior>().SetMyNum(i + 1);

            //Set the first button to active
            b.GetComponent<SelectionSquareBehavior>().SetActive(i == 0);

            //Move the offset to the next button's location
            offset.x += 110;
        }
        //Set the current selection to the first value
        curSelection = 1;
    }

    //Recursive function to create a valid randomized board
    private bool RandomizeBoard()
    {
        //When there are no more empty cells end recursion
        Vector2Int emptyCell = GetNextEmpty();
        if (emptyCell.x == -1)
        {
            //Debug.Log("Found Empty At " + emptyCell.x + "," + emptyCell.y);
            return true;
        }

        //Start this cell with a full list of possible values
        List<int> choices = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        //Loop until a valid number is found or the list runs out (list should never run out)
        while (choices.Count > 0)
        {
            //Choose random number from list and remove it
            int selection = Random.Range(0, choices.Count); 
            int num = choices[selection];
            choices.RemoveAt(selection);
            
            //Check if that number is valid for this cell
            if (isValid(emptyCell.x, emptyCell.y, num))
            {
                //Debug.Log(emptyCell.x + "," + emptyCell.y + ": " + num + " is Valid");
                board[emptyCell.x, emptyCell.y,num] = true; //Assign selected number to cell

                //Move on or clear this cell to be reassigned
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

    //Remove random values until the difficulty threshold has been reached
    private void RandomRemove()
    {
        int toRemove = settingDiff;
        while(toRemove > 0)
        {
            Vector2Int cell = new Vector2Int(Random.Range(0, 9), Random.Range(0, 9)); //Select random cell
            List<int> selected = GetListOfSelected(cell.x, cell.y); //Get list of set values
            
            //If this cell still has a value assigned to it, empty it and decrament number to remove
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
    }

    //Update display on all 81 main game cells
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

    //Update display on given cell
    private void DisplayCell(Vector2Int cell)
    {
        //Debug.Log("IS A LOCKED CELL: " + (board[cell.x,cell.y,0] ? "true" : "false"));
        //Debug.Log("STORED VALUES: "+cell.x+","+cell.y);
        //DisplayContentsOfListOfInts(GetListOfSelected(cell.x, cell.y));
        gameSquares[cell.x, cell.y].GetComponent<GameSquareBehavior>().SetDisplay(GetListOfSelected(cell.x, cell.y), settingHighlightErrors, board[cell.x, cell.y, 0]);
    }

    //Search the game board in order for the next empty cell and return it
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
                        break; //This cell has a number so skip it
                    }
                }
                if(isEmpty) return new Vector2Int(x, y); //This spot had no number so is the next empty
                isEmpty = true; //Reset isEmpty for checking the next cell
            }
        }
        return new Vector2Int(-1, -1); //Return negatives to indicate no empty cells
    }

    //Return true only if no values stored in given cell are present in the rest of the row
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

    //Return true only if no values stored in given cell are present in the rest of the column
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

    //Return true only if no values stored in given cell are present in the rest of the box
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

    //Returns true only if no values stored in the given cell are present in the row, column and box
    public bool isValid(int x, int y, int num)
    {
        return (IsValidInRow(x, y, num) && IsValidInCol(x, y, num) && IsValidInBox(x, y, num));
    }

    //Return a list of all values stored in the given cell
    private List<int> GetListOfSelected(int x, int y)
    {
        List<int> temp = new List<int>();
        for (int i = 1; i < 10; i++)
        {
            if (board[x, y, i]) temp.Add(i);
        }
        return temp;
    }

    //Respond to any of the 9 selection buttons being clicked
    public void SelectButtonClicked(int sel)
    {
        //Only respond if the game is unpaused
        if (!paused)
        {
            //Set the current button to inactive, update the current selection and then set the new button to active 
            selectionSquares[curSelection - 1].GetComponent<SelectionSquareBehavior>().SetActive(false);
            curSelection = sel;
            selectionSquares[curSelection - 1].GetComponent<SelectionSquareBehavior>().SetActive(true);
        }
    }

    //Respond to any of the 81 game board buttons being clicked
    public void GameButtonClicked(Vector2Int cell)
    {
        bool hasErrorsOrMultiples = true;
        //Debug.Log("CurSel: " + curSelection);

        //Only respond when not clicking on a base cell and the gmae is unpaused
        if (board[cell.x, cell.y, 0] && !paused)
        {
            if (settingAllowNotes) //If taking notes is turned on...
            {
                //...just have to toggle the value selected in the cell
                board[cell.x, cell.y, curSelection] = !board[cell.x, cell.y, curSelection];
            }
            else //Otherwise if taking notes is turned off...
            {
                //...turn off all values that may be present and toggle the value selected
                for (int i = 1; i < 10; i++)
                {
                    board[cell.x, cell.y, i] = (i == curSelection) ? !board[cell.x, cell.y, i] : false;
                }
            }
            if (settingHighlightErrors) //If error highlighting is turned on...
            {
                //...check for a full, valid gameboard and display any errors found
                hasErrorsOrMultiples = FindConflictsInPlayerBoard(true);
            }
            else //If error highlighting is turned off...
            {
                //...display just this cell and check for a full, valid gameboard without displaying errors
                DisplayCell(cell);
                hasErrorsOrMultiples = FindConflictsInPlayerBoard(false);
            }
        }
        if (!hasErrorsOrMultiples) GameOver();
    }

    //Display game over
    private void GameOver()
    {
        paused = true;
        winnerPanel.transform.localPosition = Vector3.zero;
    }

    //Search the board for conflicting values or empty cells with option to display errors found
    //Return true only if no empty cells, cells with more than one value or conflicting cells are present
    //Uses negatives to indicate errors
    private bool FindConflictsInPlayerBoard(bool alsoDisplay)
    {
        bool hasErrors = false;
        bool hasMultipesOrEmpties = false;
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (board[x, y, 0]) //This is not one of the base cells
                {
                    List<int> myValues = GetListOfSelected(x, y); //Get list of values stored in this cell
                    
                    //If it is empty or conatins more than one value set flag
                    if (myValues.Count == 0 || myValues.Count > 1)
                    {
                        hasMultipesOrEmpties = true;
                        if (!alsoDisplay) return true; //Can return now if not displaying values
                    }

                    //If this original cell does have values stored proceed to check row, column and box for conflicts
                    if (myValues.Count > 0)
                    {
                        //Check the row
                        for (int col = 0; col < 9; col++)
                        {
                            if (y != col) //Skip the original cell
                            {
                                List<int> checkValues = GetListOfSelected(x, col); //Get all values in new cell
                                for (int i = 0; i < myValues.Count; i++) //For every value in the new cell
                                {
                                    //Check if there is a match (positive or negative) with the original cell
                                    if (checkValues.Contains(myValues[i]) || checkValues.Contains(myValues[i] * -1))
                                    {
                                        myValues[i] = Mathf.Abs(myValues[i]) * -1; //Set original cells value to negative
                                        hasErrors = true; //Flag has errors
                                        if (!alsoDisplay) return true; //Can return now if not displaying values
                                    }
                                }
                            }
                        }
                        //Check the column
                        for (int row = 0; row < 9; row++)
                        {
                            if (x != row) //Skip the original cell
                            {
                                List<int> checkValues = GetListOfSelected(row, y); //Get all values in new cell
                                for (int i = 0; i < myValues.Count; i++) //For every value in the new cell
                                {
                                    //Check if there is a match (positive or negative) with the original cell
                                    if (checkValues.Contains(myValues[i]) || checkValues.Contains(myValues[i] * -1))
                                    {
                                        myValues[i] = Mathf.Abs(myValues[i]) * -1; //Set original cells value to negative
                                        hasErrors = true; //Flag has errors
                                        if (!alsoDisplay) return true; //Can return now if not displaying values
                                    }
                                }
                            }
                        }
                        //Check the box
                        //Get the upperleft cell of the box
                        Vector2Int boxUpperLeft = new Vector2Int(x - x % 3, y - y % 3);
                        for (int col = boxUpperLeft.y; col < boxUpperLeft.y + 3; col++)
                        {
                            for (int row = boxUpperLeft.x; row < boxUpperLeft.x + 3; row++)
                            {
                                if (x != row && y != col) //Skip the original cell
                                {
                                    List<int> checkValues = GetListOfSelected(row, col); //Get all values in new cell
                                    for (int i = 0; i < myValues.Count; i++) //For every value in the new cell
                                    {
                                        //Check if there is a match (positive or negative) with the original cell
                                        if (checkValues.Contains(myValues[i]) || checkValues.Contains(myValues[i] * -1))
                                        {
                                            myValues[i] = Mathf.Abs(myValues[i]) * -1; //Set original cells value to negative
                                            hasErrors = true; //Flag has errors
                                            if (!alsoDisplay) return true; //Can return now if not displaying values
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //If displaying values, send myValues to the gameboard object
                    if(alsoDisplay) gameSquares[x, y].GetComponent<GameSquareBehavior>().SetDisplay(myValues, settingHighlightErrors);
                }
            }
        }
        //Return true only if no errors and no multiples or empties
        return (hasErrors || hasMultipesOrEmpties);
    }

    //Handle clicks of the highlight settings toggle
    public void SetHighlightSetting(bool setting)
    {
        //Set the flag, update and save playerprefs, and search for and display conflicts if on
        settingHighlightErrors = setting;
        PlayerPrefs.SetInt("sudoku_highlight", settingHighlightErrors ? 1 : 0);
        PlayerPrefs.Save();
        if(settingHighlightErrors) FindConflictsInPlayerBoard(true);
    }

    //Handle clicks of the notes setting toggle
    public void SetNoteSetting(bool setting)
    {
        //Set flag, update and save playerprefs
        settingAllowNotes = setting;
        PlayerPrefs.SetInt("sudoku_notes", settingAllowNotes ? 1 : 0);
        PlayerPrefs.Save();
    }

    //Handle clicks of the main menu button
    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
