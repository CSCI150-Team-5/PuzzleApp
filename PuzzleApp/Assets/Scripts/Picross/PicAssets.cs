﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicAssets : MonoBehaviour
{
	public static PicAssets instance;

	public Camera cam;
	public Canvas can;
	public Canvas can2;

	public Sprite unfilledTexture;  //Variables for storing textures to be used by the tiles.
	public Sprite filledTexture;    //
	public Sprite flaggedTexture;   //

	public bool gameStart = false;
	public bool gameWon = false;

	//	public PicTile[,] tiles;       //A container of all the picross tiles, their constructors place themselves in here. They are identified by their coordinates in the world.
	public List<List<PicTile>> tiles = new List<List<PicTile>>();       //A container of all the picross tiles, their constructors place themselves in here. They are identified by their coordinates in the world.
	public PicBoard board;
	//	public int[][] rows;
	//	public int[][] columns;
	public List<List<int>> rows = new List<List<int>>();
	public List<List<int>> columns = new List<List<int>>();

	public double emptyFrequency = 0.5;//0.15;

	public int w;
	public int h;

//	public Material mat;

	//	public void checkRows()
	//	{
	////		instance.rows = new List<List<int>>();
	////		foreach (PicTile tile in PicAssets.instance.tiles) if (tile.mine) tile.fill(-1);  //Reveal all the mines because we lost.
	//		for (int rowNumber = 0; rowNumber < instance.h; rowNumber++)
	//		{
	//			instance.rows.Add(new List<int>());
	//
	//			int current = 0;
	//			for (int i = 0; i < instance.w; i++)
	//			{
	////				tiles[0, 0].empty = true;
	//				if (instance.tiles[i,0].empty == true) current++;
	////				if (instance.tiles[i, rowNumber].empty) current++;
	////				else if (current != 0)
	////				{
	//////					PicAssets.instance.rows[rowNumber].Add(current);
	////					current = 0;
	////				}
	//			}
	////			if (current != 0) PicAssets.instance.rows[rowNumber].Add(current);
	//
	//			if (instance.rows[rowNumber].Count == 0) instance.rows[rowNumber].Add(current);
	//		}
	//	}
	//
	//	public void Hi()
	//	{
	//		Debug.Log("Hi");
	////		instance.rows = new List<List<int>>();
	////		foreach (PicTile tile in PicAssets.instance.tiles) if (tile.mine) tile.fill(-1);  //Reveal all the mines because we lost.
	//		for (int rowNumber = 0; rowNumber < instance.h; rowNumber++)
	//		{
	//			instance.rows.Add(new List<int>());
	//
	//			int current = 0;
	//			for (int i = 0; i < instance.w; i++)
	//			{
	////				tiles[0, 0].empty = true;
	//				if (PicAssets.instance.tiles[i,0].empty == true) current++;
	////				if (instance.tiles[i, rowNumber].empty) current++;
	////				else if (current != 0)
	////				{
	//////					PicAssets.instance.rows[rowNumber].Add(current);
	////					current = 0;
	////				}
	//			}
	////			if (current != 0) PicAssets.instance.rows[rowNumber].Add(current);
	//
	//			if (instance.rows[rowNumber].Count == 0) instance.rows[rowNumber].Add(current);
	//		}
	//	}

	void Awake()
	{
		instance = this;
		//		instance.can.transform.localPosition = instance.cam.transform.localPosition;
		//		instance.can.transform.localScale = instance.cam.transform.localScale;
		//		Debug.Log(instance.cam.transform.localPosition);
		//		instance.cam.transform.localPosition = instance.can.transform.localPosition;
		//		instance.cam.transform.localScale = instance.can.transform.localScale;
		/////		instance.cam.GetComponent<Camera>().Size = 960;
	}

	//	void Update()
	//	{
	//		if (instance != this) return;
	//		if (gameStart)
	//		{
	////			instance.board.checkRows();
	////			PicBoard.checkRows();
	////			instance.board.checkRows();
	////			instance.board.checkRows(instance.tiles);
	////			instance.checkRows();
	////			checkRows();
	////			PicFunctionality.newGame();
	//			gameStart = false;
	//
	//
	//		}
	//	}

	//	void OnGUI()
	//	{
	////		GUI.Label(new Rect(10, 10, 100, 70), "Hello World!");
	//		GUI.Label (new Rect (200, 200, 32, 32), flaggedTexture);
	//		GUIUtility.ScaleAroundPivot (Vector2(1,1), Vector2(3,1));
	//	}

	void Update()
	//	void OnPostRender()
	{
		//		if (!mat)
		//		{
		//			Debug.LogError("Please Assign a material on the inspector");
		//			return;
		//		}
//		GL.PushMatrix();
//		mat.SetPass(0);
//		GL.LoadOrtho();
//
//		GL.Begin(GL.LINES);
//		GL.Color(Color.red);
//		GL.Vertex(Vector3.zero);
//		GL.Vertex(new Vector3(10, 10, 0));//mousePos.x / Screen.width, mousePos.y / Screen.height, 0); ;);
//		GL.End();
//
//		GL.PopMatrix();
	}
}
