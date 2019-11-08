using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public Vector3 rotationPoint;
    private float previousTime;
    public float fallTime = 1.0f;
    private static Transform[,] grid = new Transform[10, 20];
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!isValid())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }
        }
        
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(0.5f, 0, 0);
            if (!isValid())
            {
                transform.position -= new Vector3(0.5f, 0, 0);
            }
        }
        
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isValid())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            }
        }

        if(Time.time - previousTime > (Input.GetKeyDown(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!isValid())
            {
                transform.position -= new Vector3(0, -1, 0);
                this.enabled = false;
                FindObjectOfType<SpawnTetromino>().Spawn();
            }
            previousTime = Time.time;
        }

        MoveLeft();
        MoveRight();
    }
    
    public void MoveLeft()
    {
        transform.position += new Vector3(-0.5f, 0, 0);
        if (!isValid())
        {
            transform.position -= new Vector3(-0.5f, 0, 0);
        }
    }
 
    public void MoveRight()
    {
        transform.position += new Vector3(0.5f, 0, 0);
        if (!isValid())
        {
            transform.position -= new Vector3(0.5f, 0, 0);
        }
    }

    bool isValid()
    {
        int roundedX = (int)(transform.position.x);
        int roundedY = (int)(transform.position.y);
        
        if (roundedX < -4 || roundedX >= 4 || roundedY < -9 || roundedY >= 11)
        {
            return false;
        }

        return true;
    }
}
