using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MobileUtilsScript : MonoBehaviour
{

    
    private float frequency = 1.0f;
    private string fps;

    public Text Fpstxt;

    void Start()
    {
        StartCoroutine(FPS());
    }

    private IEnumerator FPS()
    {
        for (; ; )
        {
            // Capture frame-per-second
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);
            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;

            // Display it

            fps = string.Format("FPS: {0}", Mathf.RoundToInt(frameCount / timeSpan));
        }
    }


    void OnGUI()
    {
        Fpstxt.text = fps;
    }
}