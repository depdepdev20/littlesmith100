using UnityEngine;

public class WeaponVisibilityController : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private GameObject UI;

    private void Start()
    {
        // Update visibility berdasarkan status unlocked
        UpdateVisibility();
    }
    public void Update()
    {
        UpdateVisibility();
    }

    public void UpdateVisibility()
    {
        // Aktifkan atau nonaktifkan object berdasarkan status unlocked
        Debug.Log($"Weapon {weapon.weaponName} unlocked status: {weapon.unlocked}");
        UI.SetActive(weapon.unlocked);
    }
}
