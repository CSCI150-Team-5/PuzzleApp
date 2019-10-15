using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBoard : MonoBehaviour
{
<<<<<<< Updated upstream
	public static bool firstClick = true;	//A bool that's true by default, indicating we haven't filled a tile yet. After we fill a tile, this is set to false. We use this to ensure the first click is always safe and not a mine.
	public static bool gameWon = false;		//A bool we use to represent if the game is won. If it is set to true, then we cannot fill or flag any more tiles.
	public static bool gameLost = false;	//A bool we use to represent if the game is lost. If it is set to true, then we cannot fill or flag any more tiles.

	public static int w = 10;	//The width and height of the board.
	public static int h = 10;	//
	public static MineTile[,] tiles = new MineTile[w, h];	//A container of all the minesweeper tiles, their constructors place themselves in here. They are identified by their coordinates in the world.

	public static void revealAllMines()	//This function is for when we lose, all the mines are revealed.
	{
		foreach (MineTile tile in tiles) if (tile.mine) tile.fill(-1);	//For each mine on the board, we fill it. The value we pass in actually doesn't matter if the tile is a mine.
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

	public static int totalMinesLeft()	//This counts the number of mines left unflagged.
	{
		int count = 0;

		foreach (MineTile tile in tiles) if ((tile.mine) && (!tile.flagged)) count++;

		return count;
	}

	public static void popBubble(int x, int y, bool[,] visited)	//The algorithm to detect and pop bubbles.
	{
		if (visited[x, y]) return;	//If we already visited this tile, return so we don't waste time doing it a second time.

		visited[x, y] = true;	//Set this tile to visited.

		int mineCount = countMines(x, y);    //Find out how many mines are near this tile.
		if (tiles[x, y].filled == false) tiles[x, y].fill(mineCount);	//Fill this tile if it's not already.
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

	public static void checkCompletion()	//This function checks if we met the win condition, which is that all mines are flagged. Note that we don't care if all non-mine tiles are filled in.
=======
	MineBoard(int width, int height)
>>>>>>> Stashed changes
	{
		MineAssets.instance.tiles = new MineTile[width, height];
	}
}
