using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDrop : MonoBehaviour
{
    private float previousTime;
    public float fallTime = 0.8f;

    void Start()
    {
        
    }

    void Update()
    {
        downFall();
    }

    public void downFall()
    {
        if (Time.time - previousTime > fallTime)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 1);
            previousTime = Time.time;
        }
    }
}
