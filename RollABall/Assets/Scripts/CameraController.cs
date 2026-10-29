using UnityEngine;

// Minimal camera follow + Bonus 7e: soft (smoothed) camera movement.
public class CameraController : MonoBehaviour
{
    public GameObject player;   // Reference to the player object
    private Vector3 offset;     // Offset from the player position (calculated in Start)

    [Header("7e - Soft follow")]
    [Tooltip("Higher = softer/slower follow. 0 = hard follow like the minimal version.")]
    public float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero; // Used internally by SmoothDamp

    void Start()
    {
        offset = transform.position - player.transform.position; // Calculate the initial offset
    }

    void LateUpdate()
    {
        Vector3 targetPosition = player.transform.position + offset;
        // Glide towards the target instead of jumping there instantly
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}