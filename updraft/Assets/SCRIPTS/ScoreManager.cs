using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public Transform player;           // Drag player here in Inspector
    public TextMesh scoreText;         // Drag your TextMesh object here

    


    private float highestY = 0f;
    static public int score = 0;
    void Update()
    {
        if (player.position.y > highestY)
        {
            highestY = player.position.y/10;
            score = Mathf.FloorToInt(highestY);
            scoreText.text = "SCORE: " + score.ToString();

        }
    }
    public static class SceneData
    {
        public static int finalScore = 0;
    }

}
