using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialMenuScript : MonoBehaviour
{
    public void GotoTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void QuitProgram()
    {
        Application.Quit();
    }
}
