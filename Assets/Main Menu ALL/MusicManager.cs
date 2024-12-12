using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField]
    private MusicLibrary musicLibrary;

    [SerializeField]
    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayMusic(string trackName, float fadeDuration = 0.5f)
    {
        StartCoroutine(AnimateMusicCrossFade(musicLibrary.GetClipFromName(trackName), fadeDuration));
    }

    private IEnumerator AnimateMusicCrossFade(AudioClip nextTrack, float fadeDuration = 0.5f)
    {
        if (nextTrack == null)
        {
            yield break; // Exit if no track is provided
        }

        // Fade out current music
        float percent = 0;
        float initialVolume = musicSource.volume;
        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            musicSource.volume = Mathf.Lerp(initialVolume, 0, percent);
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = nextTrack;
        musicSource.Play();

        // Fade in new music
        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            musicSource.volume = Mathf.Lerp(0, 1f, percent);
            yield return null;
        }
    }

    public void TurnOffMusic(float fadeDuration = 0.5f)
    {
        StartCoroutine(AnimateMusicTurnOff(fadeDuration));
    }

    private IEnumerator AnimateMusicTurnOff(float fadeDuration)
    {
        float percent = 0;
        float initialVolume = musicSource.volume;

        // Fade out the music
        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            musicSource.volume = Mathf.Lerp(initialVolume, 0, percent);
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = null; // Clear the current clip
    }
}
