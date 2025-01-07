using UnityEngine;

public class DisableUIElementsByTagOnEnable : MonoBehaviour
{
    [SerializeField] private string tagToDisable = "notVisibleOnCloseUps"; // Tag to identify UI elements to disable

    private void OnEnable()
    {
        SetUIElementsActive(false); // Disable UI elements when this GameObject is enabled
    }

    private void OnDisable()
    {
        SetUIElementsActive(true); // Re-enable UI elements when this GameObject is disabled
    }

    private void SetUIElementsActive(bool isActive)
    {
        GameObject[] uiElements = GameObject.FindGameObjectsWithTag(tagToDisable);

        if (uiElements != null && uiElements.Length > 0)
        {
            foreach (GameObject uiElement in uiElements)
            {
                if (uiElement != null)
                {
                    uiElement.SetActive(isActive);
                    Debug.Log($"{uiElement.name} has been {(isActive ? "enabled" : "disabled")} because {gameObject.name} was {(isActive ? "disabled" : "enabled")}.");
                }
                else
                {
                    Debug.LogWarning("DisableUIElementsByTagOnEnable: One of the UI elements with the specified tag is null.");
                }
            }
        }
        else
        {
            Debug.LogWarning($"DisableUIElementsByTagOnEnable: No UI elements found with the tag '{tagToDisable}'.");
        }
    }
}
