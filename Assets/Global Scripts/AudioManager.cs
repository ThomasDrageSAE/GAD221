using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : Singleton<AudioManager>
{
    #region --- Inspector References ---
    
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambienceSource;
    [SerializeField] private List<AudioSource> sfxSources;
    
    #endregion
    
    #region --- Properties & Variables---
    
    // -- Public --
    public int masterVolume = 100;
    public bool masterMuted = false;
    public int musicVolume = 100;
    public bool musicMuted = false;
    public int ambienceVolume = 100;
    public bool ambienceMuted = false;
    public int sfxVolume = 100;
    public bool sfxMuted = false;
    
    public const float baseSourceVolume = 0.5f;
    public const float baseSourcePitch = 1.0f;
    public const float baseSourceVolumeDeviation = 0.0f;
    public const float baseSourcePitchDeviation = 0.0f;
    
    public enum MixerTrack
    {
        Master,
        Music,
        Ambience,
        SFX
    }
    
    // -- Private --
    
    private const float minSourceVolume = 0.0f;
    private const float maxSourceVolume = 1.0f;
    private const float minSourcePitch = -3.0f;
    private const float maxSourcePitch = 3.0f;
    
    private const float minTrackDecibelVolume = -80.0f;
    
    #endregion
    
    #region --- Initialization & Termination ---

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        SetTrackVolume(MixerTrack.Master, 100);
        SetTrackVolume(MixerTrack.Music, 100);
        SetTrackVolume(MixerTrack.Ambience, 100);
        SetTrackVolume(MixerTrack.SFX, 100);
    
        SetTrackMuted(MixerTrack.Master, false);
        SetTrackMuted(MixerTrack.Music, false);
        SetTrackMuted(MixerTrack.Ambience, false);
        SetTrackMuted(MixerTrack.SFX, false);
    }

    #endregion
    
    #region --- Track Volume Control ---

    public void SetTrackVolume(MixerTrack track, int volume)
    {
        switch (track)
        {
            case MixerTrack.Master:
                SetTrackVolume("MasterVolume", volume);
                masterVolume = volume;
                break;
            case MixerTrack.Music:
                SetTrackVolume("MusicVolume", volume);
                musicVolume = volume;
                break;
            case MixerTrack.Ambience:
                SetTrackVolume("AmbienceVolume", volume);
                ambienceVolume = volume;
                break;
            case MixerTrack.SFX:
                SetTrackVolume("SFXVolume", volume);
                sfxVolume = volume;
                break;
        }
    }

    private void SetTrackVolume(string trackVolumeName, int volume)
    {
        audioMixer.SetFloat(trackVolumeName, VolumeToDecibel(volume));
    }
    
    public float GetTrackVolume(MixerTrack track)
    {
        switch (track)
        {
            case MixerTrack.Master:
                return masterVolume;
            case MixerTrack.Music:
                return musicVolume;
            case MixerTrack.Ambience:
                return ambienceVolume;
            case MixerTrack.SFX:
                return sfxVolume;
            default:
                return -1.0f;
        }
    }

    public void MuteTrack(MixerTrack track)
    {
        SetTrackMuted(track, true);
    }

    public void UnmuteTrack(MixerTrack track)
    {
        SetTrackMuted(track, false);
    }

    private void SetTrackMuted(MixerTrack track, bool muted)
    {
        switch (track)
        {
            case MixerTrack.Master:
                masterMuted = muted;
                break;
            case MixerTrack.Music:
                musicMuted = muted;
                break;
            case MixerTrack.Ambience:
                ambienceMuted = muted;
                break;
            case MixerTrack.SFX:
                sfxMuted = muted;
                break;
        }
        
        ApplyMuteStates();
    }
    
    private void ApplyMuteStates()
    {
        musicSource.mute = masterMuted || musicMuted;
        ambienceSource.mute = masterMuted || ambienceMuted;
        
        foreach (AudioSource sfxSource in sfxSources)
        {
            sfxSource.mute = masterMuted || sfxMuted;
        }
    }
    
    #endregion

    #region --- Audio Playback ---
    
    public void PlayMusic(GameMusicResource gameMusicResource)
    {
        Debug.Log("AudioManager - PlayMusic - " + gameMusicResource.name);
        SourcePlay(musicSource, gameMusicResource.GetClip(), gameMusicResource.volumeBase, baseSourceVolumeDeviation, gameMusicResource.pitchBase, baseSourcePitchDeviation, gameMusicResource.looping);
    }

    public void PlayAmbience(GameAmbienceResource gameAmbienceResource)
    {
        Debug.Log("AudioManager - PlayAmbience - " + gameAmbienceResource.name);
        SourcePlay(ambienceSource, gameAmbienceResource.GetClip(), gameAmbienceResource.volumeBase, baseSourceVolumeDeviation, gameAmbienceResource.pitchBase, baseSourcePitchDeviation, gameAmbienceResource.looping);
    }

    public void PlaySFX(GameSFXResource gameSFXResource)
    {
        Debug.Log("AudioManager - PlaySFX - " + gameSFXResource.name);
        PlaySFX(gameSFXResource.GetClip(), gameSFXResource.volumeBase, gameSFXResource.volumeDeviation, gameSFXResource.pitchBase, gameSFXResource.pitchDeviation);
    }

    private void PlaySFX(AudioClip audioClip, float volumeBase, float volumeDeviation, float pitchBase, float pitchDeviation)
    {
        AudioSource sfxSource = null;
        bool sourceFound = false;

        // Play the SFX. Already playing SFX source given priority to cutoff existing playback of that SFX. Otherwise, first empty source found.
        for (int i = 0; i < sfxSources.Count; i++)
        {
            AudioSource candidateSource = sfxSources[i];
            
            if (candidateSource.isPlaying && candidateSource.clip == audioClip) // Source is currently playing that SFX
            {
                sfxSource = candidateSource;
                sfxSource.Stop();
                break;
            }

            if (!candidateSource.isPlaying && sourceFound == false) // Source is currently not playing & a valid source hasn't already been found
            {
                sfxSource = candidateSource;
                sourceFound = true;
            }
        }
        
        if (sfxSource != null) // If a valid source was found play the SFX
        {
            SourcePlay(sfxSource, audioClip, volumeBase, volumeDeviation, pitchBase, pitchDeviation, false);
        }

        else // Otherwise all SFX sources are occupied, Log a warning.
        {
            Debug.LogWarning("AudioManager - PlaySFX - Failed: All SFX Sources Currently Occupied.");
        }
    }
    
    private void SourcePlay(AudioSource audioSource, AudioClip audioClip, float volumeBase, float volumeDeviation, float pitchBase, float pitchDeviation, bool looping)
    {
        // Set Clip
        audioSource.clip = audioClip;
        
        // Set Volume
        if (volumeDeviation > 0)
        {
            volumeBase = DeviateFloat(volumeBase, volumeDeviation, minSourceVolume, maxSourceVolume);
        }

        audioSource.volume = volumeBase;

        // Set Pitch
        if (pitchDeviation > 0)
        {
            pitchBase = DeviateFloat(pitchBase, pitchDeviation, minSourcePitch, maxSourcePitch);
        }

        audioSource.pitch = pitchBase;
        
        // Set Looping
        audioSource.loop = looping;
        
        // Play
        audioSource.Play();
        Debug.Log("AudioManager - SourcePlay - track: " + audioSource.name + " audioClip: " + audioClip.name + " volume: " + volumeBase + " pitch: " + pitchBase + " looping: " + looping + ".");
    }
    
    public void StopMusic()
    {
        musicSource.Stop();
        ResetSource(musicSource);
    }

    public void StopAmbience()
    {
        ambienceSource.Stop();
        ResetSource(ambienceSource);
    }

    public void StopSFX(AudioClip audioClip)
    {
        foreach (AudioSource sfxSource in sfxSources)
        {
            if (sfxSource.isPlaying && sfxSource.clip == audioClip)
            {
                sfxSource.Stop();
                ResetSource(sfxSource);
            }
        }
    }
    
    public void StopAllSFX()
    {
        foreach (AudioSource sfxSource in sfxSources)
        {
            sfxSource.Stop();
            ResetSource(sfxSource);
        }
    }

    private void ResetSource(AudioSource source)
    {
        source.clip = null;
        source.volume = baseSourceVolume;
        source.pitch = baseSourcePitch;
    }
    
    #endregion
    
    private float DeviateFloat(float baseValue, float deviation, float minValue, float maxValue)
    {
        float min = Mathf.Max(baseValue - deviation, minValue);
        float max = Mathf.Min(baseValue + deviation, maxValue);
    
        return Random.Range(min, max);
    }
    
    private int DecibelToVolume(float decibels)
    {
        return Mathf.RoundToInt(Mathf.Pow(10.0f, decibels / 20.0f) * 100.0f);
    }

    private float VolumeToDecibel(float volume)
    {
        if (volume <= 0.0f)
        {
            return minTrackDecibelVolume;
        }
    
        return Mathf.Log10(volume / 100.0f) * 20.0f;
    }
}