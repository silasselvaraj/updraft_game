using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public float moveRange = 2f;

    private Vector3 startPos;
    private int direction = 1;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float difficultyMultiplier = 1f + (ScoreManager.score * 0.01f);
        float currentSpeed = speed * difficultyMultiplier;
        transform.Translate(Vector2.right * currentSpeed * direction * Time.deltaTime);

        if (Vector2.Distance(startPos, transform.position) >= moveRange)
        {
            direction *= -1;
        }
    }
}
