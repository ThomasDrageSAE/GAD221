using System;
using System.Collections.Generic;
using UnityEngine;

public class Minesweeper : DesktopWindow
{
    // Generate Grid Of Tile Prefab.
    // Determine Mine Tiles
    // Determine Other Tile Values
    
    // Complete Check
    // Loss Sequence
    // Timer
    // Mines Left
    // Need A Reset Button
    
    [SerializeField] SevenSegmentDisplayArray minesDisplay;
    [SerializeField] SevenSegmentDisplayArray timerDisplay;
    
    public List<MinesweeperTile> tiles = new List<MinesweeperTile>();
    
    void Start()
    {
        
    }

    private void OnDestroy()
    {
        
    }

    public void Generate()
    {
        InstantiateGrid();
        SpawnMines();
        DetermineTileValues();
        GenerationFinished();
    }

    public void InstantiateGrid()
    {
        
    }

    public void SpawnMines()
    {
        
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
