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

     int [,] l1 = new int [6,6] 
        {
            {1, 0, 0, 0, 0 ,0} ,   
            {0, 0, 0, 0, 0, 0} ,
            {0, 0, 0, 0 ,0 ,0} ,   
            {0, 0, 0, 0, 0, 0} ,   
            {0, 0, 0, 0, 0, 0},  
            {1, 0, 0, 0, 0, 0}  
        };

    // Start is called before the first frame update
    void Start()
    {        
        generateGrid();
    }
    //generates the grid using a 2d array
    private void generateGrid()    {
        
        
        GameObject refereceTile = (GameObject)Instantiate(Resources.Load("Flowfree/pixelTile"));
        GameObject redDot = (GameObject)Instantiate(Resources.Load("Flowfree/redDot"));
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                //uses prebad to generate tiles
                GameObject tile = (GameObject)Instantiate(refereceTile, transform);
                float posX = col * tileSize;
                float posY = row * -tileSize;
                TileProperties.posX = row;
                TileProperties.posY = col;
                Debug.Log ("posX: " + TileProperties.posX + " posY: " + TileProperties.posY );
                tile.transform.position = new Vector2(posX,posY);

                if(l1[row,col] == 1){
                GameObject dot = (GameObject)Instantiate(redDot, transform);
                dot.transform.position = new Vector2(posX,posY);
                }
                
            }//cols
            //Destroy(refereceTile);
            //move reference tiles
            refereceTile.transform.position = new Vector2(100,100);
            redDot.transform.position = new Vector2(100,100);

            
        }//rows
        //get mid coordinates and position the grid centerscreen
        float gridW = cols * tileSize;
        float gridH = rows * tileSize;

        transform.position = new Vector2(-gridW/2 + tileSize/2, gridH/2 +1+ tileSize/2);
        


        
    }//end generateGrid
    
    
}
