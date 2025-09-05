using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    private float highestY;

    public float deathOffset = 6f; // Distance below camera that triggers death
    private bool isDead = false;   // Prevent multiple triggers

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        highestY = transform.position.y;
    }

    void LateUpdate()
    {
        if (isDead) return; // stop updating once death starts

        if (player.position.y > highestY)
        {
            highestY = player.position.y;
            transform.position = new Vector3(0, highestY, -10f);
        }

        // Death check
        if (player.position.y < transform.position.y - deathOffset)
        {
            StartCoroutine(DeathSequence());
        }
    }

    IEnumerator DeathSequence()
    {
        isDead = true;

        // Optionally freeze player movement
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        // Shake the camera
        yield return StartCoroutine(CameraShake(0.4f, 0.25f)); // duration, magnitude

        // Reload scene after shake
        SceneManager.LoadScene("Retry");
    }

    IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
