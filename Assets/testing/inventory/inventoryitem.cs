
using UnityEngine;
[System.Serializable]
public class inventoryitem 
{
    public ItemData itemData;
    public int itemindex;
      public inventoryitem(ItemData data)
    {
        itemData = data;
    }
}
