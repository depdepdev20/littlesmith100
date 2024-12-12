using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField]
    private GameObject destroyArea;

    private void Start()
    {
        Debug.Log(destroyArea != null ? destroyArea.name : "DestroyArea not assigned!");
    }
}