using System;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class SoundManager : MMPersistentSingleton<SoundManager>
{
    [FormerlySerializedAs("SoundData")] public SoundData soundData;
    public AudioSource musicSource;

    [SerializeField] private AudioMixerGroup sfxOutput;
    [SerializeField] private AudioMixerSnapshot sfxDefault, sfxMuted, musicDefault, musicMuted;
    [SerializeField] private float timeToReachSnapshot = 0.5f;
    // [SerializeField] private Material materialBG;
    // [SerializeField] private Texture darkTexture, lightTexture;
    private readonly List<AudioSource> sources = new List<AudioSource>();
    private void Start()
    {
        var sfx = (SfxOn ? sfxDefault : sfxMuted);
        sfx.TransitionTo(timeToReachSnapshot);

        var snapshot = (MusicOn ? musicDefault : musicMuted);
        snapshot.TransitionTo(timeToReachSnapshot);

        // ThemeOn = ThemeOn;
    }

    public void PlaySfx(string soundKey, float pitch = 1, Action actionStart = null, Action actionComplete = null)
    {
        actionStart?.Invoke();
        if (!soundData.listAudioClip.ContainsKey(soundKey)) return;
        var clip = soundData.listAudioClip[soundKey];
        var source = GetFreeSource(soundKey);
        source.pitch = pitch;
        source.clip = clip;
        source.Play();
        DG.Tweening.DOVirtual.DelayedCall(clip.length + 0.2f, () => actionComplete?.Invoke());
    }
    public void PlayMusicBG(string soundKey, float pitch = 1)
    {
        if (_instance.musicSource.isPlaying) return;
        var clip = soundData.listAudioClip[soundKey];
        musicSource.pitch = pitch;
        musicSource.clip = clip;
        musicSource.Play();
    }
    public void StopMusic()
    {
        if (musicSource) musicSource.Stop();
    }

    public void ResumeMusic()
    {
        if (musicSource && !musicSource.isPlaying) musicSource.Play();
    }

    private void CreateSources(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = sfxOutput;
            source.playOnAwake = false;
            sources.Add(source);
        }
    }

    private AudioSource GetFreeSource(string key = "")
    {
        foreach (var source in sources.Where(source => !source.isPlaying))
        {
            return source;
        }

        CreateSources(1);
        return sources[sources.Count - 1];
    }

    public bool SfxOn
    {
        get => PlayerPrefs.GetInt("SfxOn", 1) == 1;
        private set
        {
            var snapshot = (value ? sfxDefault : sfxMuted);
            snapshot.TransitionTo(timeToReachSnapshot);
            PlayerPrefs.SetInt("SfxOn", value ? 1 : 0);
        }
    }
    public bool MusicOn
    {
        get => PlayerPrefs.GetInt("MusicOn", 1) == 1;
        private set
        {
            var snapshot = (value ? musicDefault : musicMuted);
            snapshot.TransitionTo(timeToReachSnapshot);
            PlayerPrefs.SetInt("MusicOn", value ? 1 : 0);
        }
    }
    public bool VibrateOn
    {
        get => PlayerPrefs.GetInt("VibrateOn", 1) == 1;
        private set => PlayerPrefs.SetInt("VibrateOn", value ? 1 : 0);
    }

    /*public bool ThemeOn
    {
        get => PlayerPrefs.GetInt("ThemeOn", 1) == 1;
        private set
        {
            materialBG.mainTexture = value ? lightTexture : darkTexture;
            PlayerPrefs.SetInt("ThemeOn", value ? 1 : 0);
        }
    }*/

    public void ChangeState(SettingType type, bool state)
    {
        switch (type)
        {
            case SettingType.SOUNDS:
                SfxOn = state;
                break;
            case SettingType.MUSIC:
                MusicOn = state;
                break;
            case SettingType.VIBRATE:
                VibrateOn = state;
                break;
            //case SettingType.THEME:
            //    ThemeOn = state;
                break;
        }
    }
}

public enum SettingType
{
    SOUNDS,
    MUSIC,
    VIBRATE,
    //THEME
}
