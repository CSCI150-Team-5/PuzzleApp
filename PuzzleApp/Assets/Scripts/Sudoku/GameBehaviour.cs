using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBehaviour : MonoBehaviour
{
    public delegate void selectedChanged(int n);
    public static event selectedChanged OnSelectedNumChanged;

    private int selectedNum;
    private int settingDif = 50;

    private int[,] userBoard = new int[81, 10];

    private int[,] mainBoard = new int[9, 9];

    private int[,] playerBoard = new int[9, 9];

    public Button GameSquarePrefab;
    public Button[] gameSquares = new Button[81];
    // Start is called before the first frame update
    void Awake()
    {
        GameSquareBehavior.OnPlayedNumsChanged += NumsChanged;
        selectedNum = 1;
        for (int i = 0; i < 81; ++i)
        {
            for (int j = 0; j < 10; ++j)
            {
                userBoard[i, j] = 0;
            }
        }
        for (int i = 0; i < 9; ++i)
        {
            for (int j = 0; j < 9; ++j)
            {
                mainBoard[i, j] = 0;
            }
        }

        //mainBoard[0, 2] = 5;
        //mainBoard[1, 1] = 1;
        //mainBoard[4, 1] = 7;
        //mainBoard[0, 8] = 9;
        //Debug.Log(FindAvail(0,1));

        CreateVisualBoard();
        //CreateBoard();
        solveSudoku(mainBoard, 9);
        playerBoard = mainBoard.Clone() as int[,];
        HideAnswers(playerBoard, settingDif);
        ShowBoard(playerBoard, 9);
    }


    private void Start()
    {
        BroadcastChange();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetSelectedNum()
    {
        return selectedNum;
    }

    public void SetSelectedNum(int n)
    {
        selectedNum = n;
        BroadcastChange();
    }

    private void BroadcastChange()
    {
        if (OnSelectedNumChanged != null)
        {
            OnSelectedNumChanged(selectedNum);
        }
    }

    private void NumsChanged(int n, int[] pn)
    {
        for (int i = 0; i < 10; ++i)
        {
            userBoard[n, i] = pn[i];
        }
        //foreach(int i in board) Debug.Log(i);
    }

    void CreateVisualBoard()
    {
        int xOffset = -446;
        int yOffset = 446;
        for (int y = 0; y < 9; ++y)
        {
            if (y == 3 || y == 6) yOffset -= 6;
            for (int x = 0; x < 9; ++x)
            {
                if (x == 3 || x == 6) xOffset += 6;

                gameSquares[x * 9 + y] = Instantiate(GameSquarePrefab, transform);
                Button b = gameSquares[x * 9 + y];
                b.transform.localPosition = new Vector3(xOffset, yOffset, 0);
                b.GetComponent<GameSquareBehavior>().SetNumber(0, false);

                xOffset += 110;
            }
            yOffset -= 110;
            xOffset = -446;
        }
    }

    private void CreateBoard()
    {
        for (int row = 0; row < 9; ++row)
        {
            for (int col = 0; col < 9; ++col)
            {
                string avail = FindAvail(col, row);
                Debug.Log("Available " + avail);
                int index = Random.Range(0, avail.Length);
                int val = avail[index] - 48; //48 is where numbers start on the ascii table 
                Debug.Log(avail[index] + " = " + val);
                mainBoard[row, col] = val;
                gameSquares[row * 9 + col].GetComponent<GameSquareBehavior>().SetNumber(val, false);
            }
        }

    }

    private string FindAvailableInRow(int row)
    {
        string avail = "123456789";
        for (int col = 0; col < 9; ++col)
        {
            int i = avail.IndexOf(mainBoard[col, row].ToString());
            //Debug.Log("i is " + i.ToString());
            if (i > -1) avail = avail.Remove(i, 1);
        }
        Debug.Log("In Row " + avail);
        return avail;
    }
    private string FindAvailableInCol(int col)
    {
        string avail = "123456789";
        for (int row = 0; row < 9; ++row)
        {
            int i = avail.IndexOf(mainBoard[col, row].ToString());
            Debug.Log("looking for " + mainBoard[col, row].ToString());
            //Debug.Log("i is " + i.ToString());
            if (i > -1) avail = avail.Remove(i, 1);
        }
        Debug.Log("In Col " + avail);
        return avail;
    }

    private string FindAvailableInBox(int r, int c)//int box)
    {
        string avail = "123456789";
        //int rowStart = (int)(box / 3) * 3;
        //int colStart = (box % 3) * 3;
        int rowStart = (int)(r / 3) * 3;
        int colStart = (int)(c / 3) * 3;
        //Debug.Log("Row: "+ r +" R: " + rowStart + " Column: " + c +" C: " + colStart);
        for (int row = rowStart; row < rowStart + 3; ++row)
        {
            for (int col = colStart; col < colStart + 3; ++col)
            {
                int i = avail.IndexOf(mainBoard[col, row].ToString());
                //Debug.Log("i is " + i.ToString());
                if (i > -1) avail = avail.Remove(i, 1);
            }
        }
        Debug.Log("In Box " + avail);
        return avail;
    }

    private string FindAvail(int r, int c)
    {
        Debug.Log("Row " + r + "Col " + c);
        string row = FindAvailableInRow(r);
        string col = FindAvailableInCol(c);
        string box = FindAvailableInBox(r, c);
        string avail = "";

        //Debug.Log("R: " + row);
        //Debug.Log("C: " + col);
        //Debug.Log("B: " + box);
        for (int i = 1; i < 10; ++i)
        {
            if (row.IndexOf(i.ToString()) > -1 && col.IndexOf(i.ToString()) > -1 && box.IndexOf(i.ToString()) > -1)
            {
                avail += i.ToString();
            }
        }
        return avail;
    }

    public static bool solveSudoku(int[,] board, int n) //From GeeksForGeeks.com
    {
        int row = -1;
        int col = -1;
        bool isEmpty = true;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (board[i, j] == 0)
                {
                    row = i;
                    col = j;

                    // we still have some remaining 
                    // missing values in Sudoku 
                    isEmpty = false;
                    break;
                }
            }
            if (!isEmpty)
            {
                break;
            }
        }

        // no empty space left 
        if (isEmpty)
        {
            return true;
        }

        // else for each-row backtrack 
        for (int num = 1; num <= n; num++)
        {
            if (isSafe(board, row, col, num))
            {
                board[row, col] = num;
                if (solveSudoku(board, n))
                {
                    // print(board, n); 
                    return true;
                }
                else
                {
                    board[row, col] = 0; // replace it 
                }
            }
        }
        return false;
    }

    public static bool isSafe(int[,] board, int row, int col, int num) //From GeeksForGeeks.com
    {
        // row has the unique (row-clash) 
        for (int d = 0; d < 9; ++d)// board.GetLength(0); d++)
        {
            // if the number we are trying to  
            // place is already present in  
            // that row, return false; 
            if (board[row, d] == num)
            {
                return false;
            }
        }

        // column has the unique numbers (column-clash) 
        for (int r = 0; r < 9; ++r)// board.GetLength(0); r++)
        {
            // if the number we are trying to 
            // place is already present in 
            // that column, return false; 
            if (board[r, col] == num)
            {
                return false;
            }
        }

        // corresponding square has 
        // unique number (box-clash) 
        int sqrt = 3;//(int)Math.Sqrt(board.GetLength(0));
        int boxRowStart = row - row % sqrt;
        int boxColStart = col - col % sqrt;

        for (int r = boxRowStart;
                r < boxRowStart + sqrt; r++)
        {
            for (int d = boxColStart;
                    d < boxColStart + sqrt; d++)
            {
                if (board[r, d] == num)
                {
                    return false;
                }
            }
        }

        // if there is no clash, it's safe 
        return true;
    }

    private void ShowBoard(int[,] board, int n)
    {
        for (int i = 0; i < n; ++i)
        {
            for (int j = 0; j < n; ++j)
            {
                gameSquares[j * 9 + i].GetComponent<GameSquareBehavior>().SetNumber(board[i, j], false);
            }
        }
    }

    private void HideAnswers(int[,] board, int diff)
    {
        do
        {
            int randX = Random.Range(0, 9);
            int randY = Random.Range(0, 9);
            if(board[randX,randY] > 0)
            {
                board[randX, randY] = 0;
                diff--;
            }
        } while (diff > 0);
    }

}
