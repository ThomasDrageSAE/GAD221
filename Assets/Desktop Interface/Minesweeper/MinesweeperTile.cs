using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MinesweeperTile : UIComponent
{
    public int tileValue;
    public bool activated;
    public bool marked;

    [SerializeField] Image topTileImage;
    [SerializeField] Image bottomTileImage;
    [SerializeField] Image markedImage;
    [SerializeField] Image valueImage;
    
    [SerializeField] Sprite oneSprite;
    [SerializeField] Sprite twoSprite;
    [SerializeField] Sprite threeSprite;
    [SerializeField] Sprite fourSprite;
    [SerializeField] Sprite fiveSprite;
    [SerializeField] Sprite sixSprite;
    [SerializeField] Sprite sevenSprite;
    [SerializeField] Sprite eightSprite;
    [SerializeField] Sprite bombSprite;

    private void Start()
    {
        onLeftClicked.AddListener(Activate);
        onRightClicked.AddListener(Mark);
    }

    private void OnDestroy()
    {
        onLeftClicked.RemoveListener(Activate);
        onRightClicked.RemoveListener(Mark);
    }

    public void SetTileValue(int value)
    {
        switch (value)
        {
            case 0:
                valueImage.sprite = null;
                tileValue = 0;
                break;
            case 1:
                valueImage.sprite = oneSprite;
                tileValue = 1;
                break;
            case 2:
                valueImage.sprite = twoSprite;
                tileValue = 2;
                break;
            case 3:
                valueImage.sprite = threeSprite;
                tileValue = 3;
                break;
            case 4:
                valueImage.sprite = fourSprite;
                tileValue = 4;
                break;
            case 5:
                valueImage.sprite = fiveSprite;
                tileValue = 5;
                break;
            case 6:
                valueImage.sprite = sixSprite;
                tileValue = 6;
                break;
            case 7:
                valueImage.sprite = sevenSprite;
                tileValue = 7;
                break;
            case 8:
                valueImage.sprite = eightSprite;
                tileValue = 8;
                break;
            case 9:
                valueImage.sprite = bombSprite;
                tileValue = 9;
                break;
            default:
                valueImage.sprite = null;
                tileValue = 0;
                break;
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
            // Fire Event
        
            if (IsBomb())
            {
                Explode();
            }
        }
    }

    public void Mark() // On Right LeftClick
    {
        marked = !marked;

        // Fire Event
    }

    public void Explode()
    {
        
        
        // Fire Event
    }
}
