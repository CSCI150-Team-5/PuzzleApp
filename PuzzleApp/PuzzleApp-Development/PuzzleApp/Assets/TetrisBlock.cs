using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public Vector3 rotationPoint;
    private float previousTime;
    public float fallTime = 1.0f;
    public static int height = 20;
    public static int width = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //Debug.Log(transform.position.x);
            if(isValid())
            {
                transform.position += new Vector3(-1, 0, 0);
            }
            if (!isValid())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }
        }
        
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!isValid())
            {
                transform.position -= new Vector3(1, 0, 0);
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
    }
    
    public void MoveLeft()
    {
        transform.position += new Vector3(-1, 0, 0);
        if (!isValid())
        {
            transform.position -= new Vector3(-1, 0, 0);
        }
    }
 
    public void MoveRight()
    {
        transform.position += new Vector3(1, 0, 0);
        if (!isValid())
        {
            transform.position -= new Vector3(1, 0, 0);
        }
    }

    bool isValid()
    {
        int roundedX = Mathf.RoundToInt(transform.position.x);
        int roundedY = Mathf.RoundToInt(transform.position.y);

        Debug.Log(roundedX);
        if (roundedX < -5 || roundedX >= 5 || roundedY < -10 || roundedY >= 11)
        {
            return false;
        }

        return true;
    }
}
