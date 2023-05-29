using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject scorePanel;

    [SerializeField] TMP_Text scoreText;


    public void GoToScore()
    {
        scoreText.text = PlayerPrefs.GetInt("MaxScore").ToString();
        mainPanel.SetActive(false);
        scorePanel.SetActive(true);
    }

    public void GoToMain()
    {
        mainPanel.SetActive(true);
        scorePanel.SetActive(false);
    }

    public void StartGame()
    {
        //play animation and then start level, or just start level
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
