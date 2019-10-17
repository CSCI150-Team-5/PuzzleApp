using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineFunctionality : MonoBehaviour
{
	public	 GameObject[] menuObjects = new GameObject[2];
//	public	 GameObject button10x10;

	public void deactivateMenu()
	{
		for (int i = 0; i < 2; i++) menuObjects[i].SetActive(false);
//		button10x10.SetActive(false);
//		selectDifficultyText.SetActive(false);
	}

	public void generateBoard()
	{
		Screen.orientation = ScreenOrientation.Portrait;
		int greater;
		if (MineAssets.instance.w > MineAssets.instance.h) greater = MineAssets.instance.w;
		else greater = MineAssets.instance.h;
		MineAssets.instance.cam.orthographicSize = greater;// MineAssets.instance.h;
		MineAssets.instance.cam.transform.position = new Vector3(((float)MineAssets.instance.w / 2.0f) - 0.5f, ((float)MineAssets.instance.h / 2.0f) - 0.5f, -10);
//		Camera.current.transform.Translate(new Vector3(1.0f, 1.0f, 1.0f));

		for (int i = 0; i < MineAssets.instance.w; i++)
		{
			for (int j = 0; j < MineAssets.instance.h; j++)
			{
				GameObject tile = new GameObject("Tile (" + i + "," + j + ")", typeof(SpriteRenderer));
				tile.GetComponent<SpriteRenderer>().sprite = MineAssets.instance.unfilledTexture;
				tile.transform.position = new Vector3(i, j);
				tile.AddComponent<MineTile>();
				tile.AddComponent<BoxCollider2D>();
			}
		}
	}

	public void select10x10()
	{
		deactivateMenu();

		MineAssets.instance.w = 10;
		MineAssets.instance.h = 10;
		MineAssets.instance.board = new MineBoard(10, 10);

		generateBoard();
	}
}
