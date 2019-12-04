using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBoard : MonoBehaviour
{
	public MineBoard(int width, int height)	//The constructor of the board.
	{
		MineAssets.instance.tiles = new MineTile[width, height];	//We just allocate room for all the tiles.
	}

	public static void checkCompletion()	//This function checks if we met the win condition, which is that all mines are flagged. Note that we don't care if all non-mine tiles are filled in.
	{
		foreach (MineTile tile in MineAssets.instance.tiles) if (tile.mine && !tile.flagged) return;	//If even one mine is not flagged, then return because the game isn't won yet.

		//But if we reach the end of the loop without finding anything wrong,
		print("You won!");  //then we won!
//		MineAssets.instance.gameWon = true; //Set the game status to over.
		MineAssets.instance.gameJustWon = true; //Set the game status to over.
	}

	public static void loss(int x, int y)	//When we click on a mine, this function is called because we lost. We pass in the coordinates of the mine we clicked on.
	{
		foreach (MineTile tile in MineAssets.instance.tiles) if (tile.mine) tile.fill(-1);	//Reveal all the mines because we lost.

		MineAssets.instance.tiles[x, y].fill(-2);   //Make the mine we clicked on be a red mine, to highlight it separately from the other mines.
		MineAssets.instance.gameLost = true;		//Set the game to lost.
		print("You lost.");
	}

	public int totalFlags()  //This counts the total number of flagged tiles.
	{
		int count = 0;

		foreach (MineTile tile in MineAssets.instance.tiles) if (tile.flagged) count++;

		return count;
	}

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

	public bool fill(int x, int y)	//The function for filling a tile, has error checking if we lost in case the tile is a mine.
	{
		bool returnStatus = true;
		int mineCount = countMines(x, y);    //Find out how many mines are near this tile.
		if (MineAssets.instance.tiles[x, y].filled == false) returnStatus = !MineAssets.instance.tiles[x, y].fill(mineCount);   //Fill this tile if it's not already.
		if (returnStatus == false) loss(x, y);	//If we got that the tile was a mine, set ourselves to having lost.
		return returnStatus;	//And communicate to the parent that we lost.
	}

	public void popBubble(int x, int y, bool[,] visited)	//The algorithm to detect and pop bubbles.
	{
		if (visited[x, y]) return;	//If we already visited this tile, return so we don't waste time doing it a second time and risk an infinite loop.

		visited[x, y] = true;	//Set this tile to visited.

		int mineCount = countMines(x, y);    //Find out how many mines are near this tile.

		MineAssets.instance.board.fill(x, y);   //Fill this tile if it's not already.
		if (mineCount != 0) return; //If there are mines near this tile, that means we are either not in a bubble, or are at the edge of the bubble, so we're done here and can return.

		int w = MineAssets.instance.w;	//Storing the width and height for convenience.
		int h = MineAssets.instance.h;	//


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

	public void fillAndPop(int x, int y)	//Calls the fill function, then if we didn't lose, also pops the bubble IF there is a bubble.
	{
		if(fill(x, y)) popBubble(x, y, new bool[MineAssets.instance.w, MineAssets.instance.h]);
	}

	public void check(int x, int y)	//Function called from the Mine Tile class, checks the tile whose coordinates we pass in. Fills it if it can.
	{
		if (MineAssets.instance.tiles[x, y].flagged) return;	//If the tile is flagged, it returns because we're not allowed to fill it. Also prevents you from accidentally filling a flagged mine.
		if (MineAssets.instance.tiles[x, y].mine) { loss(x, y); return; }	//If we click on a mine, set ourselves to having loss and stop.
		if (!MineAssets.instance.tiles[x, y].filled && !MineAssets.instance.tiles[x, y].flagged)	//If the tile is ALREADY filled,
		{
			MineAssets.instance.board.fillAndPop(x, y);	//then just pop the bubble,
			return;										//and exit the function because the rest of the function corresponds only to unfilled tiles.
		}

		//This section of the function is if the tile was not flagged or filled, and isn't a mine.

		int w = MineAssets.instance.w;	//Storing the width locally for convenience.
		int h = MineAssets.instance.h;	//Storing the height locally for convenience.
		int mineCount = countMines(x, y);	//Counting how many mines are adjacent to the tile we're currently looking at.

		//The following code counts how many of the adjacent tiles are filled and flagged, for the bulk mode logic. We do not currently support a game mode without bulk mode logic.
		int adjacentUnfilled = 0;
		int adjacentFlagged = 0;
		bool bl = false, bm = false, br = false, ml = false, mr = false, tl = false, tm = false, tr = false;	//Bools for detecting if we're at an edge or corner tile. False by default. If the tile exists, it'll bet set to true. A tile doens't exist only if it'd be negative or over the maximum index.

		//Bound checking.
		bl = ((x - 1 >= 0) && (y - 1 >= 0));	//Bottom left
		bm = (y - 1 >= 0);						//Bottom middle
		br = ((x + 1 < w) && (y - 1 >= 0));		//Bottom right

		ml = (x - 1 >= 0);						//Middle left
		mr = (x + 1 < w);						//Middle right

		tl = ((x - 1 >= 0) && (y + 1 < h));		//Top left
		tm = (y + 1 < h);						//Top middle
		tr = ((x + 1 < w) && (y + 1 < h));		//Top right

		//At this point we record the current state of all the tiles around the tile we're looking at. We only look at the adjacent tiles that actually exist.
		if (bl)	{ if (!MineAssets.instance.tiles[x - 1, y - 1].filled	&& !MineAssets.instance.tiles[x - 1, y - 1].flagged)	adjacentUnfilled++;	if (MineAssets.instance.tiles[x - 1, y - 1].flagged)	adjacentFlagged++; }
		if (bm)	{ if (!MineAssets.instance.tiles[x, y - 1].filled		&& !MineAssets.instance.tiles[x, y - 1].flagged)		adjacentUnfilled++;	if (MineAssets.instance.tiles[x, y - 1].flagged)		adjacentFlagged++; }
		if (br) { if (!MineAssets.instance.tiles[x + 1, y - 1].filled	&& !MineAssets.instance.tiles[x + 1, y - 1].flagged)	adjacentUnfilled++;	if (MineAssets.instance.tiles[x + 1, y - 1].flagged)	adjacentFlagged++; }
                     
		if (ml) { if (!MineAssets.instance.tiles[x - 1, y].filled		&& !MineAssets.instance.tiles[x - 1, y].flagged)		adjacentUnfilled++;	if (MineAssets.instance.tiles[x - 1, y].flagged)		adjacentFlagged++; }
		if (mr) { if (!MineAssets.instance.tiles[x + 1, y].filled		&& !MineAssets.instance.tiles[x + 1, y].flagged)		adjacentUnfilled++;	if (MineAssets.instance.tiles[x + 1, y].flagged)		adjacentFlagged++; }
                     
		if (tl)	{ if (!MineAssets.instance.tiles[x - 1, y + 1].filled	&& !MineAssets.instance.tiles[x - 1, y + 1].flagged)	adjacentUnfilled++;	if (MineAssets.instance.tiles[x - 1, y + 1].flagged)	adjacentFlagged++; }
		if (tm)	{ if (!MineAssets.instance.tiles[x, y + 1].filled		&& !MineAssets.instance.tiles[x, y + 1].flagged)		adjacentUnfilled++;	if (MineAssets.instance.tiles[x, y + 1].flagged)		adjacentFlagged++; }
		if (tr)	{ if (!MineAssets.instance.tiles[x + 1, y + 1].filled	&& !MineAssets.instance.tiles[x + 1, y + 1].flagged)	adjacentUnfilled++;	if (MineAssets.instance.tiles[x + 1, y + 1].flagged)	adjacentFlagged++; }

		//If we can draw a conclusion that we flagged everything that needs to be flagged, we fill the rest.
		if ((mineCount - adjacentFlagged == 0))
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

		//If we can draw a conclusion that we filled everything that needs to be filled, we flag the rest.
		if (mineCount - adjacentFlagged == adjacentUnfilled)
		{
			if (bl) if (!MineAssets.instance.tiles[x - 1, y - 1].flagged)	{ MineAssets.instance.tiles[x - 1, y - 1].flag();	checkCompletion(); }
			if (bm) if (!MineAssets.instance.tiles[x, y - 1].flagged)		{ MineAssets.instance.tiles[x, y - 1].flag();		checkCompletion(); }
			if (br) if (!MineAssets.instance.tiles[x + 1, y - 1].flagged)	{ MineAssets.instance.tiles[x + 1, y - 1].flag();	checkCompletion(); }

			if (ml) if (!MineAssets.instance.tiles[x - 1, y].flagged)		{ MineAssets.instance.tiles[x - 1, y].flag();		checkCompletion(); }
			if (mr) if (!MineAssets.instance.tiles[x + 1, y].flagged)		{ MineAssets.instance.tiles[x + 1, y].flag();		checkCompletion(); }

			if (tl) if (!MineAssets.instance.tiles[x - 1, y + 1].flagged)	{ MineAssets.instance.tiles[x - 1, y + 1].flag();	checkCompletion(); }
			if (tm) if (!MineAssets.instance.tiles[x, y + 1].flagged)		{ MineAssets.instance.tiles[x, y + 1].flag();		checkCompletion(); }
			if (tr) if (!MineAssets.instance.tiles[x + 1, y + 1].flagged)	{ MineAssets.instance.tiles[x + 1, y + 1].flag();	checkCompletion(); }
		}
	}
}
