using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartExit : MonoBehaviour
{
    public GameObject[] buttons;
    public GameObject backButton;

    public void startGame()
    {
        SceneManager.LoadScene(1 + SceneManager.GetActiveScene().buildIndex);
    }

    public void endGame()
    {
        Application.Quit();
    }

    public void showInstructions()
    {
        foreach(GameObject obj in buttons)
        {
            obj.SetActive(false);
        }
        backButton.SetActive(true);
    }

    public void goBack()
    {
        backButton.SetActive(false);
        foreach (GameObject obj in buttons)
        {
            obj.SetActive(true);
        }
    }


}
