using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    protected override void Awake()
    {
        base.Awake();
    }
    
    public void PlaySound(string soundName, Transform transform)
    {
        MasterAudio.PlaySound3DAtTransform(soundName, transform);
    }

    public void PlaySound(string soundName)
    {
        MasterAudio.PlaySound(soundName);                        
    }

    public void PausePlayListSound()
    {
        MasterAudio.PausePlaylist();
    }

    public void UnpausePlaylistSound()
    {
        MasterAudio.UnpausePlaylist();
    }
    public void ChangePlayListClip(string clipName)
    {
        MasterAudio.TriggerPlaylistClip(clipName);
    }


}
