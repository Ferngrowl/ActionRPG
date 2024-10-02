using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRendererSorter : MonoBehaviour
{
    // Base sorting order value. Higher values will be rendered in front of lower values.
    [SerializeField]
    private int sortingOrderBase = 5000;

    // Optional manual offset to adjust the sorting order for specific objects, set via the Inspector.
    [SerializeField]
    private int offset = 0;

    // If true, the script will run once and destroy itself. Otherwise, it will keep updating.
    [SerializeField]
    private bool runOnlyOnce = false;

    // Timer variables to control how often sorting is updated.
    private float timer;
    private float timerMax = .1f; // Sorting is updated every 0.1 seconds.

    // The Renderer component that controls the object's sorting order.
    private Renderer myRenderer;

    // Unique offset for each object, derived from the object's instance ID.
    private int instanceIdOffset;

    // Called when the object is initialized (before Start). Sets up the renderer and calculates the instance ID offset.
    private void Awake()
    {
        myRenderer = gameObject.GetComponent<Renderer>(); // Get the Renderer component attached to the object.

        // Generate a unique offset based on the instance ID. The instance ID is unique to every object.
        // Using % 100 limits the range of the offset between 0 and 99 to avoid too large a variation.
        instanceIdOffset = Mathf.Abs(gameObject.GetInstanceID()) % 100;
    }

    // Called once per frame after Update. This is where we update the sorting order.
    private void LateUpdate()
    {
        timer -= Time.deltaTime; // Reduce the timer by the time elapsed since the last frame.
        
        // If the timer reaches zero, it's time to update the sorting order.
        if (timer <= 0f)
        {
            timer = timerMax; // Reset the timer to ensure sorting is updated at regular intervals.

            // Calculate the sorting order:
            // - The primary factor is Y-position (multiplied by 10 to give more weight to Y differences).
            // - An instance ID offset is added to handle objects at the same Y-position.
            // - An additional manual offset can be set in the Inspector for special cases.
            myRenderer.sortingOrder = (int)(sortingOrderBase - transform.position.y * 10) + instanceIdOffset + offset;

            // If the script should only run once, destroy the script after the first sorting calculation.
            if (runOnlyOnce)
            {
                Destroy(this);
            }
        }
    }
}
