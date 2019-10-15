using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void MoveLeft()
    {
        transform.position = new Vector3(transform.position.x - 1, transform.position.y);
    }
 
    public void MoveRight()
    {
        transform.position = new Vector3(transform.position.x + 1, transform.position.y);
    }
}
