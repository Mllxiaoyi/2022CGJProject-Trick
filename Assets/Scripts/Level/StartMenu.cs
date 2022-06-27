using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("CG");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
