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

	void superpositionOfExtremes(List<Vector2> edges, int begin, int end)
	{

	}

	public static void operateOnRow(int rowNum)
	{
		Dictionary<int, List<int>> duplicates = new Dictionary<int, List<int>>();
//		Dictionary<int, HashSet<int>> duplicates = new Dictionary<int, HashSet<int>>();
		for (int i = 0; i < PicAssets.instance.rows[rowNum].Count; i++)
		{
			for (int j = i+1; j < PicAssets.instance.rows[rowNum].Count; j++)
			{
				if (PicAssets.instance.rows[rowNum][i] == PicAssets.instance.rows[rowNum][j])
				{
					if (duplicates.ContainsKey(PicAssets.instance.rows[rowNum][i]))
					{
						if (duplicates[PicAssets.instance.rows[rowNum][i]].Contains(j) == false) duplicates[PicAssets.instance.rows[rowNum][i]].Add(j);
					}
					else duplicates[PicAssets.instance.rows[rowNum][i]] = new List<int>() { i, j };
//					else duplicates[PicAssets.instance.rows[rowNum][i]] = new HashSet<int>() { i, j };
				}
			}
		}
		List<Vector2> edges = new List<Vector2>();
		for (int i = 0; i < PicAssets.instance.rows[rowNum].Count; i++)
		{
			if (duplicates.ContainsKey(PicAssets.instance.rows[rowNum][i]))
			{
				duplicates[PicAssets.instance.rows[rowNum][i]].RemoveAt(0);
				edges.Add(new Vector2(PicAssets.instance.rows[rowNum][i], duplicates[PicAssets.instance.rows[rowNum][i]].Count));
			}
			else edges.Add(new Vector2(PicAssets.instance.rows[rowNum][i], 0));
		}
		foreach (var Item in edges) Debug.Log(Item);
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
