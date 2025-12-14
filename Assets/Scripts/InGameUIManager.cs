using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics.CodeAnalysis;

public class InGameUIManager : MonoBehaviour
{
    public Button pauseButton;
    public Button unPauseButton;
    public Button menuButton;
    public GameObject pauseMenuPanel;
    private void Start()
    {
        pauseButton.onClick.AddListener(PauseMenu);
        unPauseButton.onClick.AddListener(unPauseMenu);
        menuButton.onClick.AddListener(backToMenu);
    }

    private void PauseMenu()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    private void unPauseMenu()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void backToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

}

