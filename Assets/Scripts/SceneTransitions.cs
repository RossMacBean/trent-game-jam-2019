using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitions : MonoBehaviour
{
    public Button startButton, creditsButton;
    public Animator transitionAnim;

    void Start()
    {
        startButton.onClick.AddListener(StartOnClick);
        creditsButton.onClick.AddListener(CreditsOnClick);
    }

    void Update()
    {

    }

    void StartOnClick()
    {
        StartCoroutine(LoadScene());
    }

    void CreditsOnClick()
    {

    }

    IEnumerator LoadScene()
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Game");
    }
}
