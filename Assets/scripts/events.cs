using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class events
{
    //start of events for playerControler --------------------------------------------
    public static event Action<bool> onGroundCheck;
    public static void TriggerGroundCheck(bool state)
    {
        onGroundCheck?.Invoke(state);
    }
    //-------------------
    public static event Action<bool> onForwardLegCheck;

    public static void TriggerForwardLegCheck(bool state)
    {
        onForwardLegCheck?.Invoke(state);
    }
    //-------------------
    public static event Action<bool> onForwardHeadCheck;
    public static void TriggerForwardHeadCheck(bool state)
    {
        onForwardHeadCheck?.Invoke(state);
    }
    //-------------------

    //end of events for playerController-----------------------------------------------

    //start of events for uiShowItemData ----------------------------------------------
    public static event Action<List<(TileData, TileBase)>, Vector3Int> onItemDataRetrived;
    public static void TriggerShowItem(List<(TileData, TileBase)> state, Vector3Int cellpos)
    {
        onItemDataRetrived?.Invoke(state, cellpos);
    }
    // end of events for uiShowItemData -----------------------------------------------




    //start of events for tileHover ---------------------------------------------------

    // --these functions are not called eah frame but on each mouse move--
    public static event Action<Vector3Int> onTileHoverStart;
    public static void TriggerTileHoverStart(Vector3Int cellPos)
    {
        onTileHoverStart?.Invoke(cellPos);
    }
    //-----------------
    public static event Action<Vector3Int> onTileHoverStay;
    public static void TriggerTileHoverStay(Vector3Int cellPosistion)
    {
        onTileHoverStay?.Invoke(cellPosistion);
    }
    //-----------------------------


    public static event Action<Vector3Int> onTileMovingStay; // updated every frame which in is the mose moving (fast) (the above updated when it is not moving (slow))
    public static void TriggerTileMovingStay(Vector3Int cellPos)
    {
        onTileMovingStay?.Invoke(cellPos);
    }
    public static event Action<Vector3Int> onTileLeave;
    public static void TriggerTileLeave(Vector3Int cellPos)
    {
        onTileLeave?.Invoke(cellPos);
    }
    // end of events for tileHover

    //start of events for breaking --------------------------------------------------

    public static event Action<(TileData, TileBase), Vector3Int> onTileBreakStart;
    public static void TriggerTileBreakStart((TileData, TileBase) data, Vector3Int cellpos)
    {
        onTileBreakStart?.Invoke(data, cellpos);
    }
    //-----------------------------
    public static event Action<(TileData, TileBase), Vector3Int> onTileBreakEnd;
    public static void TriggerTileBreakEnd((TileData, TileBase) data, Vector3Int cellpos)
    {
        onTileBreakEnd?.Invoke(data, cellpos);
    }


    // events for mouse interactions -------------------------------------------------

    public static event Action onMouseUp;
    public static void TriggerMouseUp()
    {
        onMouseUp?.Invoke();
    }

    // ---------------------------
    public static event Action<Vector3Int> onMouseDown;
    public static void TriggerMouseDown(Vector3Int cellPos)
    {
        onMouseDown?.Invoke(cellPos);
    }
    // end ----------------------------------------------------------------------------

    public static event Action<bool> onInventoryToggle;
    public static void TriggerInventoryToggle(bool triggerTo)
    {
        onInventoryToggle?.Invoke(triggerTo);
    }
    //-------------------
    public static event Action<Transform> onItemDroppedInUI;
    public static void TriggerItemDroppedInUI(Transform itemTransform)
    {
        onItemDroppedInUI?.Invoke(itemTransform);
    }
    public static event Action<Transform> onItemDroppedInWorld;
    public static void TriggerItemDroppedInWorld(Transform itemTransform)
    {
        onItemDroppedInWorld?.Invoke(itemTransform);
    }
    public static event Action<Transform> onItemDraggingInWorld;
    public static void TriggerItemDraggingInWorld(Transform itemTransform)
    {
        onItemDraggingInWorld?.Invoke(itemTransform);
    }
    public static event Action<Transform> onItemDraggingInUI;
    public static void TriggerItemDraggingInUI(Transform itemTransform)
    {
        onItemDraggingInUI?.Invoke(itemTransform);
    }
    public static event Action<Transform> onItemPickedInWorld;
    public static void TriggerItemPickedInWorld(Transform itemTransform)
    {
        onItemPickedInWorld?.Invoke(itemTransform);
    }
    public static event Action<Transform> onItemPickedInUI;
    public static void TriggerItemPickedInUI(Transform itemTransform)
    {
        onItemPickedInUI?.Invoke(itemTransform);
    }
    public static event Action<Transform, Action<Transform>> onItemWorldToUI;
    public static Transform TriggerItemWorldToUI(Transform itemTransform)
    {
        Transform newTransform = null;
        onItemWorldToUI?.Invoke(itemTransform, (updatedTransform) =>
        {
            // You can handle the updated transform after the event is raised
            newTransform = updatedTransform;
        });
        return newTransform;
    }
    public static event Action<Transform,Action<Transform>> onItemUIToWorld;
    public static Transform TriggerItemUIToWorld(Transform itemTransform)
    {
        Transform newTransform = null;

        onItemUIToWorld?.Invoke(itemTransform, (updatedTransform) =>
        {
            // You can handle the updated transform after the event is raised
            newTransform = updatedTransform;
        });
        return newTransform;
    }




    //--------------------------------------------------------------------------------


}