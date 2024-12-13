using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData", order = 0)]
[Serializable]
public class ItemData : ScriptableObject
{

    public int ID;

    public GameObject UIprefabe;
    public GameObject worldPrefabe;// for when switching between the inventory and the world, you can maje it so taht when holding the item inside the borders of the inventory, it destroyes the instantiated world prefab and puts the ui prefabe in its place, and vise versa

    public Vector2 dimention;

    [Tooltip("the tiles that will carry this data")]
    public TileBase[] tiles; // the tiles that will carry this data
    //---------------------------

    [Tooltip("the name of the object representing the tile")]
    public string Name;
    //-------------------------------------

    [Tooltip("the description of the object representing the tile")]
    [TextArea]
    public string description;
    //-------------------------------------

    [Tooltip("the the time to break this tile")]
    public float TimeToBreak;

    [Tooltip("is this tile breakable?")]

    public bool breakable;



    public override string ToString()
    {
        return base.ToString();
    }
}