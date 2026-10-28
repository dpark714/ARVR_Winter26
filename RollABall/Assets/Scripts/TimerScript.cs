using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public float startTime = 90f;
    public TextMeshProUGUI timerText;      // Shows "Time: 90"
    public GameObject timesUpText;         // "Time's up!" text
    public PlayerController player;

    private float timeRemaining;
    private bool endGame = false;

    void Start()
    {
        timeRemaining = startTime;
        if (timesUpText != null)
            timesUpText.SetActive(false);  // Hide "Time's up!" at the start
        UpdateTimerText();
    }

    void Update()
    {
        if (endGame) return; // Timer stopped

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            endGame = true;
            if (timesUpText != null)
                timesUpText.SetActive(true);
            if (player != null)
                player.StopPlayer();
        }

        UpdateTimerText(); // Show the new time every frame
    }

    void UpdateTimerText()
    {
        if (timerText != null)
            timerText.text = "Time: " + timeRemaining.ToString("F0");
    }

    // Called by PlayerController when all pickups are collected
    public void StopTimer()
    {
        endGame = true;
    }
}