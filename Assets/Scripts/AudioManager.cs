using System.Collections;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();

                if (instance == null)
                {
                    instance = new GameObject("AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
                }
            }

            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    public Transform SFXSources;
    public Transform MusicSources;

    public float musicVolume { get; private set; } = 1f;
    public bool isMusicMuted { get; private set; } = false;

    public float sfxVolume { get; private set; } = 1f;
    public bool isSfxMuted { get; private set; } = false;

    private readonly float mutedVolume = 0.001f;

    public List<MusicTime> musicTimes = new List<MusicTime>();

    private bool firstMusicSourceIsPlaying = false;
    private bool musicPaused = false;

    public void PlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (!isSfxMuted)
        {
            var tempGO = new GameObject("TempSFX");
            tempGO.transform.parent = SFXSources;
            var newSfxSource = tempGO.AddComponent<AudioSource>();

            newSfxSource.volume = volume * sfxVolume;
            newSfxSource.pitch = pitch;
            newSfxSource.PlayOneShot(clip);
            Destroy(tempGO, clip.length);
        }
    }

    public void PlaySFXDirectional(AudioClip clip, bool leftRight, float volume = 1f, float pitch = 1f)
    {
        if (!isSfxMuted)
        {
            var tempGO = new GameObject("TempSFX");
            tempGO.transform.parent = SFXSources;
            var newSfxSource = tempGO.AddComponent<AudioSource>();

            newSfxSource.volume = volume * sfxVolume;
            newSfxSource.pitch = pitch;

            if (leftRight) newSfxSource.panStereo = -0.75f;
            else newSfxSource.panStereo = 0.75f;

            newSfxSource.PlayOneShot(clip);
            Destroy(tempGO, clip.length);
        }
    }

    public void PlayMusic(AudioClip music, float volume = 1f)
    {
        foreach (Transform src in MusicSources)
        {
            var audio = src.GetComponent<AudioSource>();
            if (audio)
            {
                var count = 0;
                foreach (MusicTime time in musicTimes)
                {
                    if (audio.clip == time.clip)
                    {
                        time.currentTime = audio.time;
                        count++;
                    }
                }

                if (count == 0)
                {
                    var mTime = new MusicTime(audio.clip, audio.time);
                    musicTimes.Add(mTime);
                }

                Destroy(src.gameObject, 1.0f);
            }
        }

        var tempGO = new GameObject("Music");
        tempGO.transform.parent = MusicSources;
        var newSfxSource = tempGO.AddComponent<AudioSource>();

        newSfxSource.clip = music;
        newSfxSource.volume = !isMusicMuted ? volume * musicVolume : 0;
        newSfxSource.loop = true;
        newSfxSource.Play();

        foreach (MusicTime time in musicTimes)
        {
            if (music == time.clip)
            {
                newSfxSource.time = time.currentTime;
            }
        }
    }

    public void PlayMusicWithCrossFade(AudioClip music, float transitionTime = 1.0f, float volume = 1f)
    {
        foreach (Transform src in MusicSources)
        {
            var audio = src.GetComponent<AudioSource>();
            if (audio)
            {
                var count = 0;
                foreach (MusicTime time in musicTimes)
                {
                    if (audio.clip == time.clip)
                    {
                        time.currentTime = audio.time;
                        count++;
                    }
                }

                if (count == 0)
                {
                    var mTime = new MusicTime(audio.clip, audio.time);
                    Debug.Log(mTime);
                    musicTimes.Add(mTime);
                }

                StartCoroutine(FadeOutMusic(src.GetComponent<AudioSource>(), transitionTime));
            }
        }

        firstMusicSourceIsPlaying = !firstMusicSourceIsPlaying;

        var tempGO = new GameObject("Music");
        tempGO.transform.parent = MusicSources;
        var newSfxSource = tempGO.AddComponent<AudioSource>();

        newSfxSource.clip = music;
        newSfxSource.volume = 0;
        newSfxSource.loop = true;
        newSfxSource.Play();

        foreach (MusicTime time in musicTimes)
        {
            if (music == time.clip)
            {
                Debug.Log(time.currentTime + "" + time.clip);
                newSfxSource.time = time.currentTime;
            }
        }
        StartCoroutine(FadeInMusic(newSfxSource, volume * musicVolume, transitionTime));
    }

    public IEnumerator FadeOutMusic(AudioSource music, float transitionTime)
    {
        float t = 0.0f;
        float volume = music.volume * musicVolume;

        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            if (music) music.volume = (volume - (t / transitionTime));
            yield return null;
        }

        if (music)
        {
            music.Stop();
            Destroy(music.gameObject);
        }
    }
    public IEnumerator FadeInMusic(AudioSource music, float volume, float transitionTime)
    {
        float t = 0.0f;

        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            music.volume = (t / transitionTime) * musicVolume;
            yield return null;
        }
    }

    public void SetMusicVolume(float volume)
    {
        foreach (Transform music in MusicSources)
        {
            if (music.GetComponent<AudioSource>())
            {
                float currentSetVolume = music.GetComponent<AudioSource>().volume;
                float currentSetVolume1Percent = currentSetVolume / (musicVolume * 100);
                float newVolume = currentSetVolume1Percent * (volume * 100);

                music.GetComponent<AudioSource>().volume = newVolume;
            }
        }

        if (volume != mutedVolume) musicVolume = volume;
    }

    public void ResetMusicVolume()
    {
        Debug.Log(musicVolume);

        foreach (Transform music in MusicSources)
        {
            if (music.GetComponent<AudioSource>())
            {
                float currentSetVolume = music.GetComponent<AudioSource>().volume;
                float currentSetVolume1Percent = currentSetVolume / (0.001f * 100);
                float newVolume = currentSetVolume1Percent * (musicVolume * 100);

                music.GetComponent<AudioSource>().volume = newVolume;
            }
        }
    }

    public void PauseUnpauseMusic()
    {
        foreach (Transform music in MusicSources)
        {
            if (music.GetComponent<AudioSource>())
            {
                if (!musicPaused) music.GetComponent<AudioSource>().Pause();
                else music.GetComponent<AudioSource>().UnPause();
            }
        }

        musicPaused = !musicPaused;
    }

    public void MuteUnmuteMusic()
    {
        if (isMusicMuted)
        {
            ResetMusicVolume();
        }
        else
        {
            SetMusicVolume(mutedVolume);
        }

        isMusicMuted = !isMusicMuted;
    }

    public void SetSFXVolume(float setVolume)
    {
        foreach (Transform sfx in SFXSources)
        {
            if (sfx.GetComponent<AudioSource>())
            {
                float currentSetVolume = sfx.GetComponent<AudioSource>().volume;
                float currentSetVolume1Percent = currentSetVolume / (sfxVolume * 100);
                float newVolume = currentSetVolume1Percent * (setVolume * 100);

                sfx.GetComponent<AudioSource>().volume = newVolume;
            }
        }

        if (setVolume != mutedVolume) sfxVolume = setVolume;
    }

    public void ResetSFXVolume()
    {
        foreach (Transform sfx in SFXSources)
        {
            if (sfx.GetComponent<AudioSource>())
            {
                float currentSetVolume = sfx.GetComponent<AudioSource>().volume;
                float currentSetVolume1Percent = currentSetVolume / (0.001f * 100);
                float newVolume = currentSetVolume1Percent * (sfxVolume * 100);

                sfx.GetComponent<AudioSource>().volume = newVolume;
            }
        }
    }

    public void MuteUnmuteSfx()
    {
        if (isSfxMuted)
        {
            ResetSFXVolume();
        }
        else
        {
            SetSFXVolume(mutedVolume);
        }

        isSfxMuted = !isSfxMuted;
    }
}

[System.Serializable]
public class MusicTime
{
    public float currentTime;
    public AudioClip clip;

    public MusicTime(AudioClip audio, float time)
    {
        clip = audio;
        currentTime = time;
    }
}