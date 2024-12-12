using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
{
    //private NPCBuyer npcBuyer; // Reference to NPCBuyer script

    //[SerializeField]
    //public GameObject[] buyingAreaPrefabs; // Prefabs of the buying areas
    //[SerializeField]
    //public Transform destroyArea; // Area to go when the NPC's money is depleted
    
    //[HideInInspector] private NavMeshAgent navAgent; // To handle the NPC movement
    //[HideInInspector] private int currentAreaIndex = 0; // To track the current buying area
    //[HideInInspector] private Transform[] buyingAreas; // Store instantiated buying areas

    //private void Start()
    //{
    //    npcBuyer = GetComponent<NPCBuyer>();
    //    navAgent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component

    //    // Instantiate the prefabs and store their transforms
    //    buyingAreas = new Transform[buyingAreaPrefabs.Length];
    //    for (int i = 0; i < buyingAreaPrefabs.Length; i++)
    //    {
    //        GameObject areaInstance = Instantiate(buyingAreaPrefabs[i]);
    //        buyingAreas[i] = areaInstance.transform;
    //    }

    //    MoveToNextBuyingArea();
    //}

    //private void Update()
    //{
    //    // If NPC is near the current buying area, try to purchase items
    //    if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
    //    {
    //        TryToBuyItems();
    //    }
    //}

    //// Move the NPC to the next buying area
    //private void MoveToNextBuyingArea()
    //{
    //    if (currentAreaIndex < buyingAreas.Length)
    //    {
    //        navAgent.SetDestination(buyingAreas[currentAreaIndex].position); // Set destination to next buying area
    //    }
    //    else
    //    {
    //        // No more areas to visit, go to destroy area
    //        navAgent.SetDestination(destroyArea.position);
    //    }
    //}

    //// Try to buy items at the current buying area
    //private void TryToBuyItems()
    //{
    //    int itemPrice = 10; // Example item price
    //    if (npcBuyer.CanAfford(itemPrice))
    //    {
    //        npcBuyer.PurchaseItem(itemPrice); // NPC buys an item if they can afford it
    //        Debug.Log($"{gameObject.name} bought an item at {buyingAreas[currentAreaIndex].name}");
    //    }

    //    // If the NPC's money is depleted, send them to the destroy area
    //    if (npcBuyer.GetMoney() <= 0)
    //    {
    //        Debug.Log($"{gameObject.name} has no more money and is leaving.");
    //        navAgent.SetDestination(destroyArea.position); // Move to destroy area
    //    }
    //    else
    //    {
    //        // Move to the next buying area after a short delay
    //        currentAreaIndex++;
    //        if (currentAreaIndex < buyingAreas.Length)
    //        {
    //            MoveToNextBuyingArea();
    //        }
    //        else
    //        {
    //            // All areas are visited, now head to the destroy area
    //            MoveToDestroyArea();
    //        }
    //    }
    //}

    //// Move the NPC to the destroy area
    //private void MoveToDestroyArea()
    //{
    //    navAgent.SetDestination(destroyArea.position);
    //}
}
