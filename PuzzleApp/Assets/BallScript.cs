using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{

    public Transform ballPos;
    public GameObject spawmBall;
    public Vector3 MyGameObjectPosition;
    public float posX;
    public float posY;
    

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        MyGameObjectPosition = GameObject.Find("ball").transform.position;
        posX = MyGameObjectPosition.x;
        posY = MyGameObjectPosition.y;
        if(posY < -10)
        {
            Destroy(this);
            Instantiate(this, ballPos.position, transform.rotation);

        }
    }
}
