using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBoard : MonoBehaviour
{
	public MineBoard(int width, int height)
	{
		MineAssets.instance.tiles = new MineTile[width, height];
//		Debug.Log("Testing 123");

	}
}
