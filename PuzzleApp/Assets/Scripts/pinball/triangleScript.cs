﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triangleScript : MonoBehaviour
{
    public int power = 5;
    private Rigidbody2D rb;
    Animator r_Animator;
    

    void start(){
        rb = GetComponent<Rigidbody2D>();
        r_Animator = gameObject.GetComponent<Animator>();
    }
     void OnCollisionEnter2D(Collision2D col)
     {
         //Debug.Log ("bumper col");
         if(col.gameObject.tag == "ball") // Do not forget assign tag to the field
         {
             rb = col.gameObject.GetComponent<Rigidbody2D>();    
             rb.AddForce(-col.contacts[0].normal * power, ForceMode2D.Impulse);
             //r_Animator.SetTrigger ("rotateBumper");
             Color transparencyColor = new Color(1, 1, 1, 0.50f);
             gameStats.score += 50;
             Debug.Log("score update " + gameStats.score);
         }
     }
}
