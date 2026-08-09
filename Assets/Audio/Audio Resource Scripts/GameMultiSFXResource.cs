using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Multi SFX Resource", menuName = "Audio/Game Multi SFX Resource")]
public class GameMultiSFXResource : GameSFXResource
{
    #region --- Properties & Variables ---
    
    public enum PlaybackOrder
    {
        Random, RandomNoRepeat, Sequenced
    }
    
    public List<AudioClip> audioClips;
    public PlaybackOrder playbackOrder = PlaybackOrder.Random;
    
    private int lastIndex = -1;
    
    #endregion
    
    #region --- Clip Selection ---
    
    public override AudioClip GetClip()
    {
        if (audioClips == null || audioClips.Count == 0)
        {
            Debug.LogWarning("GameMultiSFXResource - GetClip - Failed: No Clips Assigned.");
            return null;
        }
        
        if (audioClips.Count == 1)
        {
            lastIndex = 0;
            return audioClips[0];
        }
        
        int index = 0;
        
        switch (playbackOrder)
        {
            case PlaybackOrder.Random:
                index = GetRandomIndex();
                break;
            case PlaybackOrder.RandomNoRepeat:
                index = GetRandomNoRepeatIndex();
                break;
            case PlaybackOrder.Sequenced:
                index = GetSequencedIndex();
                break;
        }
        
        lastIndex = index;
        return audioClips[index];
    }

    private int GetRandomIndex()
    {
        return Random.Range(0, audioClips.Count);
    }
    
    private int GetRandomNoRepeatIndex() // Return random index excluding lastIndex.
    {
        if (lastIndex < 0)
        {
            return Random.Range(0, audioClips.Count);
        }
        
        int index = Random.Range(0, audioClips.Count - 1);
        
        if (index >= lastIndex)
        {
            index++;
        }
        
        return index;
    }
    
    private int GetSequencedIndex() // Return next index in order. Loop if reached end of list.
    {
        int index = lastIndex + 1;
        
        if (index >= audioClips.Count)
        {
            index = 0;
        }
        
        return index;
    }
    
    #endregion
}