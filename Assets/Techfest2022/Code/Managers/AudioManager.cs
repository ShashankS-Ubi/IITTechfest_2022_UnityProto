using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSourceBackground;
    [SerializeField]
    private AudioSource _audioSourceVFX;

    [SerializeField]
    private AudioDataScriptableObject _audioScriptableObject;

    [SerializeField]
    private float _masterVolume;
    public float MasterVolume => _masterVolume;

    public void Init()
    {
        
    }

    // to change the master volume level.
    public void SetMasterVolume(float vol)
    {
        _masterVolume = vol;
    }

    // wrapper to call unity audio with just passing the audio file type
    public void PlayAudio(AudioFileType fileType)
    {
        AudioData? data = GetAudioClip(fileType);
        if (!data.HasValue)
        {
            Debug.LogError($"No Audio of file type {fileType} was found");
            return;
        }

        AudioData audioData = data.Value;

        switch(data.Value.AudioType)
        {
            case LoopType.Loop:
                _audioSourceBackground.clip = audioData.AudioClip;
                _audioSourceBackground.loop = true;
                _audioSourceBackground.volume = _masterVolume;
                _audioSourceBackground.Play();
                break;
            case LoopType.OneShot:
                _audioSourceVFX.PlayOneShot(audioData.AudioClip, _masterVolume);
                break;
        }
    }

    public void PauseAudio()
    {
        if(_audioSourceBackground != null)
            _audioSourceBackground.Pause();
    }

    public void UnpauseAudio()
    {
        if (_audioSourceBackground != null)
            _audioSourceBackground.UnPause();
    }

    public void StopAudio()
    {
        if (_audioSourceBackground != null)
            _audioSourceBackground.Stop();
    }

    private AudioData GetAudioClip(AudioFileType type)
    {
        return _audioScriptableObject.AudioData.Find(x => x.AudioFileType == type);
    }
}

[Serializable]
public struct AudioData
{
    public AudioFileType AudioFileType;
    public LoopType AudioType;
    public AudioClip AudioClip;
}

public enum AudioFileType
{
    BackgroundMusic,
    Click,
    Create,
    Destroy,
    Repair,
    Ground
}

public enum LoopType
{
    Loop,
    OneShot,
}