/*using UnityEngine;
using UnityEngine.UI;

public class BlueprintSlotUI : MonoBehaviour
{
    public Image weaponImage;    // Referensi ke komponen Image untuk gambar weapon
    public Text weaponNameText;  // Referensi ke komponen Text untuk nama weapon
    public Text priceText;       // Referensi ke komponen Text untuk harga
    public Button buyButton;     // Referensi ke tombol beli

    private Blueprint blueprint; // Blueprint yang sedang ditampilkan di slot ini

    // Fungsi untuk menginisialisasi slot dengan data Blueprint
    public void SetBlueprint(Blueprint newBlueprint, int currentChapter)
    {
        blueprint = newBlueprint;

        // Update UI
        weaponImage.sprite = blueprint.weaponToUnlock.image;
        weaponNameText.text = blueprint.blueprintName;
        priceText.text = blueprint.buyPrice.ToString() + " Gold";

        // Cek apakah blueprint bisa dibeli berdasarkan chapter
        buyButton.interactable = blueprint.CanPurchase(currentChapter);
    }

    // Fungsi yang dipanggil saat tombol beli diklik
    public void OnBuyButtonClicked()
    {
        // Panggil fungsi pembelian dari BlueprintShop
        BlueprintShop.Instance.PurchaseBlueprint(blueprint);
    }
}
*/

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlueprintSlotUI : MonoBehaviour
{
    public Image blueprintImage; // Referensi ke komponen Image
    public TextMeshProUGUI blueprintNameText; // Referensi ke komponen Text untuk nama blueprint
    public TextMeshProUGUI priceText; // Referensi ke komponen Text untuk harga
    public Button buyButton; // Tombol beli

    private Blueprint blueprint; // Blueprint yang sedang ditampilkan

    public void SetBlueprint(Blueprint newBlueprint)
    {
        blueprint = newBlueprint;

        // Update UI
        blueprintImage.sprite = blueprint.image;
        blueprintNameText.text = blueprint.blueprintName;
        priceText.text = blueprint.buyPrice.ToString() + " Gold";

        // Atur tombol beli
        buyButton.onClick.AddListener(OnBuyButtonClicked);
    }

    private void OnBuyButtonClicked()
    {
        // Proses pembelian blueprint
        blueprint.Purchase();
        Debug.Log($"Blueprint {blueprint.blueprintName} purchased!");
    }

}
