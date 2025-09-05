using UnityEngine;

public class PushPlatform : MonoBehaviour
{
    public float pushForce = 10f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero; // reset velocity for consistent push
                rb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
            }
        }
    }
}
