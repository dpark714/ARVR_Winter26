using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 10f;
    public AudioSource pickupSound; // 7a: AudioSource with the pickup clip (Play On Awake OFF)

    // For Score
    [Header("Score")]
    public TMPro.TextMeshProUGUI scoreText; // 7f
    public int score = 0;

    // For confetti
    [Header("Confetti")]
    public ParticleSystem confettiEffect;   // 7j
    public TimerScript timerScript;         // To stop the timer on win
    private int totalPickups;

    // For winning
    [Header("Win")]
    public GameObject winText;              // 7i: "Congratulations!" text object
    public ParticleSystem fireworks;        // 7j
    public AudioSource winSound;            // success jingle (Play On Awake OFF)

    // NEW: music and rolling sound
    [Header("Music & Rolling Sound")]
    public AudioSource backgroundMusic;     // Loop ON, Play On Awake ON
    public AudioSource rollingSound;        // Loop ON, Play On Awake ON, Volume 0
    public float maxRollSpeed = 8f;         // Speed at which the rolling sound is at full volume

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false); // Deactivate the pickup object

            // PlayOneShot lets sounds overlap when collecting several pickups quickly
            if (pickupSound != null)
                pickupSound.PlayOneShot(pickupSound.clip);

            score = score + 1;
            UpdateScoreText();

            if (score >= totalPickups)
            {
                Debug.Log("WIN! score " + score + " of " + totalPickups);

                if (confettiEffect != null)
                {
                    confettiEffect.transform.position = transform.position;
                    confettiEffect.Play();
                }

                if (winText != null)
                    winText.SetActive(true);

                if (fireworks != null)
                    fireworks.Play();

                if (timerScript != null)
                    timerScript.StopTimer();

                // NEW: swap the music for the success sound
                if (backgroundMusic != null)
                    backgroundMusic.Stop();
                if (winSound != null)
                    winSound.Play();
            }
        }
    }

    void Start()
    {
        if (winText != null)
            winText.SetActive(false); // Hide "Congratulations!" at the start

        rb = GetComponent<Rigidbody>();

        // Count only real pickups (tagged AND with a collider)
        totalPickups = 0;
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("PickUp"))
        {
            if (p.GetComponent<Collider>() != null)
            {
                totalPickups++;
            }
        }

        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score + " / " + totalPickups;
        }
    }

    // Called by TimerScript when the time is up
    public void StopPlayer()
    {
        rb.linearVelocity = Vector3.zero;   // Use rb.velocity on older Unity versions
        rb.angularVelocity = Vector3.zero;

        // NEW: silence the rolling sound and the music
        if (rollingSound != null)
            rollingSound.Stop();
        if (backgroundMusic != null)
            backgroundMusic.Stop();

        enabled = false;
    }

    private void FixedUpdate()
    {
        float moveX = 0f;
        float moveZ = 0f;

        // Read keyboard input using the modern Input System
        if (Keyboard.current != null)
        {
            // Left/right movement
            if (Keyboard.current.leftArrowKey.isPressed)
                moveX = 1f;
            else if (Keyboard.current.rightArrowKey.isPressed)
                moveX = -1f;

            // Forward/backward movement
            if (Keyboard.current.upArrowKey.isPressed)
                moveZ = -1f;
            else if (Keyboard.current.downArrowKey.isPressed)
                moveZ = 1f;
        }

        Vector3 movement = new Vector3(moveX, 0f, moveZ);
        rb.AddForce(movement * speed, ForceMode.Force);

        UpdateRollingSound();
    }

    // rolling sound gets louder and higher the faster the ball rolls,
    // and goes quiet when the ball stands still or is in the air.
    void UpdateRollingSound()
    {
        if (rollingSound == null) return;

        bool grounded = Physics.Raycast(transform.position, Vector3.down, 0.6f);
        float speedPercent = Mathf.Clamp01(rb.linearVelocity.magnitude / maxRollSpeed);

        float targetVolume = grounded ? speedPercent : 0f;
        rollingSound.volume = Mathf.Lerp(rollingSound.volume, targetVolume, 10f * Time.fixedDeltaTime);
        rollingSound.pitch = 0.8f + speedPercent * 0.5f;
    }
}