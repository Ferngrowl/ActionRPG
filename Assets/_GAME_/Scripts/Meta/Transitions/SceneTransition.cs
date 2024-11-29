using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public Vector2 playerPosition;
    public VectorValue playerPosMemory;
    public GameObject fadeInPanel, fadeOutPanel;
    public float fadeWait;
    public LocationInfo locationInfo;
    public string locationName; // The name of the location for the new scene

    void Awake()
    {
        if (fadeInPanel) Destroy(Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity), 1);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPosMemory.initialValue = playerPosition;

            // Set location info in LocationTextHandler before transitioning
            locationInfo.locationName = locationName;
            
            StartCoroutine(TransitionScene());
        }
    }

    IEnumerator TransitionScene()
    {
        if (fadeOutPanel) Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(fadeWait);

        yield return SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
