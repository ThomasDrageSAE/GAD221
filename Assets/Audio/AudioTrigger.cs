using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    #region --- Inspector References ---
    
    [SerializeField] private GameAudioResource audioResource;
    [SerializeField] private bool playOnStart = false;
    
    #endregion
    
    #region --- Initialization ---
    
    private void Start()
    {
        if (playOnStart)
        {
            Play();
        }
    }
    
    #endregion
    
    #region --- Audio Playback ---
    
    public void Play()
    {
        switch (audioResource)
        {
            case GameSFXResource sfxResource:
                AudioManager.Instance.PlaySFX(sfxResource);
                break;
            case GameMusicResource musicResource:
                AudioManager.Instance.PlayMusic(musicResource);
                break;
            case GameAmbienceResource ambienceResource:
                AudioManager.Instance.PlayAmbience(ambienceResource);
                break;
            default:
                Debug.LogWarning("AudioTrigger - Play - Failed: Invalid Audio Resource Type.");
                break;
        }
    }
    
    public void Stop()
    {
        switch (audioResource)
        {
            case GameSFXResource sfxResource:
                AudioManager.Instance.StopSFX(sfxResource.audioClip);
                break;
            case GameMusicResource:
                AudioManager.Instance.StopMusic();
                break;
            case GameAmbienceResource:
                AudioManager.Instance.StopAmbience();
                break;
            default:
                Debug.LogWarning("AudioTrigger - Stop - Failed: Invalid Audio Resource Type.");
                break;
        }
    }
    
    #endregion
}