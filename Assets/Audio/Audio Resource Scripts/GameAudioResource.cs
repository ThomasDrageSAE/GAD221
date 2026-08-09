using UnityEngine;

public abstract class GameAudioResource : ScriptableObject
{
    public AudioClip audioClip;
    public float volumeBase = AudioManager.baseSourceVolume;
    public float pitchBase = AudioManager.baseSourcePitch;
    
    public virtual AudioClip GetClip()
    {
        return audioClip;
    }
}