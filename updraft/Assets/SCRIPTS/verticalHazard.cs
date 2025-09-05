using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class VerticalHazard : MonoBehaviour
{
    [Header("Flicker / activation")]
    public float flickerDuration = 2f;
    public float flickerInterval = 0.12f;
    public float minTime = 1;
    public float maxTime = 4;// how long it stays before disappearing

    [Header("Colors")]
    public Color warningColor = new Color(1f, 0.2f, 0.2f, 0.7f);
    public Color activeColor = Color.red;

    private SpriteRenderer sr;
    private BoxCollider2D col;
    private bool isActive = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }

    void Start()
    {
        // Match camera height
        float camHeight = Camera.main.orthographicSize * 2f;
        float spriteHeight = sr.sprite.bounds.size.y;

        // Scale so the sprite covers the screen vertically
        float scaleY = camHeight / spriteHeight;
        transform.localScale = new Vector3(transform.localScale.x, scaleY, 1f);

        col.size = new Vector2(1f, 1f); // collider respects scale
        col.offset = Vector2.zero;
        col.enabled = false;

        StartCoroutine(FlickerAndActivate());
    }

    IEnumerator FlickerAndActivate()
    {
        float elapsed = 0f;
        bool visible = true;
        sr.color = warningColor;

        while (elapsed < flickerDuration)
        {
            visible = !visible;
            sr.enabled = visible;
            yield return new WaitForSeconds(flickerInterval);
            elapsed += flickerInterval;
        }

        sr.enabled = true;
        sr.color = activeColor;
        col.enabled = true;
        isActive = true;

        // Stay active, then disappear
        yield return new WaitForSeconds(Random.Range(minTime,maxTime));
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Retry");
        }
    }

    void Update()
    {
        // Keep hazard following camera vertically
        transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
    }
}
