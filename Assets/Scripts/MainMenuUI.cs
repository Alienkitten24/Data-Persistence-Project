using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartNew()
    {
        SceneManager.LoadScene(1);

    }

    public void Exit()
    {

#if UNITY_EDITOR
    EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void GoToSettingsScreen()
    {
        SceneManager.LoadScene(2);
    }

    public void GoToHighScoresScreen()
    {
        SceneManager.LoadScene(3);
    }

    public void GoToMainScreen()
    {
        SceneManager.LoadScene(0);
    }

}
