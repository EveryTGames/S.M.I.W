using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static events;



public class UIShowDataManager : MonoBehaviour
{
    [SerializeField] GameObject ItemDataPrefabe;
    // Start is called before the first frame update
    void Start()
    {

        onItemDataRetrived += OnItemDataRetrived;
    }

    // Update is called once per frame
    void Update()
    {

    }
    Dictionary<ItemData, Animator> oldData = new Dictionary<ItemData, Animator>();//old data

    void OnItemDataRetrived(List<(ItemData,TileBase)> newData,Vector3Int cellpos) // Takes the new data
    {
        //Debug.Log(newData.Count);


        //The hashset idea is from chatgpt :)
        // Convert the new data to a HashSet for quick lookup
       HashSet<ItemData> newDataSet = new HashSet<ItemData>(newData.Select(tuple => tuple.Item1));
       
        

        // Remove items not in new data and trigger fade
        foreach (ItemData item in oldData.Keys.ToList()) // ToList() to avoid modifying the collection during iteration
        {
            if (!newDataSet.Contains(item))
            {
                oldData[item].SetTrigger("fade"); // Trigger fade animation
                oldData.Remove(item); // Remove from dictionary
            }
        }

        // Add new items or keep existing ones
        foreach (ItemData item in newDataSet)
        {
            if (!oldData.ContainsKey(item))
            {
                Intializer initializer = Instantiate(ItemDataPrefabe, gameObject.transform).GetComponent<Intializer>();
                initializer.assign(item);


                oldData.Add(item, initializer.transform.GetComponentInChildren<Animator>());
            }
        }
    }

}
