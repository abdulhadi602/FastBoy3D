using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    private  GameObject GameOverCanvas,GameStartCanvas,TipsCanvas;
    private GameObject[] tips;

    private static string TipsKey = "TipsRead";
    private void Awake()
    {
        GameOverCanvas = GameObject.FindGameObjectWithTag("GameOverCanvas");
        GameStartCanvas = GameObject.FindGameObjectWithTag("GameStartCanvas");
        TipsCanvas = GameObject.FindGameObjectWithTag("TipsCanvas");
        tips = new GameObject[4];
        tips[0] = GameObject.FindGameObjectWithTag("tip1");
        tips[1] = GameObject.FindGameObjectWithTag("tip2");
        tips[2] = GameObject.FindGameObjectWithTag("tip3");
        tips[3] = GameObject.FindGameObjectWithTag("tip4");
    }
    private void Start()
    {
     
        GameOverCanvas.SetActive(false);
        TipsCanvas.SetActive(false);


        GameStartCanvas.SetActive(true);
      
        Time.timeScale = 0;
        if (!PlayerPrefs.GetString(TipsKey).Equals("true"))
        {
            TipsCanvas.SetActive(true);
            
        }
        else
        {
            TipsCanvas.SetActive(false);
        }
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
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
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
    public void ShowTips()
    {
        TipsCanvas.SetActive(true);
        Time.timeScale = 0;
    }
    public void SkipTips()
    {
        if (!PlayerPrefs.GetString(TipsKey).Equals("true"))
        {
            PlayerPrefs.SetString(TipsKey, "true");
            PlayerPrefs.Save();
        }
        if (!GameStartCanvas.activeSelf)
        {
            Time.timeScale = 1;
        }
        TipsCanvas.SetActive(false);
    }
}
