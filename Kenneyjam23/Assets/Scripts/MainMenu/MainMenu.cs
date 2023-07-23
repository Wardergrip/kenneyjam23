using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _defaultMenu = null;
    [SerializeField] private GameObject _creditsMenu = null;

    private void Start()
    {
        OpenDefault();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenCredits()
    {
        _defaultMenu.SetActive(false);
        _creditsMenu.SetActive(true);
    }

    public void OpenDefault()
    {
        _defaultMenu.SetActive(true);
        _creditsMenu.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}