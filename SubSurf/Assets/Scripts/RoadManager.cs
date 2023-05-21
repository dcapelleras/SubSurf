using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

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
        Instantiate(roadPrefabs[0], initialSpawnPosition.position, Quaternion.identity); //first road
        while (!gameOver)//following ones
        {
            int randomInt = Random.Range(0, roadPrefabs.Count);
            Instantiate(roadPrefabs[randomInt], secondSpawnPosition.position, Quaternion.identity);
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
        //Time.timeScale = 0f;
    }
}
