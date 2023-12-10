using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tombol : MonoBehaviour
{
    public GameObject DifficultySet;
    public GameObject Credit;
    public GameObject MainMenu;
    public GameObject PausedObject;
    public void PlayGame()
    {
        //   DifficultySet.SetActive(!DifficultySet.activeSelf);
        //    MainMenu.SetActive(!MainMenu.activeSelf);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void showCredit()
    {
        Credit.SetActive(!Credit.activeSelf);
        MainMenu.SetActive(!MainMenu.activeSelf);
    }
    public void Back()
    {
        MainMenu.SetActive(!MainMenu.activeSelf);
        Credit.SetActive(false);
        DifficultySet.SetActive(false);
    }
    public void Easy()
    {
        SceneManager.LoadScene("Game");
    }
    public void Medium()
    {
        SceneManager.LoadScene("Game");
    }
    public void Hard()
    {
        SceneManager.LoadScene("Game");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void Paused()
    {
        Time.timeScale = 0;
        PausedObject.SetActive(true);
    }
    public void resume()
    {
        PausedObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
    public void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Paused();
            }
        }
    }
}
