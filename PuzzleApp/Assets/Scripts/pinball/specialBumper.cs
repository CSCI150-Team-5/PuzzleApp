using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class specialBumper : MonoBehaviour
{
    
    public int power = 3;
    private Rigidbody2D rb;
    Animator r_Animator;
    

    void start(){
        //get rigidbody so object can be munipulated
        rb = GetComponent<Rigidbody2D>();
        //grab animator component
        r_Animator = gameObject.GetComponent<Animator>();
    }
     void OnCollisionEnter2D(Collision2D col)
     {
         //Debug.Log ("bumper col");
         //if collider hits an object with the ball tag
         if(col.gameObject.tag == "ball") // Do not forget assign tag to the field
         {
             //assign rb to Gameobject
             rb = col.gameObject.GetComponent<Rigidbody2D>();
             //apply force to gameObject     
             rb.AddForce(-col.contacts[0].normal * power, ForceMode2D.Impulse);
             //r_Animator.SetTrigger ("rotateBumper");
             Color transparencyColor = new Color(1, 1, 1, 0.50f);
             //update score
             gameStats.score += 250;
             //specialBumper spawns a free ball on hit in game area
             GameObject freeBall = (GameObject)Instantiate(Resources.Load("pinball/freeBall"));
             //Debug.Log("score update " + gameStats.score);
         }
     }
}
