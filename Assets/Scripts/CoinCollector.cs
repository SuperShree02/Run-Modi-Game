using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    public TMP_Text coinText; // Text showing coins during gameplay
    public TMP_Text gameOverCoinText; // Text for coins in Game Over UI
    public GameObject gameOverPanel; // Game Over panel

    private int coins = 0; // Counter for coins

    void Start()
    {
        coinText.text = "0"; // Initialize coin count
        gameOverPanel.SetActive(false); // Hide Game Over panel initially
    }

    public void AddCoin(int amount)
    {
        // Increase coin count and update gameplay UI
        coins += amount;
        coinText.text = "" + coins;
    }

    public void GameOver()
    {
        // Show Game Over UI
        gameOverPanel.SetActive(true);
        gameOverCoinText.text = "Votes: " + coins; // Show final coins collected
        Time.timeScale = 0; // Pause the game
    }

    // Detect obstacle collision to trigger Game Over
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            GameOver(); // Trigger Game Over when hitting an obstacle
        }
    }
}
