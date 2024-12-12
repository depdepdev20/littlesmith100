using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


public class PrologueManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string transitionSceneName = "TransitionScene"; // Transition scene name
    public string nextSceneName = "Main";             // Desired scene after transition

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // Pass the next scene's name using PlayerPrefs or a static variable
        SceneTransitionData.nextScene = nextSceneName;
        SceneManager.LoadScene(transitionSceneName);
    }
}
