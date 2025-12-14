using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{

    public Button solobutton;
    public Button Coopbutton;
    public Button controlsButton;
    public Button exitbutton;
    public Button backbutton;
    public GameObject panelControls;
    private void Awake()
    {
        solobutton.onClick.AddListener(PlaySolo);
        Coopbutton.onClick.AddListener(PlaycoOp);
        controlsButton.onClick.AddListener(OpenControls);
        backbutton.onClick.AddListener(BacktoMenu);
        exitbutton.onClick.AddListener(QuitGame);
    }

    private void PlaySolo()
    {
        Audiomanager.Instance.play(SoundsEnum.ButtonClick);
        SceneManager.LoadScene("Solo");
    }
    private void BacktoMenu()
    {
        Audiomanager.Instance.play(SoundsEnum.ButtonClick);
        panelControls.SetActive(false);
    }
    private void OpenControls()
    {
        Audiomanager.Instance.play(SoundsEnum.ButtonClick);
        panelControls.SetActive(true);
    }
    private void PlaycoOp()
    {
        Audiomanager.Instance.play(SoundsEnum.ButtonClick);
        SceneManager.LoadScene("Co-op");
    }
    public void QuitGame()
    {
        Audiomanager.Instance.play(SoundsEnum.ButtonClick);
        Application.Quit();
    }
}
