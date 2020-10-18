using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public  GameObject GameOverCanvas,GameStartCanvas;
    public GameObject LevelSlider;
    private void Start()
    {
        GameStartCanvas.SetActive(true);
        LevelSlider.SetActive(false);
        Time.timeScale = 0;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && GameOverCanvas.activeSelf==true)
        {
            Restart();
        }
    }
    public void GameOver()
    {
        Time.timeScale = 0;
        GameOverCanvas.SetActive(true);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void StartGame()
    {
        LevelSlider.SetActive(true);
        GameStartCanvas.SetActive(false);
        Time.timeScale = 1;
    }
}
