using UnityEngine;

public class GhostPlatformFlicker : MonoBehaviour
{
    private SpriteRenderer sr;
    public float flickerSpeed = 5f; // How fast it flickers
    public float minAlpha = 0.2f;
    public float maxAlpha = 0.5f;

    private float t;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        t = Random.Range(0f, Mathf.PI * 2); // Random phase offset for variety
    }

    void Update()
    {
        t += Time.deltaTime * flickerSpeed;
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(t) + 1f) / 2f);

        if (sr != null)
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
    }
}
