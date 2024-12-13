using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class widthFitter : MonoBehaviour
{
    public Transform Content;
    public float tolarence = 55;
    public static float mostLeft;

    // Start is called before the first frame update
    void Start()
    {
        PixelImageLoader[] inventories = Content.GetComponentsInChildren<PixelImageLoader>();
        float maxWidth = 0;
        foreach (PixelImageLoader inventory in inventories)
        {
            if (inventory.inventory.texture.width > maxWidth)
            {
                maxWidth = inventory.inventory.texture.width;

            }
        }
        Transform canvas = GameObject.Find("Canvas").transform;
        // change the 50
        maxWidth = maxWidth * 50 + tolarence;
        //u stopped here
        GetComponent<RectTransform>().sizeDelta = new Vector2(maxWidth, 321.8f);
        float newLocalPosition =  800 - (GetComponent<RectTransform>().sizeDelta.x  / 2); //the local position from the start of the left edge of the screen
        Debug.Log(newLocalPosition);
        GetComponent<RectTransform>().position = new Vector3 { x = newLocalPosition * Screen.width / 800f , y = 0.5f * Screen.height,z=0f };
        float halfWidth = GetComponent<RectTransform>().sizeDelta.x / 2;
      
        mostLeft = (newLocalPosition - halfWidth ) * Screen.width / 800f;

        Debug.Log(GetComponent<RectTransform>().position);
        Debug.Log(GetComponent<RectTransform>().sizeDelta);
        Debug.Log(canvas.localScale);
        Debug.Log(Screen.width);
    }
}
