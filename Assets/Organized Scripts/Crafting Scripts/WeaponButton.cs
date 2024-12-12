using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    public Weapon weapon; // Reference to the ScriptableObject Weapon
    public CraftingUIManager craftingUIManager; // Reference to the CraftingUIManager
    public RawImage selectedRawImage; // Reference to the "selected" RawImage

    private void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnWeaponButtonPressed);
        }

        // Initially hide the "selected" RawImage
        if (selectedRawImage != null)
        {
            selectedRawImage.gameObject.SetActive(false);
        }
    }

    private void OnWeaponButtonPressed()
    {
        if (craftingUIManager == null) return;

        // Tell CraftingUIManager this button was clicked
        craftingUIManager.SelectWeaponButton(this);

        // Update UI with weapon data
        craftingUIManager.DisplayWeaponMaterials(weapon);
    }

    // Method to show or hide the "selected" RawImage
    public void SetSelected(bool isSelected)
    {
        if (selectedRawImage != null)
        {
            selectedRawImage.gameObject.SetActive(isSelected);
        }
    }
}
