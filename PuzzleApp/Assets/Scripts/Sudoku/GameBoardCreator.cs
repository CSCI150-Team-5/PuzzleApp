using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoardCreator : MonoBehaviour
{

    public Button GameSquarePrefab;
    public Button[] gameSquares = new Button[81];

    // Start is called before the first frame update
    void Awake()
    {
        CreateVisualBoard();

    }

    // Update is called once per frame
    void Update()
    {
        
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
}
