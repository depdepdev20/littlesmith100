using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class TransitionManager : MonoBehaviour
{
    public float transitionDuration = 2f; // Adjust as needed

    void Start()
    {
        // Start the transition effect
        StartCoroutine(TransitionToNextScene());
    }

    private IEnumerator TransitionToNextScene()
    {
        // Optional: Add transition effects here (e.g., fading in/out)
        yield return new WaitForSeconds(transitionDuration);

        // Load the next scene (stored in SceneTransitionData)
        if (!string.IsNullOrEmpty(SceneTransitionData.nextScene))
        {
            SceneManager.LoadScene(SceneTransitionData.nextScene);
        }
    }
}
