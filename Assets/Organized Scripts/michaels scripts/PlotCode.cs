using UnityEngine;

public class PlotCode : MonoBehaviour
{
    public GameObject generatorPrefab;
    public int plotPrice;
    private PanelControllerCode panelManager;
    private bool isPurchased = false;

    private void Start()
    {
        panelManager = FindObjectOfType<PanelControllerCode>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPurchased)
        {
            // Send the appropriate message based on plot price
            string message = plotPrice == 0 ? "Build?" : $"Build for {plotPrice} coins";
            panelManager.ShowPanelGenPlot(this, message);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isPurchased)
        {
            panelManager.HidePanelGenPlot();
        }
    }

    public void PurchasePlot()
    {
        if (!isPurchased && ResourceManagerCode.instance.SpendResource("coin", plotPrice))
        {
            isPurchased = true;

            GameObject newGenerator = Instantiate(generatorPrefab, transform.position, Quaternion.identity);

            panelManager.HidePanelGenPlot();
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Koin tidak cukup untuk beli plot ini.");
        }
    }
}
