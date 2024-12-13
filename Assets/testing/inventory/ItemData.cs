using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "itemdata", menuName = "inventory/newitem")]

[Serializable]
public class ItemData : ScriptableObject
{
    //no prefab for each item here as it is in the inventoryHolder, i will just save the id which is the index in the allItems array
  public string itemname;
  public int ID;
  public Vector2 dimention;
 // public Sprite icon;
  public string type;
  //public GameObject itempreferb;
  [TextArea]
  public string description;


    public GameObject UIprefabe;
    public GameObject worldPrefabe;// for when switching between the inventory and the world, you can maje it so taht when holding the item inside the borders of the inventory, it destroyes the instantiated world prefab and puts the ui prefabe in its place, and vise versa
}
