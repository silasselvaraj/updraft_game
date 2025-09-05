using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerBounce : MonoBehaviour
{
    public float bounceForce = 10f;
    public float moveSpeed = 5f;

    public float acceleration = 15f;   // How quickly we speed up
    public float deceleration = 8f;    // How quickly we slow down

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        float difficultyMultiplier = 1f + (ScoreManager.score * 0.01f);
        // every 100 points → +100% speed

        float horizontalInput = Input.GetAxis("Horizontal");
        float targetSpeed = horizontalInput * moveSpeed * difficultyMultiplier;
        float speedDiff = targetSpeed - rb.linearVelocity.x;

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, accelRate * Time.deltaTime);

        rb.linearVelocity = new Vector2(movement, rb.linearVelocity.y);

        WrapAroundScreen();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("platform"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("KillFromBelow"))
        {
            if (transform.position.y < other.transform.position.y)
            {
                StartCoroutine(DeathSequence());
            }
        }
    }

    IEnumerator DeathSequence()
    {
        yield return StartCoroutine(CameraShake(0.3f, 0.2f)); // (duration, magnitude)
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

    void WrapAroundScreen()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewPos.x > 1f)
            viewPos.x = 0f;
        else if (viewPos.x < 0f)
            viewPos.x = 1f;

        transform.position = Camera.main.ViewportToWorldPoint(viewPos);
    }
}
