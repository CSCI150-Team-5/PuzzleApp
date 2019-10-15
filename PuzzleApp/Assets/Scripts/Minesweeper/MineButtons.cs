using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineButtons : MonoBehaviour
{
	public GameObject thisButton;
	public void select10x10()
    {
		thisButton.SetActive(false);

		MineAssets.instance.w = 10;
		MineAssets.instance.h = 10;
		MineAssets.instance.board = new MineBoard(w, h);
	}
}
