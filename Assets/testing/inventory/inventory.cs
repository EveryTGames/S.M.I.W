using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "new inventory", menuName = "inventory/new inventory")]
[System.Serializable]
public class inventory : ScriptableObject
{

    public static Dictionary<ItemData, float> AllPowerUpsItems;
    public Texture2D texture;

    [Tooltip("well the items use be applied when put in this inventory?")]
    public bool alllowItemUse;


}
