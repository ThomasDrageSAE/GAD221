using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MinesweeperTile : UIComponent
{
    private int tileValue;
    private bool activated;
    private bool marked;

    [SerializeField] Image topTileImage;
    [SerializeField] Image bottomTileImage;
    
    [SerializeField] List<Sprite> sprites;
    
    // Events
    // OnActivate
    // OnMark
    // OnExplode

    private void Start()
    {
        onLeftClicked.AddListener(Activate);
        onRightClicked.AddListener(Mark);
    }

    public void Initialize()
    {
        tileValue = 0;
        activated = false;
        marked = false;
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
    
    public bool IsBomb()
    {
        return tileValue == 9;
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

    public void Activate() // On Left LeftClick
    {
        if (!activated)
        {
            activated = true;
            topTileImage.enabled = false;
            DisableInteraction();
            // Fire Event
        
            if (IsBomb())
            {
                Explode();
            }
        }
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
        // Fire Event
    }

    public void Explode()
    {
        
        
        // Fire Event
    }
}
