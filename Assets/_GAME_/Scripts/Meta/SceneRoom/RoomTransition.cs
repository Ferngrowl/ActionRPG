using UnityEngine;
using Cinemachine;

public class RoomTransition : MonoBehaviour
{
    public Vector2 playerOffset;  // How much to move the player
    public PolygonCollider2D newConfiner;  // New collider for the room
    private CinemachineConfiner camConfiner;
    private CinemachineVirtualCamera cam;

    void Start()
    {
        cam = FindObjectOfType<CinemachineVirtualCamera>();
        camConfiner = cam.GetComponent<CinemachineConfiner>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Move the player
            other.transform.position += (Vector3)playerOffset;

            // Change the confiner collider to the new room's collider
            camConfiner.m_BoundingShape2D = newConfiner;
            camConfiner.InvalidatePathCache();  // Clears the cache for the new collider to take effect
        }
    }
}
