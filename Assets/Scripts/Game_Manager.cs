using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager Instance;

    public int playerScore = 0;
    public int aiScore = 0;
    public int winningScore = 5;

    [Header("UI")]
    public GameObject mainMenuPanel;
    public GameObject inGamePanel;
    public GameObject endGamePanel;

    public TMP_Text playerScoreText;
    public TMP_Text aiScoreText;
    public TMP_Text resultText;

    public Ball_Controller ballController;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 0f; // pause at startup
        ShowMainMenu();
    }

    public void StartGame()
    {
        playerScore = 0;
        aiScore = 0;
        UpdateScoreUI();

        mainMenuPanel.SetActive(false);
        inGamePanel.SetActive(true);
        endGamePanel.SetActive(false);

        Time.timeScale = 1f; // unpause
        ballController.ResetBall();
    }

    public void RestartGame()
    {
        Audio_Manager.Instance.PlayButton();
        StartCoroutine(RestartAfterDelay());
    }

    IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSecondsRealtime(0.2f); // unaffected by timescale
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ScorePoint(bool playerScored)
    {
        if (playerScored)
            playerScore++;
        else
            aiScore++;

        UpdateScoreUI();

        if (playerScore >= winningScore || aiScore >= winningScore)
        {
            EndGame(playerScore >= winningScore);
        }
        else
        {
            Audio_Manager.Instance.PlayGoal();
            ballController.ResetBall();
        }
    }

    void UpdateScoreUI()
    {
        playerScoreText.text = playerScore.ToString();
        aiScoreText.text = aiScore.ToString();
    }

    void EndGame(bool playerWon)
    {
        Audio_Manager.Instance.PlaySFX(playerWon ? Audio_Manager.Instance.winClip : Audio_Manager.Instance.loseClip);
        Time.timeScale = 0f;
        inGamePanel.SetActive(false);
        endGamePanel.SetActive(true);
        resultText.text = playerWon ? "You Win!" : "You Lose!";
    }

    void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        inGamePanel.SetActive(false);
        endGamePanel.SetActive(false);
    }
}
