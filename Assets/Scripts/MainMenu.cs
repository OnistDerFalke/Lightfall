using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLevel1ButtonPressed()
    {
        StartCoroutine(StartGame("Scenes/Level1"));
    }
    
    public void OnLevel2ButtonPressed()
        {
            StartCoroutine(StartGame("Scenes/Level2"));
        }

    public void OnExitButtonPressed()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    IEnumerator StartGame(string levelName)
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(levelName);
    }
}
