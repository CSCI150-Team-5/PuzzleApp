using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetris_Block : MonoBehaviour
{
    public Vector3 rotationPoint;
    private float previousTime;
    public float fallTime = 0.4f;
    public static int height = 22;
    public static int width = 9;
    private static Transform[,] grid = new Transform [width, height];
    GameObject spawner;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!isValid()) 
            { 
                transform.position += new Vector3(1, 0, 0);
            }
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!isValid())
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            if (!isValid())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, -1), -90);
            }
        }

        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!isValid())
            {
                transform.position += new Vector3(0, 1, 0);
                this.enabled = false;
                Add2Grid();
                CheckLines();
                FindObjectOfType<Spawn_Tetromino>().NewTetromino();
            }

            previousTime = Time.time;
        }
    }

    void CheckLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if(isLine(i))
            {
                Destruction(i);
                RowDown(i);
            }
        }
    }

    bool isLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
            {
                return false;
            }
        }

        return true;
    }

    void Destruction(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for(int j = 0; j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y-1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y-1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    public void MoveLeft()
    {
        transform.position += new Vector3(-1, 0, 0);
        if (!isValid()) 
        { 
            transform.position += new Vector3(1, 0, 0);
        }
    }

    public void MoveRight()
    {
        transform.position += new Vector3(1, 0, 0);
        if (!isValid())
        {
            transform.position += new Vector3(-1, 0, 0);
        }
    }

    public void Rotation()
    {
        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
        if (!isValid())
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, -1), -90);
        }
    }

    void Add2Grid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundedX, roundedY] = children;
        }
    }

    bool isValid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
            {
                return false;
            }

            if (grid[roundedX, roundedY] != null)
            {
                if (roundedY >= height - 4)
                {
                    spawner.SendMessage("Stop");
                }
                return false;
            }
        }

        return true;
    }

    public void AssignSpawner(GameObject go)
    {
        spawner = go;
    }

    public void EmptyGrid()
    {
       grid = new Transform[width, height];
    }
}