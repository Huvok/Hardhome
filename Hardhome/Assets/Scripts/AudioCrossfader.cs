using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCrossfader : MonoBehaviour
{
    AudioSource _source0;
    AudioSource _source1;

    bool cur_is_source0 = true; //is _source0 currently the active AudioSource (plays some sound right now)

    Coroutine _curSourceFadeRoutine = null;
    Coroutine _newSourceFadeRoutine = null;

    void Reset()
    {
        Update();
    }

    void Awake()
    {
        Update();
    }

    void Update()
    {
        if (_source0 == null || _source1 == null)
        {
            InitAudioSources();
        }

    }

    void InitAudioSources()
    {
        AudioSource[] audioSources = gameObject.GetComponents<AudioSource>();

        if (ReferenceEquals(audioSources, null) || audioSources.Length == 0)
        {
            _source0 = gameObject.AddComponent<AudioSource>();
            _source1 = gameObject.AddComponent<AudioSource>();
            return;
        }

        switch (audioSources.Length)
        {
            case 1:
                {
                    _source0 = audioSources[0];
                    _source1 = gameObject.AddComponent<AudioSource>();
                }
                break;
            default:
                { 
                    _source0 = audioSources[0];
                    _source1 = audioSources[1];
                }
                break;
        }
    }

    public void CrossFade(AudioClip clipToPlay, float maxVolume, float fadingTime, float delay_before_crossFade = 0)
    {
        StartCoroutine(Fade(clipToPlay, maxVolume, fadingTime, delay_before_crossFade));
    }

    IEnumerator Fade(AudioClip playMe, float maxVolume, float fadingTime, float delay_before_crossFade = 0)
    {
        if (delay_before_crossFade > 0)
        {
            yield return new WaitForSeconds(delay_before_crossFade);
        }

        AudioSource curActiveSource, newActiveSource;
        if (cur_is_source0)
        {
            curActiveSource = _source0;
            newActiveSource = _source1;
        }
        else
        {
            curActiveSource = _source1;
            newActiveSource = _source0;
        }

        newActiveSource.clip = playMe;
        newActiveSource.Play();
        newActiveSource.volume = 0;

        if (_curSourceFadeRoutine != null)
        {
            StopCoroutine(_curSourceFadeRoutine);
        }

        if (_newSourceFadeRoutine != null)
        {
            StopCoroutine(_newSourceFadeRoutine);
        }

        _curSourceFadeRoutine = StartCoroutine(fadeSource(curActiveSource, curActiveSource.volume, 0, fadingTime));
        _newSourceFadeRoutine = StartCoroutine(fadeSource(newActiveSource, newActiveSource.volume, maxVolume, fadingTime));

        cur_is_source0 = !cur_is_source0;

        yield break;
    }

    IEnumerator fadeSource(AudioSource sourceToFade, float startVolume, float endVolume, float duration)
    {
        float startTime = Time.time;

        while (true)
        {
            float elapsed = Time.time - startTime;

            sourceToFade.volume = Mathf.Clamp01(Mathf.Lerp(startVolume, endVolume, elapsed / duration));

            if (sourceToFade.volume == endVolume)
            {
                break;
            }

            yield return null;
        }
    }

    public bool isPlaying
    {
        get
        {
            if (_source0 == null || _source1 == null)
            {
                return false;
            }

            return _source0.isPlaying || _source1.isPlaying;
        }
    }
}