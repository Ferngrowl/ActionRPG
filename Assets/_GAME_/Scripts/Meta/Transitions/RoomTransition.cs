using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class RoomTransition : MonoBehaviour
{
    

    //room transition variables
    public Vector2 playerOffset;  // How much to move the player
    private CinemachineVirtualCamera cam;
    public PolygonCollider2D newConfiner;  // New collider for the room
    private CinemachineConfiner camConfiner; 

    //location text vairables
    public bool needText; // dependant on whether or not the area being moved too is a different biome or location rather than a different room in the same place
    public string locationName;

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
            
            //If text is needed show location text for x seconds
            if (needText)
            {
                LocationTextHandler.Instance.ShowLocation(locationName);
            }

        }
    }

}
