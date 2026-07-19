using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Minesweeper : DesktopWindow
{
    // Generate Grid Of Tile Prefab.
    [SerializeField] MinesweeperTile tilePrefab;
    
    // Determine Mine Tiles
    // Determine Other Tile Values
    
    // Complete Check
    // Loss Sequence
    // Timer
    // Mines Left
    // Need A Reset Button
    
    [SerializeField] SevenSegmentDisplayArray minesDisplay;
    [SerializeField] SevenSegmentDisplayArray timerDisplay;
    [SerializeField] Image gridBorder;
    
    // Should probably be a 2D array
    public int mineAmount;
    public int gridWidth;
    public int gridHeight;

    public int gridTilePadding;
    public int gridBorderWidth;
    private Vector2 windowSize;

    public MinesweeperTile[,] tileGrid;
    
    void Start()
    {
        MinesweeperTile.onActivate.AddListener(TileActivated);
        MinesweeperTile.onMark.AddListener(TileMarked);
        Generate(gridWidth, gridHeight, 10);
    }

    private void OnDestroy()
    {
        MinesweeperTile.onActivate.RemoveListener(TileActivated);
        MinesweeperTile.onMark.RemoveListener(TileMarked);
    }

    public void Generate(int gridWidth, int gridHeight, int mineAmount)
    {
        GenerateGrid(gridWidth, gridHeight);
        List<Vector2> minePositions = SpawnMines(mineAmount);
        DetermineTileValues(minePositions);
        GenerationFinished();
    }

    public void GenerateGrid(int gridWidth, int gridHeight)
    {
        tileGrid = new MinesweeperTile[gridWidth, gridHeight];

        Vector2 spawnPos = new Vector2(gridTilePadding + gridBorderWidth, -gridTilePadding + -gridBorderWidth);
        //Vector2 spawnPos = new Vector2(spawnPos.x = (gridTilePadding + gridBorderWidth) * 2, spawnPos.y = (-gridTilePadding + -gridBorderWidth) * 2);
        int row = 0;
        int column = 0;
        
        for ( ; row < gridHeight; row++) // Loop Rows
        {
            if (row != 0)
            {
                spawnPos.y += -16 + -gridTilePadding;
            }
            
            for ( ; column < gridWidth; column++) // Loop Columns
            {
                if (column != 0)
                {
                    spawnPos.x += 16 + gridTilePadding;
                }
                
                SpawnGridTile(column, row, spawnPos);
            }
            
            column = 0;
            spawnPos.x = gridTilePadding + gridBorderWidth;
        }
    }

    public void SpawnGridTile(int column, int row, Vector2 position)
    {
        MinesweeperTile tile = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity, gridBorder.transform);
        tile.transform.SetLocalPositionAndRotation(new Vector3(position.x, position.y, 0), Quaternion.identity);
        tileGrid[column, row] = tile;
        tile.Initialize(new Vector2(column, row));
        Debug.Log("Grid Tile Spawned: " + column + ", " + row);
    }

    public MinesweeperTile GetGridTile(int column, int row)
    {
        return tileGrid[column, row];
    }
    
    public List<Vector2> SpawnMines(int amount)
    {
        Debug.Log("Spawning " + mineAmount + " Mines");
        List<Vector2> minePositions = new List<Vector2>();

        for (int i = 0; i < amount; )
        {
            int column = Random.Range(0, gridWidth);
            int row = Random.Range(0, gridHeight);
            MinesweeperTile tile = tileGrid[column, row];

            if (tile.GetTileValue() != 0)
            {
                continue;
            }

            tile.SetTileValue(9);
            minePositions.Add(new Vector2(column, row));
            Debug.Log("Mine Spawned: " + column + ", " + row);
            i++;
        }
        
        minesDisplay.SetValue(mineAmount);
        return minePositions;
    }

    public void DetermineTileValues(List<Vector2> minePositions)
    {
        Debug.Log("Determining Tile Values");
        
        foreach (Vector2 minePosition in minePositions)
        {
            List<MinesweeperTile> surroundingTiles = GetSurroundingTiles(minePosition);
            Debug.Log("Mine (" + minePosition + ") has " + surroundingTiles.Count + " surrounding tiles");
            
            foreach (MinesweeperTile tile in surroundingTiles)
            {
                int tileValue = tile.GetTileValue();
                if (tileValue < 8)
                {
                    tileValue++;
                    tile.SetTileValue(tileValue);
                }
            }
        }
    }

    public List<MinesweeperTile> GetSurroundingTiles(Vector2 originTilePos)
    {
        Debug.Log("Getting Surrounding Tiles Of " + originTilePos);
        List<MinesweeperTile> surroundingTiles = new List<MinesweeperTile>();
        for (int y = (int)originTilePos.y - 1; y < originTilePos.y + 2; y++)
        {
            for (int x = (int)originTilePos.x - 1; x < originTilePos.x + 2; x++)
            {
                if (x < 0 || x > gridWidth - 1 || y < 0 || y > gridHeight - 1)
                {
                    continue;
                }
                
                MinesweeperTile tile = tileGrid[x, y];
                
                if (tile.IsBomb())
                {
                    continue;
                }
                
                surroundingTiles.Add(tile);
            }
        }
        
        return surroundingTiles;
    }

    public void GenerationFinished()
    {
        // Start Timer
        // Set Mine Amount Counter
    }

    public void TileMarked(MinesweeperTile tile)
    {
        
    }

    public void TileActivated(MinesweeperTile tile)
    {
        if (tile.IsBomb())
        {
            LoseGame();
        }

        if (tile.IsEmpty())
        {
            ActivateSurrounding(tile);
        }
    }

    public void ActivateSurrounding(MinesweeperTile originTile)
    {
        List<MinesweeperTile> surroundingTiles = GetSurroundingTiles(originTile.GetGridPos());
        
        foreach (MinesweeperTile tile in surroundingTiles)
        {
            tile.Activate();
        }
    }

    public void LoseGame()
    {
        
    }

    public void WinGame()
    {
        
    }

    public void ResetGame()
    {
        
    }
}
