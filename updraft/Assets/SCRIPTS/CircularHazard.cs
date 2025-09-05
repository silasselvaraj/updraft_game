using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CircularHazard : MonoBehaviour
{
    public float speed = 5f;
    public float moveDistance = 10f;
    private Vector3 startPos;
    private int direction = 1;
    private bool isDead = false; // prevent multiple triggers

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (isDead) return;

        float difficultyMultiplier = 1f + (ScoreManager.score * 0.01f);
        float currentSpeed = speed * difficultyMultiplier;
        transform.Translate(Vector3.right * currentSpeed * direction * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - startPos.x) >= moveDistance)
        {
            direction *= -1; // Reverse direction
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            StartCoroutine(DeathSequence(other.gameObject));
        }
    }

    IEnumerator DeathSequence(GameObject player)
    {
        isDead = true;

        // Optionally freeze player
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        // Shake camera
        yield return StartCoroutine(CameraShake(0.4f, 0.25f));

        // Reload scene
        SceneManager.LoadScene("Retry");
    }

    IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalPos = Camera.main.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.localPosition = originalPos;
    }
}
