using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{

    protected override void Awake()
    {
        base.Awake();
    }

    public void SoundSliderSetting(Slider sfxSlider, Slider bgmSlider)
    {
        // 슬라이더 값 초기화
        sfxSlider.value = MasterAudio.MasterVolumeLevel;
        bgmSlider.value = MasterAudio.PlaylistMasterVolume;

        // 슬라이더 값이 변경될 때마다 이벤트 발생
        sfxSlider.onValueChanged.AddListener(delegate { ControllVolume(sfxSlider,bgmSlider); });
        bgmSlider.onValueChanged.AddListener(delegate { ControllVolume(sfxSlider,bgmSlider); });
    }

    // 사운드 재생, 위치 지정
    public void PlaySound(string soundName, Transform transform)
    {
        MasterAudio.PlaySound3DAtTransform(soundName, transform);
    }

    // 사운드 재생2
    public void PlaySound(string soundName)
    {
        MasterAudio.PlaySound(soundName);
    }

    //  사운드 일시정지
    public void PausePlayListSound()
    {
        MasterAudio.PausePlaylist();
    }

    // 사운드 재생
    public void UnpausePlaylistSound()
    {
        MasterAudio.UnpausePlaylist();
    }

    // 사운드 정지
    public void ChangePlayListClip(string clipName)
    {
        MasterAudio.TriggerPlaylistClip(clipName);
    }

    // 사운드 설정
    public void ControllVolume(Slider sfxSlider, Slider bgmSlider)
    {
        MasterAudio.MasterVolumeLevel = sfxSlider.value;
        MasterAudio.PlaylistMasterVolume = bgmSlider.value;
    }
}
