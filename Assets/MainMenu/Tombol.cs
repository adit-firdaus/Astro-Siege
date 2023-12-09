using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tombol : MonoBehaviour
{
    public GameObject DifficultySet;
    public GameObject Credit;
    public GameObject MainMenu;

    public void PlayGame()
    {
        DifficultySet.SetActive(!DifficultySet.activeSelf);
        MainMenu.SetActive(!MainMenu.activeSelf);
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
}
