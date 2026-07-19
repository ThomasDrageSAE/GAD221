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
    
    // Events
    // OnActivate
    public static UnityEvent<MinesweeperTile> onActivate = new UnityEvent<MinesweeperTile>();
    // OnMark
    public static UnityEvent<MinesweeperTile> onMark = new UnityEvent<MinesweeperTile>();

    private void Start()
    {
        onLeftClicked.AddListener(Activate);
        onRightClicked.AddListener(Mark);
    }

    public void Initialize()
    {
        Initialize(0, false, false, Vector2.zero);
    }
    
    public void Initialize(Vector2 gridPos)
    {
        Initialize(0, false, false, gridPos);
    }
    
    public void Initialize(int tileValue, bool activated, bool marked, Vector2 gridPos)
    {
        this.tileValue = tileValue;
        this.activated = activated;
        this.marked = marked;
        this.gridPos = gridPos;
    }
    
    private void OnDestroy()
    {
        onLeftClicked.RemoveListener(Activate);
        onRightClicked.RemoveListener(Mark);
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
        if (tileValue > 0 && tileValue < 10)
        {
            bottomTileImage.sprite = sprites[tileValue];
        }

        else
        {
            Debug.Log("Invalid Bottom Sprite");
        }
    }

    public void UpdateTopSprite()
    {
        if (!marked)
        {
            topTileImage.sprite = sprites[10];
        }
        
        else
        {
            topTileImage.sprite = sprites[11];
        }
    }

    public void Activate() // On Left LeftClick
    {
        if (marked)
        {
            return;
        }

        if (activated)
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

    public void Mark() // On Right LeftClick
    {
        marked = !marked;
        Debug.Log("Marked: " + marked);
        UpdateTopSprite();
        onMark?.Invoke(this);
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

    public bool IsDefused()
    {
        if (tileValue == 9 && marked)
        {
            return true;
        }

        return false;
    }

    public bool IsIncorrect()
    {
        if (tileValue != 9 && marked)
        {
            return true;
        }
        
        return false;
    }
}
