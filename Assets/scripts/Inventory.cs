using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class inventoryItem 
{

    public inventoryItem(TileData data, int count)
    {
        itemData = data;
        numberOfTheItem = count;

    }
    [SerializeField]
    public TileData itemData;
    [SerializeField]

    public int numberOfTheItem;



}


[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory", order = 0)]
public class Inventory : ScriptableObject
{
    [SerializeField] List<inventoryItem> items = new List<inventoryItem>();
    public Inventory()
    {

    }

    public   Inventory(List<inventoryItem> _items) 
    {   
        items = _items;

    }





    public void deleteItem(TileData data)
    {
        int foundIndex = items.FindIndex((a) => a.itemData == data);
        if (foundIndex != -1)
        {
            if (items[foundIndex].numberOfTheItem > 1)
            {
                items[foundIndex].numberOfTheItem--;
            }
            else
            {

                items.RemoveAt(foundIndex);
            }
        }
        else
        {
            Debug.LogError("item not " + data + " in " + this);
        }

    }



    public void addItem(TileData data)
    {
        int foundIndex = items.FindIndex((a) => a.itemData == data);
        if (foundIndex == -1)
        {
            items.Add(new inventoryItem(data, 1));
        }
        else
        {
            items[foundIndex].numberOfTheItem++;
        }
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
