using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetris_Block : MonoBehaviour
{
    public Vector3 rotationPoint;
    private float previousTime;
    public float fallTime = 0.1f;
    public static int height = 22;
    public static int width = 10;
    private static Transform[,] grid = new Transform [width, height];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-0.5f, 0, 0);
            if (!isValid())
            {
                transform.position += new Vector3(0.5f, 0, 0);
            }
        }
        
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(0.5f, 0, 0);
            if (!isValid())
            {
                transform.position += new Vector3(-0.5f, 0, 0);
            }
        }
        
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            if (!isValid())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            }
        }

        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -0.5f, 0);
            if (!isValid())
            {
                transform.position += new Vector3(0, 0.5f, 0);
                Add2Grid();
                this.enabled = false;
                FindObjectOfType<Spawn_Tetromino>().NewTetromino();
            }
            previousTime = Time.time;
        }
    }

    void Add2Grid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = (int)(children.transform.position.x);
            int roundedY = (int)(children.transform.position.y);

            grid[roundedX, roundedY] = children;
        }
    }

    bool isValid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = (int)(children.transform.position.x);
            int roundedY = (int)(children.transform.position.y);

            if(roundedX < 1 || roundedX >= width || roundedY < 1 || roundedY >= height)
            {
                return false;
            }

            if(grid[roundedX, roundedY] != null)
            {
                return false;
            }
        }

        return true;
    }
}
