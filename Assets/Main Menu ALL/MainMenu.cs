using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    private const string tempLoadFilePath = "load_flag.temp";
    private const float minVolumeValue = 0.0001f; // Prevents log10(0)

    private void Start()
    {
        MusicManager.Instance.PlayMusic("mainMenu");
        LoadVolume();
    }

    public void Play()
    {
        MusicManager.Instance.TurnOffMusic(1.0f);
        SceneTransitionData.nextScene = "Main";
        SceneManager.LoadScene("TransitionScene");
    }
    public void Load()
    {
        // Create a temporary file to signal loading
        string tempFilePath = Path.Combine(Application.persistentDataPath, tempLoadFilePath);
        File.WriteAllText(tempFilePath, "LOAD_GAME");

        // Handle the scene transition to "Main"
        MusicManager.Instance.TurnOffMusic(1.0f);
        SceneTransitionData.nextScene = "Main"; // Set the next scene to "Main"
        SceneManager.LoadScene("TransitionScene"); // Load the transition scene
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void UpdateMusicVolume(float value)
    {
        // Ensure value is always above the minimum threshold
        value = Mathf.Clamp(value, minVolumeValue, 1f);
        float volume = Mathf.Log10(value) * 20;
        audioMixer.SetFloat("MusicVolume", volume);
        Debug.Log($"Music Volume updated to {volume} dB");
    }

    public void UpdateSoundVolume(float value)
    {
        // Ensure value is always above the minimum threshold
        value = Mathf.Clamp(value, minVolumeValue, 1f);
        float volume = Mathf.Log10(value) * 20;
        audioMixer.SetFloat("SFXVolume", volume);
        Debug.Log($"SFX Volume updated to {volume} dB");
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        Debug.Log("Volumes saved.");
    }

    public void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        UpdateMusicVolume(musicVolume);
        UpdateSoundVolume(sfxVolume);
        Debug.Log("Volumes loaded.");
    }
}
