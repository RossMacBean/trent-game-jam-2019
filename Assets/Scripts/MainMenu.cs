using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Animator transitionAnim;
    private Button startButton, creditsButton, creditsBackButton;
    private RectTransform mainMenuPanel, creditsPanel;

    void Start()
    {
        var buttons = GetComponentsInChildren<Button>();
        startButton = buttons.First(b => b.name == "StartButton");
        creditsButton = buttons.First(b => b.name == "CreditsButton");
        creditsBackButton = buttons.First(b => b.name == "BackButton");

        var rects = GetComponentsInChildren<RectTransform>();
        mainMenuPanel = rects.First(r => r.name == "MainMenuPanel");
        creditsPanel = rects.First(r => r.name == "CreditsPanel");
        creditsPanel.gameObject.SetActive(false);

        startButton.onClick.AddListener(StartOnClick);
        creditsButton.onClick.AddListener(CreditsOnClick);
        creditsBackButton.onClick.AddListener(CreditsBackOnClick);
    }

    void StartOnClick()
    {
        StartCoroutine(LoadScene());
    }

    void CreditsOnClick()
    {
        creditsPanel.gameObject.SetActive(true);
    }

    void CreditsBackOnClick()
    {
        creditsPanel.gameObject.SetActive(false);
    }

    IEnumerator LoadScene()
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Game");
    }
}
