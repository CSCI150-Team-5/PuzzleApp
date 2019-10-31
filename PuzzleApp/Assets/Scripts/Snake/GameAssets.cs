using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets instance;

    public Camera cam;
    public Sprite tileSprite;
    public Sprite snakeheadSprite;
    public Sprite snakebodySprite;
    public Sprite appleSprite;
    public Vector2Int gridSize;

    //public int halfVert;

    private void Awake()
    {
        //halfVert = gridSize.y / 2;
        instance = this;
    }
}
