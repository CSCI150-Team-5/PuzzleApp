using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicAI : MonoBehaviour
{
	public static int longestEmptyStreak(bool row, int num, int begin, int end)
//	int longestEmptyStreak(int rowNum, int begin, int end)
	{
		int largest = 0;
		int current = 0;
		bool hit = false;
		for (int i = begin; i < end; i++)
		{
			if (PicAssets.instance.tileObjects[row ? i : num][row ? num : i].GetComponent<SpriteRenderer>().sprite != PicAssets.instance.flaggedTexture)
			{
				hit = true;
				current++;
			}
			else
			{
				if (hit)
				{
					hit = false;
					largest = (current > largest) ? current : largest;
					current = 0;
				}
			}
		}
		if (hit) largest = (current > largest) ? current : largest;
		return largest;
	}

	static List<Vector2> superpositionOfExtremes(bool row, int num, List<Vector2> edges, int begin, int end)
	{
		List<Vector4> markList = new List<Vector4>();// (row ? PicAssets.instance.w : PicAssets.instance.h);

		for (int i = 0; i < edges.Count; i++)
		{
			for (int completed = 0; completed < edges[i].x; completed++)
			{
				markList.Add(new Vector4(edges[i].x, edges[i].y,-2,-2));
			}
			if (markList.Count < (row ? PicAssets.instance.w : PicAssets.instance.h))
				markList.Add(new Vector4(-1, -1, -2, -2)); //-1 represents a flag
		}

		for (int i = markList.Count; i < (row ? PicAssets.instance.w : PicAssets.instance.h); i++)
			markList.Add(new Vector4(-2, -2, -2, -2));

		int currentEdge = edges.Count - 1;
		int currentEdgeCount = 0;
		bool skipNext = false;
		for (int i = (row ? PicAssets.instance.w : PicAssets.instance.h) - 1; i >= 0; i--)
		{
			if (skipNext)
			{
//				Debug.Log("Skip");
				currentEdge--;
				currentEdgeCount = 0;
				markList[i] = new Vector4(markList[i].x, markList[i].y, -1, -1);
//				markList[i].z = -1;
//				markList[i].w = -1;
				skipNext = false;
			}
			else if (currentEdge < 0)
			{
				skipNext = true;
				i++;
			}
			else
			{
//				if (currentEdge < 0) { Debug.Log("Break"); break; }
//				Debug.Log("Current edge is: " + edges[currentEdge]);
				markList[i] = new Vector4(markList[i].x, markList[i].y, edges[currentEdge].x, edges[currentEdge].y);
//				markList[i].z = edges[currentEdge].x;
//				markList[i].w = edges[currentEdge].y;
				if (++currentEdgeCount >= edges[currentEdge].x) skipNext = true;
			}
		}

		List<Vector2> definitelyFill = new List<Vector2>();
		for (int i = 0; i < markList.Count; i++)
		{
//			Debug.Log(markList[i]);
			if ((markList[i].x == markList[i].z) && (markList[i].y == markList[i].w) && (markList[i].x >= 1))
			{
//				Debug.Log("Double marked: (" + (row ? (i + "," + num) : (num + "," + (PicAssets.instance.h - i - 1))) + ")");
				if (row) definitelyFill.Add(new Vector2(i, num));
				else definitelyFill.Add(new Vector2(num, PicAssets.instance.h - i - 1));
			}
		}
//		for (int i = 0; i < definitelyFill.Count; i++) Debug.Log(definitelyFill[i]);
		return definitelyFill;
	}

	static List<Vector2> pushOffLeftOrTopWall(bool row, int num, List<Vector2> edges, int begin, int end)
//	static List<Vector2> pushOffWalls(bool row, int num, List<Vector2> edges, int begin, int end)
	{
		List<Vector4> markList = new List<Vector4>();

		int indexOfFirstFilled = -1;
		for (int i = (row ? 0 : PicAssets.instance.h - 1) ; (row ? (i < PicAssets.instance.w) : (i >= 0)) == true; i += row ? 1 : -1)
		{
			if (PicAssets.instance.tiles[row ? i : num][row ? num : i].GetComponent<SpriteRenderer>().sprite == PicAssets.instance.filledTexture)
			{
				indexOfFirstFilled = i;
				break;
			}
		}
		if (indexOfFirstFilled == -1) return new List<Vector2>();

		int leadingEmpties = (row ? indexOfFirstFilled - begin : end - indexOfFirstFilled - 1);

		int currentEdgeValue = (int)edges[0].x;
		int currentEdgeCount = 0;
		bool flagFound = false;
		for (int i = indexOfFirstFilled; row ? (i < PicAssets.instance.w) : (i >= 0); i += row ? 1 : -1)
		{
			if (PicAssets.instance.tiles[row ? i : num][row ? num : i].GetComponent<SpriteRenderer>().sprite == PicAssets.instance.flaggedTexture)
			{
				flagFound = true;
				break;
			}
			if (++currentEdgeCount <= currentEdgeValue) break;
		}
		if (flagFound) return new List<Vector2>();

		List<Vector2> definitelyFill = new List<Vector2>();
		currentEdgeCount = 0;
		for (int i = indexOfFirstFilled; row ? (i < PicAssets.instance.w) : (i >= 0); i += row ? 1 : -1)
//		for (int i = indexOfFirstFilled; row ? (i <= indexOfFirstFilled + (currentEdgeValue - leadingEmpties)) : (i > indexOfFirstFilled - currentEdgeValue + leadingEmpties + 1); i += row ? 1 : -1)
		{
//			Debug.Log("currentEdgeCount is:" + currentEdgeCount + "	currentEdgeValue:" + currentEdgeValue + "	leadingEmpties:" + leadingEmpties);
			Debug.Log("(" + (row ? (i + "," + num) : (num + "," + i)) + ")");

			if (row) definitelyFill.Add(new Vector2(i, num));
			else definitelyFill.Add(new Vector2(num, i));

			if (++currentEdgeCount >= currentEdgeValue - leadingEmpties) break;
		}

		return definitelyFill;

		return new List<Vector2>();
	}

	static List<Vector2> pushOffRightOrBottomWall(bool row, int num, List<Vector2> edges, int begin, int end)
//	static List<Vector2> pushOffWalls(bool row, int num, List<Vector2> edges, int begin, int end)
	{
		List<Vector4> markList = new List<Vector4>();

		int indexOfFirstFilled = -1;
		for (int i = (row ? PicAssets.instance.w - 1 : 0) ; (row ? (i >= 0) : (i < PicAssets.instance.h)) == true; i += row ? -1 : 1)
		{
			if (PicAssets.instance.tiles[row ? i : num][row ? num : i].GetComponent<SpriteRenderer>().sprite == PicAssets.instance.filledTexture)
			{
				indexOfFirstFilled = i;
				break;
			}
		}
		if (indexOfFirstFilled == -1) return new List<Vector2>();
//		Debug.Log("!!! (" + (row ? indexOfFirstFilled + "," + num : num + "," + indexOfFirstFilled) + ")");

//		int leadingEmpties = (row ? indexOfFirstFilled - begin : end - indexOfFirstFilled - 1);
		int leadingEmpties = (row ? end - indexOfFirstFilled - 1 : indexOfFirstFilled - begin);

		int currentEdgeValue = (int)edges[edges.Count - 1].x;
		int currentEdgeCount = 0;
		bool flagFound = false;
		for (int i = indexOfFirstFilled; row ? (i >= 0) : (i < PicAssets.instance.h) ; i += row ? -1 : 1)
		{
			if (PicAssets.instance.tiles[row ? i : num][row ? num : i].GetComponent<SpriteRenderer>().sprite == PicAssets.instance.flaggedTexture)
			{
				flagFound = true;
				break;
			}
			if (++currentEdgeCount <= currentEdgeValue) break;
		}
		if (flagFound) return new List<Vector2>();

		List<Vector2> definitelyFill = new List<Vector2>();
		currentEdgeCount = 0;
		for (int i = indexOfFirstFilled; row ? (i >= 0) : (i < PicAssets.instance.h) ; i += row ? -1 : 1)
//		for (int i = indexOfFirstFilled; row ? (i <= indexOfFirstFilled + (currentEdgeValue - leadingEmpties)) : (i > indexOfFirstFilled - currentEdgeValue + leadingEmpties + 1); i += row ? 1 : -1)
		{
//			Debug.Log("currentEdgeCount is:" + currentEdgeCount + "	currentEdgeValue:" + currentEdgeValue + "	leadingEmpties:" + leadingEmpties);
			Debug.Log("(" + (row ? (i + "," + num) : (num + "," + i)) + ")");

			if (row) definitelyFill.Add(new Vector2(i, num));
			else definitelyFill.Add(new Vector2(num, i));

			if (++currentEdgeCount >= currentEdgeValue - leadingEmpties) break;
		}

		return definitelyFill;

		return new List<Vector2>();
	}

	public static List<Vector2> operateOnRow(bool row, int num)
	{
		Dictionary<int, List<int>> duplicates = new Dictionary<int, List<int>>();
//		Dictionary<int, HashSet<int>> duplicates = new Dictionary<int, HashSet<int>>();
		for (int i = 0; i < (row ? PicAssets.instance.rows[num] : PicAssets.instance.columns[num]).Count; i++)
		{
			for (int j = i+1; j < (row ? PicAssets.instance.rows : PicAssets.instance.columns)[num].Count; j++)
			{
				if ((row ? PicAssets.instance.rows : PicAssets.instance.columns)[num][i] == (row ? PicAssets.instance.rows : PicAssets.instance.columns)[num][j])
				{
					if (duplicates.ContainsKey((row ? PicAssets.instance.rows : PicAssets.instance.columns)[num][i]))
					{
						if (duplicates[(row ? PicAssets.instance.rows : PicAssets.instance.columns)[num][i]].Contains(j) == false) duplicates[(row ? PicAssets.instance.rows : PicAssets.instance.columns)[num][i]].Add(j);
					}
					else duplicates[(row ? PicAssets.instance.rows : PicAssets.instance.columns)[num][i]] = new List<int>() { i, j };
//					else duplicates[PicAssets.instance.rows[num][i]] = new HashSet<int>() { i, j };
				}
			}
		}
		List<Vector2> edges = new List<Vector2>();
		for (int i = 0; i < (row ? PicAssets.instance.rows : PicAssets.instance.columns)[num].Count; i++)
		{
			if (duplicates.ContainsKey((row ? PicAssets.instance.rows : PicAssets.instance.columns)[num][i]))
			{
				duplicates[(row ? PicAssets.instance.rows : PicAssets.instance.columns)[num][i]].RemoveAt(0);
				edges.Add(new Vector2((row ? PicAssets.instance.rows : PicAssets.instance.columns)[num][i], duplicates[(row ? PicAssets.instance.rows : PicAssets.instance.columns)[num][i]].Count));
			}
			else edges.Add(new Vector2((row ? PicAssets.instance.rows : PicAssets.instance.columns)[num][i], 0));
		}
		Debug.Log("Start");
//		foreach (var Item in edges) Debug.Log(Item);
//		List<Vector2> retVal = superpositionOfExtremes(row, num, edges, 0, (row ? PicAssets.instance.w : PicAssets.instance.h));
///		PicAssets.instance.tiles[1][0].GetComponent<SpriteRenderer>().sprite = PicAssets.instance.filledTexture;
//		List<Vector2> retVal = pushOffLeftOrTopWall(row, num, edges, 0, (row ? PicAssets.instance.w : PicAssets.instance.h));
		List<Vector2> retVal = pushOffRightOrBottomWall(row, num, edges, 0, (row ? PicAssets.instance.w : PicAssets.instance.h));

		Debug.Log("End");

		return retVal;


///		superpositionOfExtremes(true,edges,0,PicAssets.instance.w);

//		for (int i = 0; i < PicAssets.instance.w; i++)
//		{
//
//		}
//		int index = 0;
//		for (int numIndex = 0; numIndex < PicAssets.instance.rows[rowNum].Count; numIndex++)
//		{
//			int numbersHit = 0;
//			bool hit = false;
//
//			for (; ; index++)
//			{
//				if (index >= PicAssets.instance.w)
//				{
//					if (numbersHit == PicAssets.instance.rows[rowNum][numIndex]) break;
//					else return false;
//				}
//
//				if (PicAssets.instance.tileObjects[index][rowNum].GetComponent<SpriteRenderer>().sprite == PicAssets.instance.filledTexture)
//				{
//					hit = true;
//					numbersHit++;
//				}
//				else
//				{
//					if (hit)
//					{
//						if (numbersHit == PicAssets.instance.rows[rowNum][numIndex])
//						{
//							if (numIndex + 1 != PicAssets.instance.rows[rowNum].Count) break;
//						}
//						else return false;
//					}
//				}
//			}
//		}
//
//		return true;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
