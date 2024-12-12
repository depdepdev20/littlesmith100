using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CanvasChooseMaterialCode : MonoBehaviour
{

    public GameObject panelChooseMaterial;

    private bool isChooseMaterialPanelActive = false;
    private bool isPlayerNear = false;
    private MaterialGeneratorCode currentGen;

    private void Start()
    {
        panelChooseMaterial.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            TogglePanel(panelChooseMaterial, ref isChooseMaterialPanelActive);
            Debug.Log("Active");
        }
    }

    void TogglePanel(GameObject panel, ref bool isActive)
    {
        isActive = !isActive;
        panel.SetActive(isActive);
    }

    public void ShowPanelGen(Vector3 position, MaterialGeneratorCode gen)
    {
        currentGen = gen;
        panelChooseMaterial.SetActive(true);
        /* panelBuildGenerator.transform.position = position + new Vector3(0, 2, 0); */
    }

    public void HidePanelGen()
    {
        panelChooseMaterial.SetActive(false);
        isChooseMaterialPanelActive = false;
        currentGen = null;
    }


    public void SetPlayerNear(bool isNear, MaterialGeneratorCode gen)
    {
        isPlayerNear = isNear;
        currentGen = isNear ? gen : null;

        if (!isNear)
        {
            panelChooseMaterial.SetActive(false);
            isChooseMaterialPanelActive = false;
        }
    }
}
