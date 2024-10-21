using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public Vector2 playerPosition;
    public VectorValue playerPosMemory;
    public GameObject fadeInPanel, fadeOutPanel;
    public float fadeWait;
    public PolygonCollider2D newConfiner;

    void Awake() {
        if (fadeInPanel) Destroy(Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity), 1);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerPosMemory.initialValue = playerPosition;
            StartCoroutine(TransitionScene());
        }
    }

    IEnumerator TransitionScene() {
        if (fadeOutPanel) Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(fadeWait);

        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        // Move player to the saved position
        GameObject.FindGameObjectWithTag("Player").transform.position = playerPosMemory.initialValue;

        // Update Cinemachine confiner in the new scene
        CinemachineConfiner confiner = FindObjectOfType<CinemachineConfiner>();
        if (confiner) {
            confiner.m_BoundingShape2D = newConfiner;
            confiner.InvalidatePathCache();
        }
    }
}
