using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LocationTextHandler : MonoBehaviour
{
    public static LocationTextHandler Instance { get; private set; }
    public TextMeshProUGUI locationText; // The TMP text component for displaying location

    private string currentLocationName; // Current location name to be displayed
    private const float DefaultDisplayTime = 2f; // How long to display the location name

    void Awake()
    {
        // Ensure there's only one instance of this script and it persists across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Listen for scene load events
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Called when a scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!string.IsNullOrEmpty(currentLocationName))
        {
            ShowLocation(currentLocationName); // Display location text if set
        }
    }

    // Set location name and display time
    public void SetLocation(string locationName)
    {
        currentLocationName = locationName;
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


    void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
