using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject playPanel;
    [SerializeField] GameObject resumeButton;
    [SerializeField] GameObject restartButton;

    bool paused;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Time.timeScale = 0.3f;
        StartCoroutine(StartTime());
    }

    public void GameEnded()
    {
        resumeButton.SetActive(false);
        restartButton.SetActive(true);
    }

    IEnumerator StartTime()
    {
        yield return new WaitForSeconds(0.9f);
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        paused = true;
        Time.timeScale= 0;
        pausePanel.SetActive(true);
        playPanel.SetActive(false);
    }

    public void ResumeGame()
    {
        paused = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        playPanel.SetActive(true);
    }

    public void RestartGame()
    {
        paused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
