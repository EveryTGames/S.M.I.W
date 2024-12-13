using System;
using System.Collections;
using System.IO;
using UnityEngine;
//put this code in the map object that the map will be child of


[Serializable]
public struct co
{
    public string name;
    public Color32 Color32;
    public GameObject prefab;
}



public class PixelImageLoader : MonoBehaviour
{



    public inventory inventory;
    public int cellSize = 50;


    public static bool Done = false;
     co[] objects;
    Color32[] map;
    Texture2D texture;
    int width, height;
    public GameObject slotPrefab;
    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Done = false;
        texture = inventory.texture;
            map = texture.GetPixels32();
            width = texture.width;
            height = texture.height;
        rectTransform.sizeDelta = new Vector2(width * cellSize, height * cellSize);
            StartCoroutine(loadmap());
        
    }

    // Update is called once per frame
    void Update()
    {

    }








    IEnumerator loadmap()
    {
        deletMap();
       
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color32 co = map[(y * width) + x];
                
                if(co.r == 255)
                {
                    GameObject duf = Instantiate(slotPrefab, transform);
                    duf.transform.SetAsFirstSibling();
                    //duf.GetComponent<RectTransform>().anchorMin = Vector2.zero;
                    //duf.GetComponent<RectTransform>().anchorMax = Vector2.zero;
                    //yield return null;
                    duf.GetComponent<RectTransform>().localPosition += new Vector3 { x = x * cellSize, y = y * cellSize };
                    //Debug.Log(duf.transform.localPosition);
                        //new Vector2 { x = x * cellSize, y = y * cellSize } /*- new Vector2 { x = width /2,y = height/2}*/;

                   // Debug.Log(x+"   "+ y);
                }
                else
                {

                }

                //Debug.LogWarning($"something went wrong in pixel {x},{y}, with color {co.ToString()}");
                // yield return null;

            }

        }
        yield return new WaitForSeconds(0.001f);

        Done = true;
        GetComponent<inventoryHolder>().load();

    }
   // public static bool stillDeveloping = false;
    void deletMap()
    {
        while (transform.childCount > 0)
        {
            Transform x = transform.GetChild(0);
            x.SetParent(null);
            Destroy(x.gameObject);

        }
        GameObject itemss = new GameObject("items");
        itemss.transform.SetParent(transform);
        itemss.transform.localPosition = Vector3.zero;
        itemss.transform.localScale = new Vector3(1,1,1);



    }

    /// <summary>
    /// before calling it, update the values of waveHandler.waves_static to the desired values -- this is old
    /// </summary>


}

