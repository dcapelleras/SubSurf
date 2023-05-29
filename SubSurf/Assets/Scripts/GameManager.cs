using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject playPanel;

    bool paused;

    private void Start()
    {
        Time.timeScale = 0.3f;
        StartCoroutine(StartTime());
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
                paused = false;
                ResumeGame();
            }
            else
            {
                paused = true;
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        Time.timeScale= 0;
        pausePanel.SetActive(true);
        playPanel.SetActive(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        playPanel.SetActive(true);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
