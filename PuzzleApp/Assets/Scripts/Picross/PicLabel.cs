using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicLabel : MonoBehaviour
{
	public Color color = Color.black;
	public float xpos = -1;
	public float ypos = -1;

	void Awake()
//	void Start()
	{
		xpos = (float)transform.position.x;		//Storing the game world coordinates of the label in the class variables.
		ypos = (float)transform.position.y;		//These will be used to identify the label.

///		Debug.Log("Testing Pos: (" + xpos + "," + ypos + ")");
	}

//	public void OnMouseDown()
//	public void OnPointerEnter()
	public void OnMouseOver()
	{
//		if (PicAssets.instance.gameStart) return;
//		if (PicAssets.instance.gameWon) return; //If the game is over, return because we shouldn't allow any tiles to be flagged or filled.
//		Debug.Log("Testing Pos: (" + xpos + "," + ypos + ")");
		Debug.Log("Testing Pos: (" + transform.position.x + "," + transform.position.y + ")");

///		if (Input.GetMouseButtonDown(0))
///		{
///			Debug.Log("Testing Pos: (" + xpos + "," + ypos + ")");
///
///		}
	}
}
