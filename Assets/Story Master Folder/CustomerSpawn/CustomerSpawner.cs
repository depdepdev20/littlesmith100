using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomerSchedule
{
    public int startDay; // First day to spawn customers
    public int endDay; // Last day to spawn customers
    public GameObject[] customerModels; // Array of customer models to spawn
    public Transform[] destinations; // Destinations to visit
    public Transform destructionPoint; // Final point where the customer will be destroyed
    public float idleDuration = 2f; // Time spent idling at each destination
    public float spawnInterval = 3f; // Time between spawns
    public int maxCustomerCount = 5; // Maximum number of customers to spawn
}

public class CustomerSpawner : MonoBehaviour
{
    [Header("Customer Schedules")]
    [SerializeField] private List<CustomerSchedule> customerSchedules;

    private List<GameObject> activeCustomers = new List<GameObject>();
    private bool isSpawning = false;

    private void Update()
    {
        int currentDay = TimeManager.Instance.GetTimestamp().day;

        foreach (var schedule in customerSchedules)
        {
            if (currentDay >= schedule.startDay && currentDay <= schedule.endDay)
            {
                if (!isSpawning)
                {
                    StartCoroutine(SpawnCustomers(schedule));
                }
            }
        }
    }

    private IEnumerator SpawnCustomers(CustomerSchedule schedule)
    {
        isSpawning = true;

        while (activeCustomers.Count < schedule.maxCustomerCount)
        {
            GameObject customer = SpawnCustomer(schedule);
            if (customer != null)
            {
                StartCoroutine(HandleCustomerMovement(customer, schedule));
            }

            yield return new WaitForSeconds(schedule.spawnInterval);
        }

        isSpawning = false;
    }

    private GameObject SpawnCustomer(CustomerSchedule schedule)
    {
        if (schedule.customerModels.Length == 0)
        {
            Debug.LogError("No customer models assigned!");
            return null;
        }

        GameObject selectedModel = schedule.customerModels[Random.Range(0, schedule.customerModels.Length)];
        GameObject newCustomer = Instantiate(selectedModel, transform.position, Quaternion.identity);

        if (newCustomer != null)
        {
            activeCustomers.Add(newCustomer);
        }

        return newCustomer;
    }

    private IEnumerator HandleCustomerMovement(GameObject customer, CustomerSchedule schedule)
    {
        CustomerWalk customerWalk = customer.GetComponent<CustomerWalk>();
        if (customerWalk == null)
        {
            Debug.LogError("Customer does not have a CustomerWalk component!");
            yield break;
        }

        List<Transform> shuffledDestinations = new List<Transform>(schedule.destinations);
        ShuffleList(shuffledDestinations);

        foreach (var destination in shuffledDestinations)
        {
            if (!MoveToDestination(customerWalk, destination))
            {
                Debug.Log($"{customer.name} could not reach {destination.name} within 10 seconds, moving to the next destination.");
                continue;
            }

            yield return new WaitForSeconds(schedule.idleDuration);
        }

        // Move to destruction point
        if (MoveToDestination(customerWalk, schedule.destructionPoint))
        {
            yield return new WaitForSeconds(schedule.idleDuration);
        }

        DestroyCustomer(customer);
    }

    private bool MoveToDestination(CustomerWalk customerWalk, Transform destination)
    {
        customerWalk.SetDestination(destination.position);
        float timer = 0f;

        while (timer < 10f && !customerWalk.HasReachedDestination())
        {
            timer += Time.deltaTime;
        }

        return customerWalk.HasReachedDestination();
    }

    private void DestroyCustomer(GameObject customer)
    {
        activeCustomers.Remove(customer);
        Destroy(customer);
        Debug.Log($"{customer.name} has been destroyed.");
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
