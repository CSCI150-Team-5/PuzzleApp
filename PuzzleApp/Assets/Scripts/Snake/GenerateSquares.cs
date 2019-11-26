using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateSquares : MonoBehaviour
{
    [SerializeField]
    private Image square;

    Vector2 lowerLeft = new Vector2(-895, -400);
    Vector2 boxSize = new Vector2(51, 50);
    Vector2 gridSize = new Vector2(36, 17);

    // Start is called before the first frame update
    void Awake()
    {
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.AutoRotation;
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Image s = Instantiate(square, Vector3.zero, Quaternion.identity);
                s.rectTransform.SetParent(transform, false);
                s.transform.localScale = boxSize / 100;//new Vector3(.51f, .5f, 1);
                s.rectTransform.anchoredPosition = lowerLeft + boxSize * new Vector2(x, y);// new Vector2(x * 51, y * 50);
            }
        }
    }
}
