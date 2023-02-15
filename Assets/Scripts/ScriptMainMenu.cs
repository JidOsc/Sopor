using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScriptMainMenu : MonoBehaviour
{
    //
    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void InstructionsGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);

    }

    public void Backgame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);

    }

    public void QuitGame ()
    {
        Application.Quit();
    }
        
    
}
