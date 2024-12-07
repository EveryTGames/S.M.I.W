using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileData", menuName = "TileData", order = 0)]
public class TileData : ScriptableObject
{
    [Tooltip("the tiles that will carry this data")]
    public TileBase[] tiles; // the tiles that will carry this data
    //---------------------------

    [Tooltip("the name of the object representing the tile")]
    public string Name;
    //-------------------------------------

    [Tooltip("the the time to break this tile")]
    public float TimeToBreak;

    public override string ToString()
    {
        return base.ToString();
    }
}