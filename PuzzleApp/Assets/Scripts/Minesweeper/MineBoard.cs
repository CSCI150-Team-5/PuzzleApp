using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBoard : MonoBehaviour
{
	public static bool firstClick = true;	//A bool that's true by default, indicating we haven't filled a tile yet. After we fill a tile, this is set to false. We use this to ensure the first click is always safe and not a mine.
	public static bool gameWon = false;		//A bool we use to represent if the game is won. If it is set to true, then we cannot fill or flag any more tiles.
	public static bool gameLost = false;    //A bool we use to represent if the game is lost. If it is set to true, then we cannot fill or flag any more tiles.

	public static bool bulkMode = true;//false;

	public static int w = 10;	//The width and height of the board.
	public static int h = 10;	//
	public static MineTile[,] tiles = new MineTile[w, h];	//A container of all the minesweeper tiles, their constructors place themselves in here. They are identified by their coordinates in the world.

	public static void revealAllMines()	//This function is for when we lose, all the mines are revealed.
	{
		foreach (MineTile tile in tiles) if (tile.mine) tile.fill(-1);	//For each mine on the board, we fill it. The value we pass in actually doesn't matter if the tile is a mine.
	}

	public static void loss(MineTile tile)
	{
		MineBoard.revealAllMines(); //Reveal all the mines because we lost.
		tile.fill(-2);				//Make the mine we clicked on be a red mine, to highlight it separately from the other mines.
		gameLost = true;			//Set the game to lost.
		print("You lost.");
	}

	public static int countMines(int x, int y)	//This function detects the number of adjacent mines, and returns the total.
	{
		int count = 0;

		//I first check if the adjacent tiles are off map, before I consider them.
		if ((x - 1 >= 0) && (y - 1 >= 0)) if (tiles[x - 1, y - 1].mine) count++;
		if (y - 1 >= 0) if (tiles[x, y - 1].mine) count++;
		if ((x + 1 < w) && (y - 1 >= 0)) if (tiles[x + 1, y - 1].mine) count++;

		if (x - 1 >= 0) if (tiles[x - 1, y].mine) count++;
		if (x + 1 < w) if (tiles[x + 1, y].mine) count++;

		if ((x - 1 >= 0) && (y + 1 < h)) if (tiles[x - 1, y + 1].mine) count++;
		if (y + 1 < h) if (tiles[x, y + 1].mine) count++;
		if ((x + 1 < w) && (y + 1 < h)) if (tiles[x + 1, y + 1].mine) count++;

		return count;
	}

	public static int totalMines()  //This counts the total number of mines.
	{
		int count = 0;

		foreach (MineTile tile in tiles) if (tile.mine) count++;

		return count;
	}

	public static int totalFlags()  //This counts the total number of flagged tiles.
	{
		int count = 0;

		foreach (MineTile tile in tiles) if (tile.flagged) count++;

		return count;
	}

/*	public static int totalMinesLeft()  //This counts the number of mines left unflagged.
	{
		int count = 0;

		foreach (MineTile tile in tiles) if ((tile.mine) && (!tile.flagged)) count++;

		return count;
	}
*/

	public static void bulkCheck(int x, int y, int mineCount)
	{
		if (tiles[x, y].mine) loss(tiles[x, y]);
		else if (tiles[x,y].filled == false && tiles[x,y].flagged == false)
		{
			popBubble(x, y, new bool[MineBoard.w, MineBoard.h]);
			return;
		}

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

		if (bl)	{ if (!tiles[x - 1, y - 1].filled	&& !tiles[x - 1, y - 1].flagged)	adjacentUnfilled++;	if (tiles[x - 1, y - 1].flagged)	adjacentFlagged++; }
		if (bm)	{ if (!tiles[x, y - 1].filled		&& !tiles[x, y - 1].flagged)		adjacentUnfilled++;	if (tiles[x, y - 1].flagged)		adjacentFlagged++; }
		if (br) { if (!tiles[x + 1, y - 1].filled	&& !tiles[x + 1, y - 1].flagged)	adjacentUnfilled++;	if (tiles[x + 1, y - 1].flagged)	adjacentFlagged++; }
                     
		if (ml) { if (!tiles[x - 1, y].filled		&& !tiles[x - 1, y].flagged)		adjacentUnfilled++;	if (tiles[x - 1, y].flagged)		adjacentFlagged++; }
		if (mr) { if (!tiles[x + 1, y].filled		&& !tiles[x + 1, y].flagged)		adjacentUnfilled++;	if (tiles[x + 1, y].flagged)		adjacentFlagged++; }
                     
		if (tl)	{ if (!tiles[x - 1, y + 1].filled	&& !tiles[x - 1, y + 1].flagged)	adjacentUnfilled++;	if (tiles[x - 1, y + 1].flagged)	adjacentFlagged++; }
		if (tm)	{ if (!tiles[x, y + 1].filled		&& !tiles[x, y + 1].flagged)		adjacentUnfilled++;	if (tiles[x, y + 1].flagged)		adjacentFlagged++; }
		if (tr)	{ if (!tiles[x + 1, y + 1].filled	&& !tiles[x + 1, y + 1].flagged)	adjacentUnfilled++;	if (tiles[x + 1, y + 1].flagged)	adjacentFlagged++; }

		if ((mineCount - adjacentFlagged == 0))// && (adjacentUnfilled > 0)) //true)//ajacentUnfilled + ajacentFlagged == mineCount)
		{
			if (bl) if (!tiles[x - 1, y - 1].filled	&& !tiles[x - 1, y - 1].flagged)	{ if(tiles[x - 1, y - 1].mine)	loss(tiles[x - 1, y - 1]);		else tiles[x - 1, y - 1].fill(countMines(x-1, y - 1));	}
			if (bm) if (!tiles[x, y - 1].filled		&& !tiles[x, y - 1].flagged)		{ if(tiles[x, y - 1].mine)		loss(tiles[x, y - 1]);			else tiles[x, y - 1].fill(countMines(x, y - 1));		}
			if (br) if (!tiles[x + 1, y - 1].filled	&& !tiles[x + 1, y - 1].flagged)	{ if(tiles[x + 1, y - 1].mine)	loss(tiles[x + 1, y - 1]);		else tiles[x + 1, y - 1].fill(countMines(x+1, y-1));	}

			if (ml) if (!tiles[x - 1, y].filled		&& !tiles[x - 1, y].flagged)		{ if(tiles[x - 1, y].mine)		loss(tiles[x - 1, y]);			else tiles[x - 1, y].fill(countMines(x-1, y));			}
			if (mr) if (!tiles[x + 1, y].filled		&& !tiles[x + 1, y].flagged)		{ if(tiles[x + 1, y].mine)		loss(tiles[x + 1, y]);			else tiles[x + 1, y].fill(countMines(x+1, y));			}

			if (tl) if (!tiles[x - 1, y + 1].filled	&& !tiles[x - 1, y + 1].flagged)	{ if(tiles[x - 1, y + 1].mine)	loss(tiles[x - 1, y + 1]);		else tiles[x - 1, y + 1].fill(countMines(x-1, y + 1));	}
			if (tm) if (!tiles[x, y + 1].filled		&& !tiles[x, y + 1].flagged)		{ if(tiles[x, y + 1].mine)		loss(tiles[x, y + 1]);			else tiles[x, y + 1].fill(countMines(x, y + 1));		}
			if (tr) if (!tiles[x + 1, y + 1].filled	&& !tiles[x + 1, y + 1].flagged)	{ if(tiles[x + 1, y + 1].mine)	loss(tiles[x + 1, y + 1]);		else tiles[x + 1, y + 1].fill(countMines(x+1, y+1));	}
		}

		if (mineCount - adjacentFlagged == adjacentUnfilled)
		{
			if (bl) if (!tiles[x - 1, y - 1].flagged)	tiles[x - 1, y - 1].flag();
			if (bm) if (!tiles[x, y - 1].flagged)		tiles[x, y - 1].flag();
			if (br) if (!tiles[x + 1, y - 1].flagged)	tiles[x + 1, y - 1].flag();

			if (ml) if (!tiles[x - 1, y].flagged)		tiles[x - 1, y].flag();
			if (mr) if (!tiles[x + 1, y].flagged)		tiles[x + 1, y].flag();

			if (tl) if (!tiles[x - 1, y + 1].flagged)	tiles[x - 1, y + 1].flag();
			if (tm) if (!tiles[x, y + 1].flagged)		tiles[x, y + 1].flag();
			if (tr) if (!tiles[x + 1, y + 1].flagged)	tiles[x + 1, y + 1].flag();
		}

	}

	public static void popBubble(int x, int y, bool[,] visited)	//The algorithm to detect and pop bubbles.
	{
		if (visited[x, y]) return;	//If we already visited this tile, return so we don't waste time doing it a second time.

		visited[x, y] = true;	//Set this tile to visited.

		int mineCount = countMines(x, y);    //Find out how many mines are near this tile.
		if (tiles[x, y].filled == false) tiles[x, y].fill(mineCount);   //Fill this tile if it's not already.
//		if (bulkMode) bulkCheck(x, y, mineCount);
		if (mineCount != 0) return;	//If there are mines near this tile, that means we are either not in a bubble, or are at the edge of the bubble, so we're done here and can return.


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

	public static void popEmpties() //The algorithm to detect and pop bubbles.
	{
		foreach (MineTile tile in tiles) if(tile.filled) if (countMines(tile.xpos,tile.ypos) == 0) popBubble(tile.xpos, tile.ypos, new bool[MineBoard.w, MineBoard.h]);
	}

	public static void checkCompletion()	//This function checks if we met the win condition, which is that all mines are flagged. Note that we don't care if all non-mine tiles are filled in.
	{
		foreach (MineTile tile in tiles) if (tile.mine && !tile.flagged) return;	//If even one mine is not flagged, then return because the game isn't won yet.
		//But if we reach the end of the loop without finding anything wrong,
		print("You won!");	//then we won!
		gameWon = true;	//Set the game status to over.
	}
}
