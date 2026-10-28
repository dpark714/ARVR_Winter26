using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public GameObject player; // Reference to the player object
    public Vector3 offset; // Offset from the player position
    void Start()
    {
        offset = transform.position - player.transform.position; // Calculate the initial offset
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset; // Update the camera position based on the player's position and the offset
    }
}
