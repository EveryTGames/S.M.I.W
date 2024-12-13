using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class allTheItems : MonoBehaviour
{
    public static ItemData[] allitems;
    public ItemData[] allItems_non_static;
    // Start is called before the first frame update
    void Start()
    {
        allitems = allItems_non_static;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
