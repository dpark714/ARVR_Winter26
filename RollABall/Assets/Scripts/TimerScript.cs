using UnityEngine;
using TMPro;

// 7g: countdown timer + "Time's up!"
public class TimerScript : MonoBehaviour
{
    public float startTime = 90f;
    public TextMeshProUGUI timerText;      // Shows "Time: 90"
    public GameObject timesUpText;         // "Time's up!" text
    public PlayerController player;
    public AudioSource failSound;          // NEW: fail sound (Play On Awake OFF)

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
                player.StopPlayer();       // also stops music + rolling sound
            if (failSound != null)
                failSound.Play();
        }

        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        // CeilToInt: shows "1" during the last second instead of already "0"
        if (timerText != null)
            timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining);
    }

    // Called by PlayerController when all pickups are collected
    public void StopTimer()
    {
        endGame = true;
    }
}