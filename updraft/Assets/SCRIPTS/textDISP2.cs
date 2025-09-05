using UnityEngine;

public class DisplayMessage : MonoBehaviour
{
    public TextMesh textMesh;
    private Vector3 originalScale;
    public float zoomAmplitude = 0.1f; // How much it grows/shrinks
    public float zoomSpeed = 2f;       // How fast it zooms

    void Start()
    {
        textMesh.text = "SCORE:" + ScoreManager.score;
        originalScale = transform.localScale;
    }

    void Update()
    {
        float scaleFactor = 1 + Mathf.Sin(Time.time * zoomSpeed) * zoomAmplitude;
        transform.localScale = originalScale * scaleFactor;
    }
}
