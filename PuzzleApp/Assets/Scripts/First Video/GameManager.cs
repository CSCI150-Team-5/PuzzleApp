using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Board mBoard;

    #region Second Video Variables
    public PieceManager mPieceManager;
    #endregion

    private float time;
    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        time = Time.time;
    }

    void Start()
    {

        // Create the board
        //mBoard.Create();

        //#region Second Video Functions
        // Create pieces
        //mPieceManager.Setup(mBoard);
        //#endregion
    }

    private void Update()
    {
        float t = Time.time;
        if(t - time > 1.0)
        {
            // Create the board
            mBoard.Create();

            #region Second Video Functions
            // Create pieces
            mPieceManager.Setup(mBoard);
            #endregion

            Destroy(this);
        }
    }
}
