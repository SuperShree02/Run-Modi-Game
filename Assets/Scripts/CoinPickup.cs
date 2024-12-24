using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    private CoinCollector coinCollector;

    void Start()
    {
        // Find the CoinCollector attached to the player
        coinCollector = GameObject.FindGameObjectWithTag("Player").GetComponent<CoinCollector>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            coinCollector.AddCoin(1); // Increase coin count by 1
            Destroy(gameObject); // Destroy the coin after collection
        }
    }
}
