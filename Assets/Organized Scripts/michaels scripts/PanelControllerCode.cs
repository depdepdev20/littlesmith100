using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelControllerCode : MonoBehaviour
{
    public GameObject panelBuildGenerator;
    public GameObject panelResources;
    public Button purchaseButton;
    private bool isResourcesPanelActive = false;
    private PlotCode currentPlot;

    // Add a reference to the TextMeshProUGUI (or Text if you're using it)
    public TextMeshProUGUI plotPriceText;  // For displaying the plot price message

    void Start()
    {
        panelBuildGenerator.SetActive(false);
        panelResources.SetActive(false);

        if (purchaseButton != null)
        {
            purchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TogglePanel(panelResources, ref isResourcesPanelActive);
        }
    }

    void TogglePanel(GameObject panel, ref bool isActive)
    {
        isActive = !isActive;
        panel.SetActive(isActive);
    }

    // Update ShowPanelGenPlot to accept the dynamic text message
    public void ShowPanelGenPlot(PlotCode plot, string message)
    {
        currentPlot = plot;
        panelBuildGenerator.SetActive(true);

        // Update the text on the panel with the passed message
        plotPriceText.text = message; // Set the dynamic text
    }

    public void HidePanelGenPlot()
    {
        panelBuildGenerator.SetActive(false);
        currentPlot = null;
    }

    private void OnPurchaseButtonClicked()
    {
        if (currentPlot != null)
        {
            currentPlot.PurchasePlot();
        }
    }
}
