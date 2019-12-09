using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bumperScriptOld : MonoBehaviour
{

    Light lightBumper;
    ParticleSystem particles;
    float wTimer ;
    float rTimer ;

    // Start is called before the first frame update
    void Start()
    {
        lightBumper = GetComponent<Light>();
        particles = GetComponent<ParticleSystem>();
        particles.Stop(true);

    }

    // Update is called once per frame
    void Update()
    {
        if (lightBumper.color == Color.blue)
            wTimer += Time.deltaTime;

        if(wTimer > 0.5){
            lightBumper.color = Color.white;
            wTimer = 0;
            particles.Stop(true);

        }

        if(lightBumper.color == Color.green){
            rTimer += Time.deltaTime;
        }

        if(rTimer > 0.5){
            lightBumper.color = Color.red;
            rTimer = 0;
            particles.Stop(true);
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(lightBumper.color == Color.white){
            lightBumper.color = Color.blue;
        }

        if(lightBumper.color == Color.red){
            lightBumper.color = Color.green;
        }

        particles.Play(true);
        collision.rigidbody.AddForce(Vector2.up * 150, ForceMode2D.Impulse);
    }


}
