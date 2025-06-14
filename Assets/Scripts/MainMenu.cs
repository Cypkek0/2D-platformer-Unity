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
        StartCoroutine(StartGame("Level 1"));
    }
    public void OnLevel2ButtonPressed()
    {
        StartCoroutine(StartGame("Level 2"));
    }
    private IEnumerator StartGame(string levelName)
    {
        yield return new WaitForSeconds(0.05f);
        SceneManager.LoadScene(levelName);
    }
    public void OnExitToDesktopButtonPressed()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #endif
    Application.Quit();
    }
}
