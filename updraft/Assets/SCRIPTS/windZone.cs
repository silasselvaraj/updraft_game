using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class WindRegion : MonoBehaviour
{
    [Header("Wind strength")]
    public float minForce = 4f;
    public float maxForce = 10f;

    private float windForce;
    private int windDirection; // -1 = left, +1 = right

    void Start()
    {
        // Randomize direction and force
        windDirection = Random.value < 0.5f ? -1 : 1;
        windForce = Random.Range(minForce, maxForce);

        Debug.Log($"[WindRegion] Active → Direction: {(windDirection == 1 ? "Right" : "Left")} | Force: {windForce}");
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.attachedRigidbody;
            if (rb != null)
            {
                // Apply constant acceleration (ignores mass)
                rb.AddForce(new Vector2(windDirection * windForce * rb.mass, 0f), ForceMode2D.Force);

            }
        }
    }
}
