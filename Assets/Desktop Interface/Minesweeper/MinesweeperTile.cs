using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MinesweeperTile : UIComponent
{
    private int tileValue;
    private bool activated;
    private bool marked;
    private Vector2 gridPos;

    [SerializeField] Image topTileImage;
    [SerializeField] Image bottomTileImage;
    
    [SerializeField] List<Sprite> sprites;
    [SerializeField] Sprite mineDefusedSprite;
    [SerializeField] Sprite mineExplodedSprite;
    
    public static UnityEvent<MinesweeperTile> onActivate = new UnityEvent<MinesweeperTile>();
    public static UnityEvent<MinesweeperTile> onMark = new UnityEvent<MinesweeperTile>();
    public static UnityEvent<MinesweeperTile> onUnmark = new UnityEvent<MinesweeperTile>();

    protected override void Start()
    {
        base.Start();
        
        
    }
    
    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        
    }

    protected override void EventSubscription()
    {
        base.EventSubscription();
        
        onLeftClicked.AddListener(Activate);
        onRightClicked.AddListener(RightClicked);
        Minesweeper.onGameStart.AddListener(GameStarted);
        Minesweeper.onGameReset.AddListener(Reset);
    }

    protected override void EventUnsubscription()
    {
        base.EventUnsubscription();
        
        onLeftClicked.RemoveListener(Activate);
        onRightClicked.RemoveListener(RightClicked);
        Minesweeper.onGameStart.RemoveListener(GameStarted);
        Minesweeper.onGameReset.RemoveListener(Reset);
    }
    
    public void Initialize(Vector2 gridPos)
    {
        Initialize(0, false, false, gridPos);
        DisableInteraction();
    }
    
    public void Initialize(int tileValue, bool activated, bool marked, Vector2 gridPos)
    {
        this.tileValue = tileValue;
        this.activated = activated;
        this.marked = marked;
        this.gridPos = gridPos;
    }
    
    public void SetTileValue(int value)
    {
        tileValue = value;
        UpdateBottomSprite();
    }

    public int GetTileValue()
    {
        return tileValue;
    }

    public Vector2 GetGridPos()
    {
        return gridPos;
    }

    public void UpdateBottomSprite()
    {
        if (tileValue >= 0 && tileValue < 10)
        {
            bottomTileImage.sprite = sprites[tileValue];
        }

        else
        {
            Debug.Log("Invalid Bottom Sprite: " + tileValue);
        }
    }

    public void UpdateTopSprite()
    {
        if (!IsMarked())
        {
            topTileImage.sprite = sprites[10];
        }
        
        else
        {
            topTileImage.sprite = sprites[11];
        }
    }

    public void Activate()
    {
        if (IsMarked())
        {
            return;
        }

        if (IsActivated())
        {
            return;
        }
        
        activated = true;
        topTileImage.enabled = false;
        DisableInteraction();
        onActivate?.Invoke(this);
    }

    public void DisableInteraction()
    {
        highlight.enabled = false;
        hoverable = false;
        leftClickable = false;
        rightClickable = false;
    }

    public void EnableInteraction()
    {
        highlight.enabled = false;
        hoverable = true;
        leftClickable = true;
        rightClickable = true;
    }

    public void RightClicked()
    {
        if (marked)
        {
            Unmark();
        }

        else
        {
            Mark();
        }
    }

    public void Mark()
    {
        marked = true;
        UpdateTopSprite();
        onMark?.Invoke(this);
    }

    public void Unmark()
    {
        marked = false;
        UpdateTopSprite();
        onUnmark?.Invoke(this);
    }

    public void GameFinished()
    {
        if (IsBomb())
        {
            if (IsMarked())
            {
                bottomTileImage.sprite = mineDefusedSprite;
            }

            else
            {
                bottomTileImage.sprite = mineExplodedSprite;
            }
        }
        
        activated = true;
        topTileImage.enabled = false;
        DisableInteraction();
    }

    public void GameStarted()
    {
        EnableInteraction();
    }

    public void Reset()
    {
        SetTileValue(0);
        topTileImage.enabled = true;
        bottomTileImage.enabled = true;
        activated = false;
        Unmark();
        DisableInteraction();
    }

    public bool IsEmpty()
    {
        return tileValue == 0;
    }
    
    public bool IsBomb()
    {
        return tileValue == 9;
    }
    
    public bool IsActivated()
    {
        return activated;
    }

    public bool IsMarked()
    {
        return marked;
    }
}