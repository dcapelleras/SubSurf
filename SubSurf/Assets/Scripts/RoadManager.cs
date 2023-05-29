using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public static RoadManager Instance;
    [SerializeField] List<GameObject> roadPrefabs= new List<GameObject>();
    [SerializeField] Transform initialSpawnPosition;
    [SerializeField] Transform secondSpawnPosition;
    public float timeForNext = 10f;
    public bool gameOver;
    public float moveSpeed = 10f;
    [SerializeField] TMP_Text gameOverText;
    [SerializeField] TMP_Text pointsText;
    [SerializeField] TMP_Text maxPointsText;
    int points;
    public bool GameStarted;
    public bool gameFinished;

    Vector3 obstacleSpawnPos;
    Vector3 initialObstacleSpawnPos = new Vector3(0f, 0f,-20f);
    Vector3 secondObstacleSpawnPos = new Vector3(0f, 0f, 55f);
    bool firstSpawned;

    float distanceBetweenObstacles = 16f;
    List<int> possibleX = new List<int>();

    [SerializeField] List<GameObject> obstacles = new List<GameObject>();

    float timeSpeed = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        possibleX.Add(0);
        possibleX.Add(3);
        possibleX.Add(-3);
    }

    //se puede hacer que se pueda reiniciar

    private void Start()
    {
        StartCoroutine(SpawnRoad());
        StartCoroutine(CountPoints());
    }

    IEnumerator CountPoints()
    {
        while (!gameOver)
        {
            yield return new WaitForSeconds(0.1f);
            points++;
            pointsText.text = points.ToString();
        }
    }

    IEnumerator SpawnRoad()
    {
        GameObject newPlatform = Instantiate(roadPrefabs[0], initialSpawnPosition.position, Quaternion.identity); //first road
        int amount = 6;
        if (!firstSpawned)
        {
            amount = 5;
        }
        for (int i = 0; i < amount; i++)
        {
            int randomIndex = Random.Range(0, 3);
            int chosenX = possibleX[randomIndex];
            int randomInd = Random.Range(0, 3);

            if (!firstSpawned)
            {
                firstSpawned = true;
                obstacleSpawnPos = initialObstacleSpawnPos;
            }

            Vector3 newPos = obstacleSpawnPos;

            newPos.x = chosenX;
            if (randomInd == 0)
            {
                newPos.y = 2f;
            }
            else if (randomInd == 1)
            {
                newPos.y = -1f;
            }
            else if (randomInd == 2)
            {
                newPos.y = 1f;
            }

            
            GameObject newObstacle = Instantiate(obstacles[randomInd], newPos, Quaternion.identity);
            newObstacle.transform.parent = newPlatform.transform;
            

            obstacleSpawnPos.z += distanceBetweenObstacles;
        }
        obstacleSpawnPos = secondObstacleSpawnPos;

        while (!gameOver)//following ones        
        {
            Time.timeScale += 0.2f;
            amount = 6;
            int randomInt = Random.Range(0, roadPrefabs.Count);
            int spawningTwo = Random.Range(0, 2);
            newPlatform = Instantiate(roadPrefabs[randomInt], secondSpawnPosition.position, Quaternion.identity);

            for (int i = 0; i < amount; i++)
            {
                int randomIndex = Random.Range(0, 3);
                int chosenX = possibleX[randomIndex];
                int randomInd = Random.Range(0, 3);
                Vector3 newPos = obstacleSpawnPos;
                newPos.x = chosenX;
                if (randomInd == 0)
                {
                    newPos.y = 2f;
                }
                else if (randomInd == 1)
                {
                    newPos.y = -1f;
                }
                else if (randomInd == 2)
                {
                    newPos.y = 1f;
                }
                GameObject newObstacle = Instantiate(obstacles[randomInd], newPos, Quaternion.identity);
                newObstacle.transform.parent = newPlatform.transform;
                if (spawningTwo == 0) //spawning 2 obstacles
                {
                    if (randomIndex == 1 || randomIndex == 2)
                    {
                        int random0orNot = Random.Range(0, 2);
                        if (random0orNot == 0)
                        {
                            newPos.x = -chosenX;
                        }
                        else
                        {
                            newPos.x = 0;
                        }

                        GameObject extraObstacle = Instantiate(obstacles[randomInd], newPos, Quaternion.identity);
                        extraObstacle.transform.parent = newPlatform.transform;
                    }
                }

                obstacleSpawnPos.z += distanceBetweenObstacles;
            }

            obstacleSpawnPos = secondObstacleSpawnPos;

            yield return new WaitForSeconds(timeForNext);
        }

        
    }

    public void GameOver()
    {
        if (points > PlayerPrefs.GetInt("MaxScore"))
        {
            PlayerPrefs.SetInt("MaxScore", points);
        }
        maxPointsText.text = "Max score: " + PlayerPrefs.GetInt("MaxScore").ToString();
        gameOverText.text = "Score: " + points;
        gameOverText.gameObject.SetActive(true);
        maxPointsText.gameObject.SetActive(true);
        GameManager.instance.GameEnded();
    }
}
