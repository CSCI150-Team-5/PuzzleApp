using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicBoard : MonoBehaviour
{
	public PicBoard(int width, int height)	//This constructor allocates all the space we need ofr instance's lists.
	{
		PicAssets.instance.tiles = new List<List<PicTile>>(width);
		PicAssets.instance.tileObjects = new List<List<GameObject>>(width);
		for (int i = 0; i < width; i++)
		{
			PicAssets.instance.tiles.Add(new List<PicTile>(height));
			for (int j = 0; j < height; j++) PicAssets.instance.tiles[i].Add(null);	//Reserving its space with dummy values.
			PicAssets.instance.tileObjects.Add(new List<GameObject>(height));
			for (int j = 0; j < height; j++) PicAssets.instance.tileObjects[i].Add(null);
		}

		PicAssets.instance.rowObjects = new List<GameObject>(height);
		PicAssets.instance.rows = new List<List<int>>(height);
		for (int i = 0; i < height; i++)
			PicAssets.instance.rows.Add(new List<int>());
		PicAssets.instance.columnObjects = new List<GameObject>(height);
		PicAssets.instance.columns = new List<List<int>>(height);
		for (int i = 0; i < width; i++)
			PicAssets.instance.columns.Add(new List<int>());
	}

	public bool checkRow(int rowNum)	//This function checks if all the numbers in a row have been completed.
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

	public bool checkColumn(int columnNum)	//Largely the same as in checkRow, except itterates the other way.
	{
		int index = PicAssets.instance.h - 1;
		for (int numIndex = 0; numIndex < PicAssets.instance.columns[columnNum].Count; numIndex++)
		{
			int numbersHit = 0;
			bool hit = false;

			for(;;index--)
			{
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

	public void checkWon()	//Checks if we met the win condition.
	{
		for(int i = 0; i < PicAssets.instance.h; i++)
			if (PicAssets.instance.rowObjects[i].GetComponent<Text>().color != Color.gray)
				return;	//If we found a row that isn't gray, then we haven't won.

		for (int i = 0; i < PicAssets.instance.w; i++)
			if (PicAssets.instance.columnObjects[i].GetComponent<Text>().color != Color.gray)
				return;	//If we found a column that isn't gray, then we havne't won.

		PicAssets.instance.gameWon = true;	//Announce that we won.
	}

	public void markRow(int rowNum)	//Flags every unfilled tile in a gray row.
	{
		List<Vector3> toChange = new List<Vector3>();
		for (int i = 0; i < PicAssets.instance.w; i++)
			if ((PicAssets.instance.tiles[i][rowNum].GetComponent<SpriteRenderer>().sprite != PicAssets.instance.filledTexture) && (PicAssets.instance.tiles[i][rowNum].GetComponent<SpriteRenderer>().sprite != PicAssets.instance.flaggedTexture))
				toChange.Add(new Vector3(0f, i, rowNum));

		for (int i = 0; i < toChange.Count; i++)
			PicAssets.instance.tiles[(int)toChange[i].y][(int)toChange[i].z].flag();

		if (toChange.Count != 0) PicAssets.instance.history.Add(toChange);
	}

	public void markColumn(int colNum) //Flags every unfilled tile in a gray column.
	{
		List<Vector3> toChange = new List<Vector3>();
		for (int i = 0; i < PicAssets.instance.h; i++)
			if ((PicAssets.instance.tiles[colNum][i].GetComponent<SpriteRenderer>().sprite != PicAssets.instance.filledTexture) && (PicAssets.instance.tiles[colNum][i].GetComponent<SpriteRenderer>().sprite != PicAssets.instance.flaggedTexture))
				toChange.Add(new Vector3(0f, colNum, i));

		for (int i = 0; i < toChange.Count; i++)
			PicAssets.instance.tiles[(int)toChange[i].y][(int)toChange[i].z].flag();

		if (toChange.Count != 0) PicAssets.instance.history.Add(toChange);
	}
}