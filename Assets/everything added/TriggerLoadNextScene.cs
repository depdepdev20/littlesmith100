using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using KasperDev.ModularComponents;


public class TriggerLoadNextScene : MonoBehaviour
{
    [SerializeField] private BoolVariableSO onPrologueTalkFinished; // ScriptableObject for quest completion
    public string transitionSceneName = "TransitionScene"; // Transition scene name
    public string nextSceneName = "Main"; // Desired scene after transition

    void Start()
    {
        MusicManager.Instance.TurnOffMusic(1.0f);
        MusicManager.Instance.PlayMusic("prologue");
        LoadVolume();
    }

    void Update()
    {
        if (onPrologueTalkFinished != null && onPrologueTalkFinished.Value)
        {
            SceneTransitionData.nextScene = nextSceneName;
            SceneManager.LoadScene(transitionSceneName);
        }
    }

    private void LoadVolume()
    {
        // Stub for loading volume settings, ensure this method exists or remove it.
    }
}
