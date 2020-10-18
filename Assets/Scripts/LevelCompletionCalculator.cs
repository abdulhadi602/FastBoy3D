using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletionCalculator : MonoBehaviour
{
    public static bool LevelCompleted;
    public Text LevelCompleteiontext;
    private float percentageOfLevelCompleted;
    private float Totaldist;
    public Slider LeveLCompletionBar;
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
            LeveLCompletionBar.value = currentPercentage;
            LevelCompleteiontext.text = (int)currentPercentage + "%";
        }
        else
        {
            LeveLCompletionBar.value += 1;
            LevelCompleteiontext.text = "100%";
        }
    }
}
