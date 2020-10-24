using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public  GameObject GameOverCanvas,GameStartCanvas,TipsCanvas;
    public GameObject[] tips;
    private void Start()
    {
        
        GameStartCanvas.SetActive(true);
      
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
 
        GameStartCanvas.SetActive(false);
        Time.timeScale = 1;
    }
    public void GetNextTips()
    {
        for (int i = 0; i<tips.Length; i++)
        {
            if (tips[i].activeSelf)
            {
                tips[i].SetActive(false);
                if (i + 1 < tips.Length)
                {
                    tips[i + 1].SetActive(true);
                }
                else
                {
                    tips[0].SetActive(true);
                }
                break;
            }
           
        }
    }
    public void SkipTips()
    {
        TipsCanvas.SetActive(false);
    }
}
