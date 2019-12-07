using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickScript : MonoBehaviour
{
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //create raycast to detect object hit with mouse
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (Input.GetMouseButtonDown(0)) 
        {
            //if you hit an object, return postion 
            if(hit.collider != null && hit.collider.gameObject.transform.tag != "dot")
            {
                GameObject dot = (GameObject)Instantiate(Resources.Load("Flowfree/redBar"));
                dot.transform.position = new Vector2(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y);                
                //Debug.Log ("Target Position: " + hit.collider.gameObject.transform.position);
                //Debug.Log ("Target Position: " + hit.collider.gameObject.transform.name);                
            }

            if(hit.collider != null && hit.collider.gameObject.tag == "alive"){
                Debug.Log ("target is alive");
                Destroy(hit.transform.gameObject);
            }
        }
    
    }
    
}
