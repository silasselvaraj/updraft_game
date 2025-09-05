using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject ghostPlatformPrefab;
    public GameObject movingPlatformPrefab;
    public GameObject lowerKillerPrefab;
    public GameObject circularPlatformPrefab;
    public GameObject circularHazardPrefab; // <-- NEW prefab only for killing hazard
    public GameObject windRegionPrefab;
    public GameObject verticalHazardPrefab;
    
    



    public Transform player;
    public float spawnYOffset = 2.5f;
    public float levelWidth = 3.5f;
    public float destroyOffset = 13f;

    [Range(0f, 1f)] public float ghostChance = 0.2f;
    public int maxGhostsInRow = 2;

    [Range(0f, 1f)] public float movingPlatformChance = 0.2f;
    [Range(0f, 1f)] public float lower_killerPlatformChance = 0.2f;
    [Range(0f, 1f)] public float circularPlatformChance = 0.2f;
    [Range(0f, 1f)] public float windChance = 0.15f; 
    [Range(0f, 1f)] public float verticalHazardChance = 0.1f;
    private float lastSpawnY;
    private List<GameObject> activePlatforms = new List<GameObject>();
    private int consecutiveGhosts = 0;

    private ScoreManager scoreManager;

    void Start()
    {
        lastSpawnY = player.position.y;
        scoreManager = FindObjectOfType<ScoreManager>();

        for (int i = 0; i < 10; i++)
        {
            SpawnPlatform();
        }
    }

    void Update()
    {
        while (lastSpawnY < player.position.y + 15f)
        {
            SpawnPlatform();
        }

        for (int i = activePlatforms.Count - 1; i >= 0; i--)
        {
            if (activePlatforms[i].transform.position.y < Camera.main.transform.position.y - destroyOffset)
            {
                Destroy(activePlatforms[i]);
                activePlatforms.RemoveAt(i);
            }
        }
    }

    void SpawnPlatform()
    {
        float x = Random.Range(-levelWidth, levelWidth);
        lastSpawnY += spawnYOffset;

        GameObject prefabToSpawn = platformPrefab;
        bool spawnGhost = false;
        bool isNormalPlatform = true;

        // Check if ghost platforms are allowed (after 3 score)
        if (ScoreManager.score > 3)
        {
            spawnGhost = Random.value < ghostChance && consecutiveGhosts < maxGhostsInRow;
        }

        if (spawnGhost)
        {
            prefabToSpawn = ghostPlatformPrefab;
            consecutiveGhosts++;
            isNormalPlatform = false;
        }
        else
        {
            consecutiveGhosts = 0;
            isNormalPlatform = true;

            bool forceOnlyMoving = false;
            float currentMoveChance = 0f;

            bool isInMovingRange = ScoreManager.score >= 35 && ScoreManager.score % 10 >= 5 && ScoreManager.score % 10 <= 9;

            if (isInMovingRange && Random.value < 0.3f)
            {
                forceOnlyMoving = true;
            }

            if (forceOnlyMoving)
            {
                prefabToSpawn = movingPlatformPrefab;
                isNormalPlatform = false;
            }
            else
            {
                if (ScoreManager.score > 20)
                    currentMoveChance = 0.4f;
                else if (ScoreManager.score > 10)
                    currentMoveChance = 0.2f;

                if (Random.value < currentMoveChance)
                {
                    prefabToSpawn = movingPlatformPrefab;
                    isNormalPlatform = false;
                }
            }
        }
        if(ScoreManager.score>15 && Random.value < circularPlatformChance)
        {
            prefabToSpawn = circularPlatformPrefab;
            isNormalPlatform = false;
        }

        // Spawn the main platform
        Vector3 platformPos = new Vector3(x, lastSpawnY, 0);
        GameObject newPlatform = Instantiate(prefabToSpawn, platformPos, Quaternion.identity);
        activePlatforms.Add(newPlatform);

        if (prefabToSpawn == movingPlatformPrefab && ScoreManager.score > 20)
        {
            var moveScript = newPlatform.GetComponent<MovingPlatform>();
            if (moveScript != null)
            {
                float difficultyMultiplier = 1f + (ScoreManager.score * 0.01f);
                moveScript.speed = Random.Range(3f, 9f) * difficultyMultiplier;
                moveScript.moveRange = Random.Range(3f, 7f);
            }
        }


        // SPAWN LOWER KILLER (only below normal platforms, after score >= 25)
        // SPAWN LOWER KILLER (after score >= 25)
        if (ScoreManager.score >= 25 && Random.value < lower_killerPlatformChance)
        {
            Vector3 lowerPos = platformPos + new Vector3(0f, -0.3f, 0f); // bigger offset
            GameObject killer = Instantiate(lowerKillerPrefab, lowerPos, Quaternion.identity);

            // Make the killer follow the same parent/platform if needed
            killer.transform.SetParent(newPlatform.transform);
        }


        // SPAWN CIRCULAR HAZARD (after score >= 30)
        if (ScoreManager.score >= 30 && Random.value < 0.25f)
        {
            Vector3 hazardPos = new Vector3(-levelWidth, lastSpawnY + 1.5f, 0); // Starts from left
            GameObject hazard = Instantiate(circularHazardPrefab, hazardPos, Quaternion.identity);

            CircularHazard hazardScript = hazard.GetComponent<CircularHazard>();
            if (hazardScript != null)
            {
                float difficultyMultiplier = 1f + (ScoreManager.score * 0.01f);
                hazardScript.moveDistance = levelWidth * 2f;
                hazardScript.speed = Random.Range(2f, 6f) * difficultyMultiplier;
            }

        }
        // SPAWN WIND REGION (after score >= 50)
        if (ScoreManager.score >= 5 && Random.value < windChance)
        {
            Vector3 windPos = new Vector3(0f, lastSpawnY + 2f, 0f); // center on screen, above platform
            Instantiate(windRegionPrefab, windPos, Quaternion.identity);
        }

        // --- Spawn vertical hazard bar (full screen) ---
        // SPAWN VERTICAL HAZARD (after score >= 40)
        // SPAWN VERTICAL HAZARD (after score >= 40, 15% chance)
        // SPAWN VERTICAL HAZARD (after score >= 40)
        if (ScoreManager.score >= 2 && Random.value < verticalHazardChance)
        {
            // Pick a random X inside level width
            float hazardX = Random.Range(-levelWidth, levelWidth);

            // Position at camera center Y (hazard script will follow vertically)
            Vector3 spawnPos = new Vector3(hazardX, Camera.main.transform.position.y, 0);

            Instantiate(verticalHazardPrefab, spawnPos, Quaternion.identity);
        }





    }
}
