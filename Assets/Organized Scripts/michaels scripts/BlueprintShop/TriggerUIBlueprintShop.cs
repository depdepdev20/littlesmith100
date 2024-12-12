using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerUIBlueprintShop : MonoBehaviour
{
    public GameObject canvasUiBlueprintShop;
    private bool isUiBlueprintShopActive = false;
    private bool isPlayerNear = false;

    private void Start()
    {
        canvasUiBlueprintShop.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            ToggleCanvas(canvasUiBlueprintShop, ref isUiBlueprintShopActive);
            Debug.Log("Active");
        }
    }

    void ToggleCanvas(GameObject canvas, ref bool isActive)
    {
        isActive = !isActive;
        canvas.SetActive(isActive);
    }

    public void HideCanvasUi()
    {
        canvasUiBlueprintShop.SetActive(false);
        isUiBlueprintShopActive = false;
    }


    public void SetPlayerNear(bool isNear)
    {
        isPlayerNear = isNear;

        if (!isNear)
        {
            canvasUiBlueprintShop.SetActive(false);
            isUiBlueprintShopActive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetPlayerNear(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetPlayerNear(false);
        }
    }
}
