using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleOver : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            Time.timeScale = 0; // Pause game
        }
    }
}
