using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< Updated upstream
=======
using UnityEngine.UI;
>>>>>>> Stashed changes

public class PicBoard : MonoBehaviour
{
	public PicBoard(int width, int height)
	{
///		PicAssets.instance.tiles = new PicTile[width, height];
		PicAssets.instance.tiles = new List<List<PicTile>>(width);
		PicAssets.instance.tileObjects = new List<List<GameObject>>(width);
		for (int i = 0; i < width; i++)
		{
			PicAssets.instance.tiles.Add(new List<PicTile>(height));
			for (int j = 0; j < height; j++) PicAssets.instance.tiles[i].Add(null);	//Reserving its space.
			PicAssets.instance.tileObjects.Add(new List<GameObject>(height));
//			for (int j = 0; j < height; j++) PicAssets.instance.tileObjects[i].Add(new GameObject());
			for (int j = 0; j < height; j++) PicAssets.instance.tileObjects[i].Add(null);
		}

		PicAssets.instance.rowObjects = new List<GameObject>(height);
		PicAssets.instance.rows = new List<List<int>>(height);
		for (int i = 0; i < height; i++)
			PicAssets.instance.rows.Add(new List<int>());
		PicAssets.instance.columnObjects = new List<GameObject>(height);// width);
		PicAssets.instance.columns = new List<List<int>>(height);
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
	public bool checkRow(int rowNum)
	{
		
		int index = 0;
		for (int numIndex = 0; numIndex < PicAssets.instance.rows[rowNum].Count; numIndex++)
		{
			int numbersHit = 0;
			bool hit = false;

			for(;;index++)
			{
				if (index >= PicAssets.instance.w)
				{
					if (numbersHit == PicAssets.instance.rows[rowNum][numIndex]) break;
					else return false;
				}

				if (PicAssets.instance.tileObjects[index][rowNum].GetComponent<SpriteRenderer>().sprite == PicAssets.instance.filledTexture)
				{
					hit = true;
					numbersHit++;
				}
				else
				{
					if (hit)
					{
						if (numbersHit == PicAssets.instance.rows[rowNum][numIndex])
						{
							if (numIndex + 1 != PicAssets.instance.rows[rowNum].Count) break;
						}
						else return false;
					}
				}
			}
		}

		return true;
	}

	public bool checkColumn(int columnNum)
	{

//		int index = 0;
		int index = PicAssets.instance.h - 1;
		for (int numIndex = 0; numIndex < PicAssets.instance.columns[columnNum].Count; numIndex++)
		{
			int numbersHit = 0;
			bool hit = false;

			for(;;index--)
			{
//				if (index >= PicAssets.instance.h)
				if (index < 0)
				{
					if (numbersHit == PicAssets.instance.columns[columnNum][numIndex]) break;
					else return false;
				}

				if (PicAssets.instance.tileObjects[columnNum][index].GetComponent<SpriteRenderer>().sprite == PicAssets.instance.filledTexture)
				{
					hit = true;
					numbersHit++;
				}
				else
				{
					if (hit)
					{
						if (numbersHit == PicAssets.instance.columns[columnNum][numIndex])
						{
							if (numIndex + 1 != PicAssets.instance.columns[columnNum].Count) break;
						}
						else return false;
					}
				}
			}
		}

		return true;
	}
<<<<<<< Updated upstream
=======

	public void checkWon()
	{
		Debug.Log("' 1 '");
		bool won = true;
		for(int i = 0; i < PicAssets.instance.h; i++)
			if (PicAssets.instance.rowObjects[i].GetComponent<Text>().color != Color.gray)
				return;
		Debug.Log("' 2 '");
		for (int i = 0; i < PicAssets.instance.w; i++)
			if (PicAssets.instance.columnObjects[i].GetComponent<Text>().color != Color.gray)
				return;
		Debug.Log("' 3 '");
		PicAssets.instance.gameWon = true;
	}
>>>>>>> Stashed changes
}