using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class launcher : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private float thrust = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = this.GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
         
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rb2D.AddForce(Vector2.up * thrust  , ForceMode2D.Impulse);
            Debug.Log ("space");
        }
    }

    void OnMouseDown()
    {
        // launch ball after clicking on it
        rb2D.AddForce(Vector2.up * thrust  , ForceMode2D.Impulse);
        Debug.Log ("mouse down");
    }
}
