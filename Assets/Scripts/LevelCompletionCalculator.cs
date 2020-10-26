using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletionCalculator : MonoBehaviour
{
    public static bool LevelCompleted;
    private Text LevelCompleteiontext;
    
    private float Totaldist;
    
    private Transform Player;
    private Transform NearEnd;
    private float currentPercentage;
    private float currentdist;
    // Start is scalled before the first frame update
    private void Awake()
    {
        LevelCompleteiontext = GameObject.FindGameObjectWithTag("LevelCompletionText").GetComponent<Text>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        NearEnd = GameObject.FindGameObjectWithTag("NearEnd").transform;
    }
    void Start()
    {
       

        Totaldist = Vector3.Distance(NearEnd.position, Player.position);
        LevelCompleted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelCompleted)
        {
            currentdist = Vector3.Distance(NearEnd.position, Player.position);
            currentPercentage = ((Totaldist- currentdist) / Totaldist) * 100;
            
            LevelCompleteiontext.text = (int)currentPercentage + "%";
        }
        else
        {
         
            LevelCompleteiontext.text = "100%";
        }
    }
}
