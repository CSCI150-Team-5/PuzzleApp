using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallCollision : MonoBehaviour
{  
    private Rigidbody2D rb;

    void start(){
        rb = GetComponent<Rigidbody2D>();
    }
     void OnCollisionEnter2D(Collision2D col)
     {
         //Debug.Log ("wall col");
         if(col.gameObject.tag == "ball") // Do not forget assign tag to the field
         {
             rb = col.gameObject.GetComponent<Rigidbody2D>();    
             rb.AddForce(-col.contacts[0].normal * 3, ForceMode2D.Impulse);
         }
     }

}
