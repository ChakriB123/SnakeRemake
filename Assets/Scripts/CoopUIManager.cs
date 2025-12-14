using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class CoopUIManager : MonoBehaviour
{
    public Button menuButton;
    private void Start()
    {
        menuButton.onClick.AddListener(backToMenu);
    }

    public void backToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
 
}
