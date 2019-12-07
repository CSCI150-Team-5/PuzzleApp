using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelMaker : MonoBehaviour

    
{
    // Start is called before the first frame update
    void Start()
    {
        level1();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void level1(){

    int [,] a = new int [4,4] 
    {
        {1, 0, 0, 0} ,   /*  initializers for row indexed by 0 */
        {0, 0, 0, 0} ,
        {0, 0, 0, 0} ,   /*  initializers for row indexed by 1 */
        {1, 0, 0, 0}   /*  initializers for row indexed by 2 */
    };

    Debug.Log (a[0, 0]);


    }

}
