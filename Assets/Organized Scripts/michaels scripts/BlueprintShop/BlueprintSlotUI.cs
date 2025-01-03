using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlueprintSlotUI : MonoBehaviour
{
    public Image blueprintImage; // Referensi ke komponen Image
    public TextMeshProUGUI blueprintNameText; // Referensi ke komponen Text untuk nama blueprint
    public TextMeshProUGUI priceText; // Referensi ke komponen Text untuk harga

    public Button buyButton; // Tombol beli
    public Button lockedBuyButton; // Tombol beli terkunci
    public Button soldButton; // Tombol "Terbeli"

    private Blueprint blueprint; // Blueprint yang sedang ditampilkan

    [SerializeField] private int currentChapter; // Chapter saat ini

    private bool isChapterUnlocked; // Status apakah chapter sudah terpenuhi
    private bool isPurchased = false; // Status apakah blueprint sudah dibeli

    private void Start()
    {
        UpdateUI(); // Perbarui UI saat script pertama kali berjalan
    }

    private void Update()
    {
        // Cek secara berkala apakah kondisi berubah
        if (IsChapterUnlocked() != isChapterUnlocked)
        {
            isChapterUnlocked = IsChapterUnlocked();
            UpdateUI();
        }

        if (ChapterManager.instance != null)
        {
            currentChapter = ChapterManager.instance.CurrentChapter;
        }
    }

    public void SetBlueprint(Blueprint newBlueprint)
    {
        blueprint = newBlueprint;

        // Update UI
        blueprintImage.sprite = blueprint.image;
        blueprintNameText.text = blueprint.blueprintName;
        priceText.text = blueprint.buyPrice.ToString() + " Gold";

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (isPurchased)
        {
            // Blueprint sudah terbeli
            buyButton.gameObject.SetActive(false);
            lockedBuyButton.gameObject.SetActive(false);
            soldButton.gameObject.SetActive(true);
        }
        else if (IsChapterUnlocked())
        {
            // Chapter terpenuhi
            buyButton.gameObject.SetActive(true);
            lockedBuyButton.gameObject.SetActive(false);
            soldButton.gameObject.SetActive(false);

            // Tambahkan listener hanya jika tombol beli aktif
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(OnBuyButtonClicked);
        }
        else
        {
            // Chapter belum terpenuhi
            buyButton.gameObject.SetActive(false);
            lockedBuyButton.gameObject.SetActive(true);
            soldButton.gameObject.SetActive(false);
        }
    }

    private void OnBuyButtonClicked()
    {
        // Proses pembelian blueprint
        isPurchased = true;
        blueprint.Purchase();
        Debug.Log($"Blueprint {blueprint.blueprintName} purchased!");

        // Perbarui UI
        UpdateUI();
    }

    private bool IsChapterUnlocked()
    {
        return currentChapter >= blueprint.chapter;
    }

    // Fungsi untuk memperbarui chapter
    public void SetCurrentChapter(int chapter)
    {
        currentChapter = chapter;
    }
}

