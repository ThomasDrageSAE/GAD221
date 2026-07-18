using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
        Generate(gridWidth, gridHeight, 10);
    }

    private void OnDestroy()
    {
        
    }

    public void Generate(int gridWidth, int gridHeight, int mineAmount)
    {
        GenerateGrid(gridWidth, gridHeight);
        SpawnMines(mineAmount);
        DetermineTileValues();
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
        Debug.Log("Grid Tile Spawned: " + column + ", " + row);
    }

    public MinesweeperTile GetGridTile(int column, int row)
    {
        return tileGrid[column, row];
    }
    
    public void SpawnMines(int amount)
    {
        Debug.Log("Spawning " + mineAmount + " Mines");
        //minesDisplay.SetValue(mineAmount);
    }

    public void DetermineTileValues()
    {
        
    }

    public void GenerationFinished()
    {
        // Start Timer
        // Set Mine Amount Counter
    }

    public void LoseGame()
    {
        
    }

    public void Reset()
    {
        
    }
}
