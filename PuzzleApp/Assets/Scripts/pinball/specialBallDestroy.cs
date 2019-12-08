using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class specialBallDestroy : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {   
        //setup destroy function for special ball so it won't change life and alive status
        if (this.transform.position.y < -10)
        {
            Destroy(this.gameObject);            
        }
    }
}
