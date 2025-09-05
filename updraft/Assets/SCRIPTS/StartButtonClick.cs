using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonClick : MonoBehaviour
{
    private Vector3 originalScale;
    public float zoomAmplitude = 0.1f; // How much it grows/shrinks
    public float zoomSpeed = 2f;       // How fast it zooms

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float scaleFactor = 1 + Mathf.Sin(Time.time * zoomSpeed) * zoomAmplitude;
        transform.localScale = originalScale * scaleFactor;
    }

    void OnMouseDown()
    {
        Debug.Log("Start button clicked!");
        SceneManager.LoadScene("START");
    }
}
