using UnityEngine;

[CreateAssetMenu(fileName = "LocationInfo", menuName = "ScriptableObjects/LocationInfo", order = 1)]
public class LocationInfo : ScriptableObject
{
    public string locationName; // Stores the location name

    // Clear the stored location name
    public void Clear()
    {
        locationName = string.Empty;
    }
}
