using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    public TMP_Text coinText;  // Reference to the TextMeshPro UI text
    private int coinCount = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);
            coinText.text = "" + coinCount; // Update UI
        }
    }
}
