using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicBoard : MonoBehaviour
{
	public PicBoard(int width, int height)
	{
		///		PicAssets.instance.tiles = new PicTile[width, height];
		PicAssets.instance.tiles = new List<List<PicTile>>(new List<List<PicTile>>(width));
		for (int i = 0; i < width; i++)
		{
			PicAssets.instance.tiles.Add(new List<PicTile>(height));
			for (int j = 0; j < height; j++) PicAssets.instance.tiles[i].Add(new PicTile(false));
		}

		PicAssets.instance.rows = new List<List<int>>(new List<List<int>>(height));
		for (int i = 0; i < height; i++)
			PicAssets.instance.rows.Add(new List<int>());
		PicAssets.instance.columns = new List<List<int>>(new List<List<int>>(height));
		for (int i = 0; i < width; i++)
			PicAssets.instance.columns.Add(new List<int>());

		//		Debug.Log("R is: " + PicAssets.instance.tiles.Count);
		//		for (int i = 0; i < PicAssets.instance.tiles.Count; i++) Debug.Log("L is: " + PicAssets.instance.tiles[i].Count);
		//		PicAssets.instance.rows = new int[height][];
		//		PicAssets.instance.columns = new int[width][];
		//		PicAssets.instance.rows = new List<List<int>>;
		//		PicAssets.instance.columns = new int[width][];
	}

	//	public void checkRows()
	//	{
	////		foreach (PicTile tile in PicAssets.instance.tiles) if (tile.mine) tile.fill(-1);  //Reveal all the mines because we lost.
	//		for (int rowNumber = 0; rowNumber < PicAssets.instance.h; rowNumber++)
	//		{
	//			int current = 0;
	//			for (int i = 0; i < PicAssets.instance.w; i++)
	//			{
	//				if (PicAssets.instance.tiles[i, rowNumber].empty) current++;
	//				else if (current != 0)
	//				{
	////					PicAssets.instance.rows[rowNumber].Add(current);
	//					current = 0;
	//				}
	//			}
	////			if (current != 0) PicAssets.instance.rows[rowNumber].Add(current);
	//
	//			if (PicAssets.instance.rows[rowNumber].Count == 0) PicAssets.instance.rows[rowNumber].Add(current);
	//		}
	//	}

}