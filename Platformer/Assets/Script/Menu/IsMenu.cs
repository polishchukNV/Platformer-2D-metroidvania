using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IsMenu : MonoBehaviour
{
    private bool isMenu = true;
    private bool isOptions;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject options;

    private void MenuOpen()
    {
        if (isMenu)
        {
            menu.SetActive(true);
            Time.timeScale = 0;
            isMenu = false;
        }
        else
        {
            menu.SetActive(false);
            Time.timeScale = 1;
            isMenu = true;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && !isOptions)
            MenuOpen();
        else if(Input.GetButtonDown("Cancel") && isOptions)
        {
            options.SetActive(false);
            menuButton.SetActive(true);
        }
    }

    public void Exit()
    {
        LevelLoader.SwitchToScene("Menu");
    }

    public void Continium()
    {
        menu.SetActive(false);
        Time.timeScale = 1;
        isMenu = true;
        isOptions = false;
    }

    public void Options()
    {
        options.SetActive(true);
        menuButton.SetActive(false);
        isOptions = true;
    }
}
