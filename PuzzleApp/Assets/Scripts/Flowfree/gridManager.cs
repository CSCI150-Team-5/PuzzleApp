using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridManager : MonoBehaviour
{
    [SerializeField]
    private int rows = 5;
    [SerializeField]
    private int cols = 5;
    [SerializeField]
    private float tileSize = 1;

    // Start is called before the first frame update
    void Start()
    {
        generateGrid();
    }
    private void generateGrid()
    {
        GameObject refereceTile = (GameObject)Instantiate(Resources.Load("Flowfree/pixelTile"));
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject tile = (GameObject)Instantiate(refereceTile, transform);

                float posX = col * tileSize;
                float posY = row * -tileSize;

                tile.transform.position = new Vector2(posX,posY);

            }//cols
            Destroy(refereceTile);

            
        }//rows

        float gridW = cols * tileSize;
        float gridH = rows * tileSize;

        transform.position = new Vector2(-gridW/2 + tileSize/2, gridH/2 +1+ tileSize/2);
        
    }//end generateGrid
    
}
