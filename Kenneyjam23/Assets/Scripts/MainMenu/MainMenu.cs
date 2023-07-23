using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _defaultMenu = null;
    [SerializeField] private GameObject _creditsMenu = null;

    [SerializeField] private float _audioFadeSpeed = 2.0f;

    private AudioSource _source = null;

    private void Start()
    {
        _source = GetComponent<AudioSource>();

        OpenDefault();
    }

    private void Update()
    {
        if (_source.volume > 1.0f) return;

        _source.volume += _audioFadeSpeed * Time.deltaTime;
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