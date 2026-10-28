using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 10f;
    public AudioSource pickupSound; // Reference to the AudioSource component for the pickup sound effect

    //For Score
    [Header("Score")]
    public TMPro.TextMeshProUGUI scoreText; // Reference to the TextMeshProUGUI component for displaying the score
    public int score = 0; // Player's score

    // For confetti
    [Header("Confetti")]
    public ParticleSystem confettiEffect; // Reference to the ParticleSystem component for the confetti effect
    public TimerScript timerScript; // Reference to the TimerScript component to check if time is up
    private int totalPickups; // Total number of pickups in the scene
    //For winning
    [Header("Win")]
    public GameObject winText; // Reference to the "You Win!" text object
    public ParticleSystem fireworks; // Reference to the ParticleSystem component for the confetti effect
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false); // Deactivate the pickup object
            pickupSound.Play();
            
            score = score + 1;
            UpdateScoreText(); // Update the score display

            if (score >= totalPickups)
        
            {
                Debug.Log("WIN! score " + score + " of " + totalPickups);
                if (confettiEffect != null)
                {
                    confettiEffect.transform.position = transform.position; // Set the position of the confetti effect to the player's position
                    confettiEffect.Play(); // Play the confetti effect when all pickups are collected
            
                }
            if (winText != null)
                    winText.SetActive(true); 

            if (fireworks != null)
                fireworks.Play(); // Play the fireworks effect when all pickups are collected
            
            if (timerScript != null)
                    timerScript.StopTimer();
            }
        }
    }
    void Start()
    {
        if (winText != null)
            winText.SetActive(false); // Hide "You Win!" at the start

        rb = GetComponent<Rigidbody>();

        // Count only real pickups (tagged AND with a collider)
        totalPickups = 0;
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("PickUp"))
        {
            if (p.GetComponent<Collider>() != null)
            {
                totalPickups++;
                Debug.Log("Pickup found: " + p.name, p); // click this message to find the object
            }
        }

        UpdateScoreText(); // Initialize the score display
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score; // Update the score text
        }
    }

    public void StopPlayer()
    {
        rb.linearVelocity = Vector3.zero;   // Use rb.velocity on older Unity versions
        rb.angularVelocity = Vector3.zero;
        enabled = false;
    }

    // Update is called once per frame
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

        // Create a movement vector based on the input
        Vector3 movement = new Vector3(moveX, 0f, moveZ);
        rb.AddForce(movement*speed, ForceMode.Force);
    }
    
}
