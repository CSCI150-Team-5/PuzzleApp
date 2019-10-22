using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBoard : MonoBehaviour
{
//	MineFunctionality func;

//	public MineBoard(MineFunctionality f, int width, int height)
	public MineBoard(int width, int height)
	{
//		func = f;
		MineAssets.instance.tiles = new MineTile[width, height];
	}

//	public static void revealAllMines()	//This function is for when we lose, all the mines are revealed.
//	{
//			//For each mine on the board, we fill it. The value we pass in actually doesn't matter if the tile is a mine.
//	}

//	public static void loss(MineTile tile)
	public static void loss(int x, int y)
	{
		foreach (MineTile tile in MineAssets.instance.tiles) if (tile.mine) tile.fill(-1);	//Reveal all the mines because we lost.
//		tile.fill(-2);                          //Make the mine we clicked on be a red mine, to highlight it separately from the other mines.
		MineAssets.instance.tiles[x, y].fill(-2);
		MineAssets.instance.gameLost = true;	//Set the game to lost.
		print("You lost.");
	}

//	public static int totalFlags()  //This counts the total number of flagged tiles.
	public int totalFlags()  //This counts the total number of flagged tiles.
	{
		int count = 0;

		foreach (MineTile tile in MineAssets.instance.tiles) if (tile.flagged) count++;

		return count;
	}

//	public static int totalMines()  //This counts the total number of mine tiles.
	public int totalMines()  //This counts the total number of mine tiles.
	{
		int count = 0;

		foreach (MineTile tile in MineAssets.instance.tiles) if (tile.mine) count++;

		return count;
	}

	public int totalUnflaggedMines()  //This counts the total number of mine tiles minus the total number flagged tiles. The flagged tile need not be the same tile as the mine.
	{
		int count = 0;

		foreach (MineTile tile in MineAssets.instance.tiles)
		{
			if (tile.mine) count++;
			if (tile.flagged) count--;
		}

		return count;
	}

	public static int countMines(int x, int y)  //This function detects the number of adjacent mines, and returns the total.
	{
		int count = 0;

		int w = MineAssets.instance.w;
		int h = MineAssets.instance.h;

		//I first check if the adjacent tiles are off map, before I consider them.
		if ((x - 1 >= 0) && (y - 1 >= 0)) if (MineAssets.instance.tiles[x - 1, y - 1].mine) count++;
		if (y - 1 >= 0) if (MineAssets.instance.tiles[x, y - 1].mine) count++;
		if ((x + 1 < w) && (y - 1 >= 0)) if (MineAssets.instance.tiles[x + 1, y - 1].mine) count++;

		if (x - 1 >= 0) if (MineAssets.instance.tiles[x - 1, y].mine) count++;
		if (x + 1 < w) if (MineAssets.instance.tiles[x + 1, y].mine) count++;

		if ((x - 1 >= 0) && (y + 1 < h)) if (MineAssets.instance.tiles[x - 1, y + 1].mine) count++;
		if (y + 1 < h) if (MineAssets.instance.tiles[x, y + 1].mine) count++;
		if ((x + 1 < w) && (y + 1 < h)) if (MineAssets.instance.tiles[x + 1, y + 1].mine) count++;

		return count;
	}

	public bool fill(int x, int y)
	{
		bool returnStatus = true;
		int mineCount = countMines(x, y);    //Find out how many mines are near this tile.
		if (MineAssets.instance.tiles[x, y].filled == false) returnStatus = !MineAssets.instance.tiles[x, y].fill(mineCount);   //Fill this tile if it's not already.
		if (returnStatus == false) loss(x, y);
		return returnStatus;

//		Debug.Log("TEST" + mineCount);
	}

	public void popBubble(int x, int y, bool[,] visited)	//The algorithm to detect and pop bubbles.
	{
		if (visited[x, y]) return;	//If we already visited this tile, return so we don't waste time doing it a second time and risk an infinite loop.

		visited[x, y] = true;	//Set this tile to visited.

		int mineCount = countMines(x, y);    //Find out how many mines are near this tile.
//		if (tiles[x, y].filled == false) tiles[x, y].fill(mineCount);   //Fill this tile if it's not already.
//		if (bulkMode) bulkCheck(x, y, mineCount);
		MineAssets.instance.board.fill(x, y);
		if (mineCount != 0) return; //If there are mines near this tile, that means we are either not in a bubble, or are at the edge of the bubble, so we're done here and can return.

		int w = MineAssets.instance.w;
		int h = MineAssets.instance.h;


		//Here I check all 8 adjacent squares. If they are on the board and not off-board,
		//then we recursively call popBubble on the adjacents.
		if ((x - 1 >= 0) && (y - 1 >= 0)) popBubble(x - 1, y - 1, visited);
		if (y - 1 >= 0) popBubble(x, y - 1, visited);
		if ((x + 1 < w) && (y - 1 >= 0)) popBubble(x + 1, y - 1, visited);

		if (x - 1 >= 0) popBubble(x - 1, y, visited);
		if (x + 1 < w) popBubble(x + 1, y, visited);

		if ((x - 1 >= 0) && (y + 1 < h)) popBubble(x - 1, y + 1, visited);
		if (y + 1 < h) popBubble(x, y + 1, visited);
		if ((x + 1 < w) && (y + 1 < h)) popBubble(x + 1, y + 1, visited);
	}

	public void fillAndPop(int x, int y)
	{
		if(fill(x, y)) popBubble(x, y, new bool[MineAssets.instance.w, MineAssets.instance.h]);
	}

	public void check(int x, int y)
	{
		if (MineAssets.instance.tiles[x, y].flagged) return;
		if (MineAssets.instance.tiles[x, y].mine) { loss(x, y); return; }//Debug.Log("You lost."); //loss(MineAssets.instance.tiles[x, y]);
		if (!MineAssets.instance.tiles[x, y].filled && !MineAssets.instance.tiles[x, y].flagged)
		{
//			MineAssets.instance.board.fill(x, y);
//			MineAssets.instance.board.popBubble(x, y, new bool[MineAssets.instance.w, MineAssets.instance.h]);
			MineAssets.instance.board.fillAndPop(x, y);
			return;
		}

		if (MineAssets.instance.bulkMode == false) { Debug.Log("We are not in bulk mode."); return; }

		int w = MineAssets.instance.w;
		int h = MineAssets.instance.h;
		int mineCount = countMines(x, y);

		int adjacentUnfilled = 0;
		int adjacentFlagged = 0;
		bool bl = false, bm = false, br = false, ml = false, mr = false, tl = false, tm = false, tr = false;

		bl = ((x - 1 >= 0) && (y - 1 >= 0));
		bm = (y - 1 >= 0);
		br = ((x + 1 < w) && (y - 1 >= 0));

		ml = (x - 1 >= 0);
		mr = (x + 1 < w);

		tl = ((x - 1 >= 0) && (y + 1 < h));
		tm = (y + 1 < h);
		tr = ((x + 1 < w) && (y + 1 < h));

		if (bl)	{ if (!MineAssets.instance.tiles[x - 1, y - 1].filled	&& !MineAssets.instance.tiles[x - 1, y - 1].flagged)	adjacentUnfilled++;	if (MineAssets.instance.tiles[x - 1, y - 1].flagged)	adjacentFlagged++; }
		if (bm)	{ if (!MineAssets.instance.tiles[x, y - 1].filled		&& !MineAssets.instance.tiles[x, y - 1].flagged)		adjacentUnfilled++;	if (MineAssets.instance.tiles[x, y - 1].flagged)		adjacentFlagged++; }
		if (br) { if (!MineAssets.instance.tiles[x + 1, y - 1].filled	&& !MineAssets.instance.tiles[x + 1, y - 1].flagged)	adjacentUnfilled++;	if (MineAssets.instance.tiles[x + 1, y - 1].flagged)	adjacentFlagged++; }
                     
		if (ml) { if (!MineAssets.instance.tiles[x - 1, y].filled		&& !MineAssets.instance.tiles[x - 1, y].flagged)		adjacentUnfilled++;	if (MineAssets.instance.tiles[x - 1, y].flagged)		adjacentFlagged++; }
		if (mr) { if (!MineAssets.instance.tiles[x + 1, y].filled		&& !MineAssets.instance.tiles[x + 1, y].flagged)		adjacentUnfilled++;	if (MineAssets.instance.tiles[x + 1, y].flagged)		adjacentFlagged++; }
                     
		if (tl)	{ if (!MineAssets.instance.tiles[x - 1, y + 1].filled	&& !MineAssets.instance.tiles[x - 1, y + 1].flagged)	adjacentUnfilled++;	if (MineAssets.instance.tiles[x - 1, y + 1].flagged)	adjacentFlagged++; }
		if (tm)	{ if (!MineAssets.instance.tiles[x, y + 1].filled		&& !MineAssets.instance.tiles[x, y + 1].flagged)		adjacentUnfilled++;	if (MineAssets.instance.tiles[x, y + 1].flagged)		adjacentFlagged++; }
		if (tr)	{ if (!MineAssets.instance.tiles[x + 1, y + 1].filled	&& !MineAssets.instance.tiles[x + 1, y + 1].flagged)	adjacentUnfilled++;	if (MineAssets.instance.tiles[x + 1, y + 1].flagged)	adjacentFlagged++; }

		if ((mineCount - adjacentFlagged == 0))// && (adjacentUnfilled > 0)) //true)//ajacentUnfilled + ajacentFlagged == mineCount)
		{
			if (bl) if (!MineAssets.instance.tiles[x - 1, y - 1].filled	&& !MineAssets.instance.tiles[x - 1, y - 1].flagged)	{ if(MineAssets.instance.tiles[x - 1, y - 1].mine)	loss(x - 1, y - 1);		else fillAndPop(x - 1, y - 1);	}
			if (bm) if (!MineAssets.instance.tiles[x, y - 1].filled		&& !MineAssets.instance.tiles[x, y - 1].flagged)		{ if(MineAssets.instance.tiles[x, y - 1].mine)		loss(x, y - 1);			else fillAndPop(x, y - 1);		}
			if (br) if (!MineAssets.instance.tiles[x + 1, y - 1].filled	&& !MineAssets.instance.tiles[x + 1, y - 1].flagged)	{ if(MineAssets.instance.tiles[x + 1, y - 1].mine)	loss(x + 1, y - 1);		else fillAndPop(x + 1, y - 1);	}

			if (ml) if (!MineAssets.instance.tiles[x - 1, y].filled		&& !MineAssets.instance.tiles[x - 1, y].flagged)		{ if(MineAssets.instance.tiles[x - 1, y].mine)		loss(x - 1, y);			else fillAndPop(x - 1, y);		}
			if (mr) if (!MineAssets.instance.tiles[x + 1, y].filled		&& !MineAssets.instance.tiles[x + 1, y].flagged)		{ if(MineAssets.instance.tiles[x + 1, y].mine)		loss(x + 1, y);			else fillAndPop(x + 1, y);		}

			if (tl) if (!MineAssets.instance.tiles[x - 1, y + 1].filled	&& !MineAssets.instance.tiles[x - 1, y + 1].flagged)	{ if(MineAssets.instance.tiles[x - 1, y + 1].mine)	loss(x - 1, y + 1);		else fillAndPop(x - 1, y + 1);	}
			if (tm) if (!MineAssets.instance.tiles[x, y + 1].filled		&& !MineAssets.instance.tiles[x, y + 1].flagged)		{ if(MineAssets.instance.tiles[x, y + 1].mine)		loss(x, y + 1);			else fillAndPop(x, y + 1);		}
			if (tr) if (!MineAssets.instance.tiles[x + 1, y + 1].filled	&& !MineAssets.instance.tiles[x + 1, y + 1].flagged)	{ if(MineAssets.instance.tiles[x + 1, y + 1].mine)	loss(x + 1, y + 1);		else fillAndPop(x + 1, y + 1);	}
		}

		if (mineCount - adjacentFlagged == adjacentUnfilled)
		{
			if (bl) if (!MineAssets.instance.tiles[x - 1, y - 1].flagged)	MineAssets.instance.tiles[x - 1, y - 1].flag();
			if (bm) if (!MineAssets.instance.tiles[x, y - 1].flagged)		MineAssets.instance.tiles[x, y - 1].flag();
			if (br) if (!MineAssets.instance.tiles[x + 1, y - 1].flagged)	MineAssets.instance.tiles[x + 1, y - 1].flag();

			if (ml) if (!MineAssets.instance.tiles[x - 1, y].flagged)		MineAssets.instance.tiles[x - 1, y].flag();
			if (mr) if (!MineAssets.instance.tiles[x + 1, y].flagged)		MineAssets.instance.tiles[x + 1, y].flag();

			if (tl) if (!MineAssets.instance.tiles[x - 1, y + 1].flagged)	MineAssets.instance.tiles[x - 1, y + 1].flag();
			if (tm) if (!MineAssets.instance.tiles[x, y + 1].flagged)		MineAssets.instance.tiles[x, y + 1].flag();
			if (tr) if (!MineAssets.instance.tiles[x + 1, y + 1].flagged)	MineAssets.instance.tiles[x + 1, y + 1].flag();
		}
	}
}
