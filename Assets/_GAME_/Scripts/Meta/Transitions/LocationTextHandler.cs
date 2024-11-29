using System.Collections;
using UnityEngine;
using TMPro;

public class LocationTextHandler : MonoBehaviour
{
    public TextMeshProUGUI locationText; // The TMP text component for displaying location
    public LocationInfo locationInfo;   // Reference to the ScriptableObject
    private const float DefaultDisplayTime = 2f; // How long to display the location name

    void Start()
    {
        // Check if there's a location name in the ScriptableObject
        if (!string.IsNullOrEmpty(locationInfo.locationName))
        {
            ShowLocation(locationInfo.locationName); // Display it
            locationInfo.Clear(); // Clear the location name after displaying
        }
    }

    // Show location text with fade in/out effect
    public void ShowLocation(string locationName)
    {
        StartCoroutine(FadeText(locationName));
    }

    private IEnumerator FadeText(string locationName)
    {
        locationText.text = locationName;

        // Fade in: gradually increase alpha from 0 to 1
        float fadeDuration = 1f;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            locationText.color = new Color(locationText.color.r, locationText.color.g, locationText.color.b, alpha);
            yield return null;
        }

        // Ensure text is fully visible after fade-in
        locationText.color = new Color(locationText.color.r, locationText.color.g, locationText.color.b, 1);

        // Wait for the standardized display time
        yield return new WaitForSeconds(DefaultDisplayTime);

        // Fade out: gradually decrease alpha from 1 to 0
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            locationText.color = new Color(locationText.color.r, locationText.color.g, locationText.color.b, alpha);
            yield return null;
        }

        // Ensure text is fully transparent after fade-out
        locationText.color = new Color(locationText.color.r, locationText.color.g, locationText.color.b, 0);
    }
}
