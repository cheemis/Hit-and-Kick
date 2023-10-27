using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    //pages variables
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject HowToPlayScreen;
    [SerializeField]
    private GameObject CreditScreen;

    //animation variables
    [SerializeField]
    private Animator anim;

    //managing variables
    private bool alreadyStarted = false;

    private void Start()
    {
        GoBack();
    }

    public void StartGame()
    {
        alreadyStarted = true;
        if (anim != null) { anim.SetTrigger("fadeOut"); }
        StartCoroutine(LoadGame());
    }

    public void OpenHowToPlay()
    {
        mainMenu.SetActive(false);
        HowToPlayScreen.SetActive(true);
    }

    public void OpenCredits()
    {
        mainMenu.SetActive(false);
        CreditScreen.SetActive(true);
    }

    public void GoBack()
    {
        mainMenu.SetActive(true);
        HowToPlayScreen.SetActive(false);
        CreditScreen.SetActive(false);
    }

    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(1);
    }

}
