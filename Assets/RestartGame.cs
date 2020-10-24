using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RestartGame : MonoBehaviour
{
    public void Restartgame()
    {
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();  
    }
}
