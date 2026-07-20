using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Minesweeper : DesktopWindow
{
    #region --- Inspector References ---
    
    // -- In Scene --
    [SerializeField] SevenSegmentDisplayArray minesDisplay;
    [SerializeField] SevenSegmentDisplayArray timerDisplay;
    
    [SerializeField] Image gridBorder;
    [SerializeField] Image buttonImage;
    
    // -- Resources & Prefabs --
    [SerializeField] MinesweeperTile tilePrefab;
    
    [SerializeField] Sprite buttonPlay;
    [SerializeField] Sprite buttonRestart;
    
    #endregion
    
    #region --- Properties & Variables ---
    
    // -- Public --
    public int totalMines;
    public int gridWidth;
    public int gridHeight;
    public int gridTilePadding;
    public int gridBorderWidth;
    
    public enum TimerType
    {
        Increment, Decrement
    }
    
    // -- Private --
    private MinesweeperTile[,] tileGrid;
    
    private Vector2 windowSize;
    
    private int totalTiles;
    private int totalSafeTiles;
    private int activatedSafeTiles;
    private int minesMarked;
    
    private int timerValue;
    private int timerStartValue;
    private int timerStopValue;
    
    private bool gameActive;
    private bool timerActive;
    
    private Coroutine timerCoroutine;
    
    private TimerType timerType;
    
    #endregion
    
    #region --- Events ---
    
    public static UnityEvent onGameStart = new UnityEvent();
    public static UnityEvent onGameReset = new UnityEvent();
    
    #endregion
    
    #region --- Initialization & Termination ---
    
    protected override void Start()
    {
        base.Start();
        
        EventSubscription();
        
        totalTiles = gridWidth * gridHeight;
        totalSafeTiles = totalTiles - totalMines;
        activatedSafeTiles = 0;
        
        Initialize(gridWidth, gridHeight);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        EventUnsubscription();
    }

    private void EventSubscription()
    {
        MinesweeperTile.onActivate.AddListener(TileActivated);
        MinesweeperTile.onMark.AddListener(TileMarked);
        MinesweeperTile.onUnmark.AddListener(TileUnmarked);
    }

    private void EventUnsubscription()
    {
        MinesweeperTile.onActivate.RemoveListener(TileActivated);
        MinesweeperTile.onMark.RemoveListener(TileMarked);
    }

    // -- Variable Initialization --
    public void Initialize(int gridWidth, int gridHeight)
    {
        SpawnGrid(gridWidth, gridHeight);
    }

    public void SpawnGrid(int gridWidth, int gridHeight)
    {
        tileGrid = new MinesweeperTile[gridWidth, gridHeight]; // Set grid size

        // Populate grid with tiles
        Vector2 spawnPos = new Vector2(gridTilePadding + gridBorderWidth, -gridTilePadding + -gridBorderWidth);
        int row = 0;
        int column = 0;
        
        for ( ; row < gridHeight; row++) // For each row
        {
            if (row != 0) // If not the first row
            {
                spawnPos.y += -16 + -gridTilePadding;
            }
            
            for ( ; column < gridWidth; column++) // For each column
            {
                if (column != 0) // If not the first column
                {
                    spawnPos.x += 16 + gridTilePadding;
                }
                
                SpawnGridTile(column, row, spawnPos);
            }
            
            column = 0;
            spawnPos.x = gridTilePadding + gridBorderWidth; // Spawn grid tile at position
        }
    }
    
    public void SpawnGridTile(int column, int row, Vector2 position)
    {
        MinesweeperTile tile = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity, gridBorder.transform);
        tile.transform.SetLocalPositionAndRotation(new Vector3(position.x, position.y, 0), Quaternion.identity);
        tileGrid[column, row] = tile;
        tile.Initialize(new Vector2(column, row));
    }
    
    #endregion
    
    #region --- Game State ---
    
    public void StartGame()
    {
        List<Vector2> minePositions = SpawnMines(totalMines);
        DetermineTileValues(minePositions);
        StartTimer(TimerType.Increment, 0, timerDisplay.GetMaxValue());
        gameActive = true;
        buttonImage.sprite = buttonRestart;
        onGameStart?.Invoke();
    }
    
    public void ResetGame()
    {
        gameActive = false;
        activatedSafeTiles = 0;
        ResetTimer();
        minesDisplay.ResetDisplay();
        buttonImage.sprite = buttonPlay;
        onGameReset?.Invoke();
    }
    
    public void GameFinish(bool gameResult)
    {
        StopTimer();
        
        foreach (MinesweeperTile tile in tileGrid)
        {
            tile.GameFinished();
        }
        
        Debug.Log("Game Finished - Result: " + gameResult);
    }

    public void LoseGame()
    {
        GameFinish(false);
    }

    public void WinGame()
    {
        GameFinish(true);
    }
    
    public void ButtonPress()
    {
        if (!gameActive)
        {
            StartGame();
        }

        else
        {
            ResetGame();
        }
    }
    
    #endregion
    
    #region --- Tile Interaction ---
    public void TileMarked(MinesweeperTile tile)
    {
        if (!gameActive)
        {
            return;
        }
        
        if (tile.IsMarked())
        {
            return;
        }

        if (tile.IsBomb())
        {
            minesMarked++;
        }
        
        WinCheck();
    }

    public void TileUnmarked(MinesweeperTile tile)
    {
        if (!gameActive)
        {
            return;
        }

        if (!tile.IsMarked())
        {
            return;
        }

        if (tile.IsBomb())
        {
            minesMarked--;
        }
        
        WinCheck();
    }

    public void TileActivated(MinesweeperTile tile)
    {
        if (!gameActive)
        {
            return;
        }
        
        if (tile.IsBomb())
        {
            LoseGame();
            return;
        }

        activatedSafeTiles++;

        if (tile.IsEmpty())
        {
            ActivateSurrounding(tile);
        }
        
        WinCheck();
    }

    public void WinCheck()
    {
        if (activatedSafeTiles == totalSafeTiles && minesMarked == totalMines)
        {
            WinGame();
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
    
    public List<MinesweeperTile> GetSurroundingTiles(Vector2 originTilePos)
    {
        //Debug.Log("Getting Surrounding Tiles Of " + originTilePos);
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
    #endregion
    
    #region --- Generation --
    
    public List<Vector2> SpawnMines(int amount)
    {
        //Debug.Log("Spawning " + totalMines + " Mines");
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
            //Debug.Log("Mine Spawned: " + column + ", " + row);
            i++;
        }
        
        minesDisplay.SetValue(totalMines);
        return minePositions;
    }

    public void DetermineTileValues(List<Vector2> minePositions)
    {
        //Debug.Log("Determining Tile Values");
        
        foreach (Vector2 minePosition in minePositions)
        {
            List<MinesweeperTile> surroundingTiles = GetSurroundingTiles(minePosition);
            //Debug.Log("Mine (" + minePosition + ") has " + surroundingTiles.Count + " surrounding tiles");
            
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
    
    #endregion
    
    #region --- Timer ---
    
    public void StartTimer(TimerType timerType, int startValue, int stopValue)
    {
        timerActive = true;
        timerCoroutine = StartCoroutine(Timer(startValue, timerType, stopValue, false));
    }
    
    public void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        
        timerActive = false;
        timerCoroutine = null;
    }

    public void ResetTimer()
    {
        StopTimer();
        timerValue = 0;
        timerDisplay.ResetDisplay();
    }
    
    IEnumerator Timer(int startValue, TimerType timerType, int stopValue, bool infinite)
    {
        this.timerType = timerType;
        timerValue = startValue;
        timerDisplay.SetValue(startValue);
        
        while (timerActive)
        {
            yield return new WaitForSeconds(1f);
            
            if (this.timerType == TimerType.Increment)
            {
                timerValue++;

                if (timerValue >= timerDisplay.GetMaxValue())
                {
                    Debug.Log("Time has exceeded max display value");
                    continue;
                }
                
                if (timerValue >= stopValue)
                {
                    timerActive = false;
                }
            }
            
            else if (this.timerType == TimerType.Decrement)
            {
                timerValue--;
                
                if (timerValue <= timerDisplay.GetMinValue())
                {
                    Debug.Log("Time has exceeded min display value");
                    continue;
                }
                
                if (timerValue <= stopValue)
                {
                    timerActive = false;
                }
            }

            timerDisplay.SetValue(timerValue);
        }
    }
    #endregion
}