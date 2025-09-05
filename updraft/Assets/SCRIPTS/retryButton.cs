using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{

    void OnMouseDown()
    {
        ScoreManager.score = 0;
        Debug.Log("Start button clicked!");
        SceneManager.LoadScene("START");
    }
}
