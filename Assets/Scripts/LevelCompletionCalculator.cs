using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletionCalculator : MonoBehaviour
{
    public static bool LevelCompleted;
    public Text LevelCompleteiontext;
    
    private float Totaldist;
    
    public Transform Player;
    public Transform End;
    private float currentPercentage;
    private float currentdist;
    // Start is called before the first frame update
    void Start()
    {
        Totaldist = Vector3.Distance(End.position, Player.position);
        LevelCompleted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelCompleted)
        {
            currentdist = Vector3.Distance(End.position, Player.position);
            currentPercentage = ((Totaldist- currentdist) / Totaldist) * 100;
            
            LevelCompleteiontext.text = (int)currentPercentage + "%";
        }
        else
        {
         
            LevelCompleteiontext.text = "100%";
        }
    }
}
