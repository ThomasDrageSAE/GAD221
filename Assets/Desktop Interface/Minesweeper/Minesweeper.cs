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
    private Vector2 gridBorderSize;
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

        for (int row = 0; row < gridHeight; row++) // Loop Rows
        {
            for (int column = 0; column < gridWidth; column++) // Loop Columns
            {
                // For Each Tile
                Vector2 spawnPos = Vector2.zero;

                // Offset that grows with each addition would solve issue.
                
                if (row != 0)
                {
                    
                }
                MinesweeperTile tile = Instantiate(tilePrefab, new Vector3(gridTilePadding + gridBorderWidth + column * 16, gridTilePadding + row * 16 , 0), Quaternion.identity, transform);
            }
        }
        
        SetGridBorderSize(new Vector2(gridWidth + (gridTilePadding + gridBorderWidth) * 2, gridHeight + (gridTilePadding + gridBorderWidth) * 2));
        gridBorder.enabled = true;
    }

    public void SetGridTile(int column, int row, MinesweeperTile tile)
    {
        tileGrid[column, row] = tile;
    }

    public MinesweeperTile GetGridTile(int column, int row)
    {
        return tileGrid[column, row];
    }

    public void SetGridBorderSize(Vector2 size)
    {
        gridBorderSize = size;
        gridBorder.rectTransform.sizeDelta = gridBorderSize;
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
