using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MobileUtilsScript : MonoBehaviour
{

    
    private float frequency = 1.0f;
    private string fps;

    public Text Fpstxt;
    private int lastFrameCount;
    private float lastTime;
    private float timeSpan;
    private int frameCount;
    void Start()
    {
        StartCoroutine(FPS());
    }

    private IEnumerator FPS()
    {
        for (; ; )
        {
            // Capture frame-per-second
            lastFrameCount = Time.frameCount;
            lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);
            timeSpan = Time.realtimeSinceStartup - lastTime;
            frameCount = Time.frameCount - lastFrameCount;

            // Display it

            fps = string.Format("FPS: {0}", Mathf.RoundToInt(frameCount / timeSpan));
        }
    }


    void OnGUI()
    {
        Fpstxt.text = fps;
    }
}