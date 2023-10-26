using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    //pages variables
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject CreditScreen;

    //animation variables
    private Animator anim;

    private void Start()
    {
        GoBack();
    }

    public void StartGame()
    {

    }

    public void OpenCredits()
    {
        mainMenu.SetActive(false);
        CreditScreen.SetActive(true);
    }

    public void GoBack()
    {
        mainMenu.SetActive(true);
        CreditScreen.SetActive(false);
    }

}
