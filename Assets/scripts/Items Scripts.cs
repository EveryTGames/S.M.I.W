using System;
using UnityEngine;
using UnityEngine.Tilemaps;


//this is used for custom scripts when clicking on an itme  that is in teh tiles mode 
public class ItemsScripts : MonoBehaviour
{
    public static ScriptNames scriptNames;
    public static Action<(ItemData, TileBase), Vector3Int> Script;
    public enum ScriptNames
    {
        nothing,
        Chest
    }
    // Start is called before the first frame update
    void Start()
    {
        events.onTileUse += OnTileUse;
    }
    void OnTileUse((ItemData, TileBase) data, Vector3Int cellPos)
    {
        Debug.Log("finally??");
        switch (data.Item1.script)
        {
            case ScriptNames.Chest:
                Script += ChestScript;
                break;
            default:
                break;
        }

        Script.Invoke(data, cellPos);

    }

    public void ChestScript((ItemData, TileBase) data, Vector3Int cellPos)
    {
        Debug.Log("chest script");

        new TileAnimator("Chest open", data, cellPos, true, 0, 0.4f, (data,cellPos) =>
        {
            //open the ui for the opened chest
            GenericInventories.AddToMap(cellPos);
            GameObject _inventoryUI = GenericInventories.loadInventory(InventoryTypes.normalChest, "inv_" + GenericInventories.CellPosMap(cellPos));
            _inventoryUI.SetActive(true);
            //TODO add somehting so that if it was a tile then it will be auto closed when the player goes away
        });

    }


}
