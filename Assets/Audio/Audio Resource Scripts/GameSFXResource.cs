using UnityEngine;

[CreateAssetMenu(fileName = "New Game SFX Resource", menuName = "Audio/Game SFX Resource")]
public class GameSFXResource : GameAudioResource
{
    public float volumeDeviation = AudioManager.baseSourceVolumeDeviation;
    public float pitchDeviation = AudioManager.baseSourcePitchDeviation;
}