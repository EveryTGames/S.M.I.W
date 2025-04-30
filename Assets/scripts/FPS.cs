using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//it shows the FPS on teh screen (must be in build mode for stable results)
public class FPS : MonoBehaviour
{
   
   public Text fps_;
    private void Start()
    {
        
        StartCoroutine(FramesPerSecond());
    }
 
    private IEnumerator FramesPerSecond()
    {
        while (true)
        {
            int fps = (int) (1f / Time.deltaTime);
            DisplayFPS(fps);
 
            yield return new WaitForSeconds(0.2f);
        }
    }
 
    private void DisplayFPS(float fps)
    {
        fps_.text = "" + fps;
      // Debug.Log(fps);
    }
}