using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static events;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine.Tilemaps;
using UnityEngine.AI;





[Serializable]
public class SavedPowerUp
{
    public List<ItemEntry> items;  // Store as a list (Unity can't serialize Dictionary)

    public SavedPowerUp(Dictionary<ItemData, float> dictionary)
    {
        items = new List<ItemEntry>();
        foreach (var kvp in dictionary)
        {
            items.Add(new ItemEntry { itemID = kvp.Key.ID, multiplier = kvp.Value });
        }
    }
}

[Serializable]
public class ItemEntry
{
    public int itemID;
    public float multiplier;
}


//-----------------------------------------------------------------------------
//the slot must have a slot tag and the items must have ok tag
public class DragAndDrop : MonoBehaviour
{
    //tehse for the moving betwean inventory process
    //the item holder inside of each inventory must be the last item, so every instantia of a slot needto be sibling index 0
    public Transform theMainContentViewer;
    public Transform itemTransfareHolder; // for putting the item in while transfaring u need tomake it the last item in the inventory viewr, whcih means eevery inventory u instantaite it needs to be sibking index 0
    Transform oldParent;
    float OldRotation;
    Vector2 oldDimention;




    //end here
    public static float step;
    /// the squar width and height

    public float Rstep = 50;
    // Start is called before the first frame update
    void Start()
    {
        if (itemTransfareHolder == null)
        {
            itemTransfareHolder = transform.parent.GetChild(transform.parent.childCount - 1);
        }

        if (raycaster == null)
            {
                raycaster = transform.parent.GetComponent<GraphicRaycaster>();
                Debug.LogWarning("the raycaster have been assigned automaticlly");
            }


        step = Rstep * (GameObject.Find("Canvas").transform.localScale.x);

        onItemWorldToUI += OnItemWorldToUI;
        onItemUIToWorld += OnItemUIToWorld;
        onItemDraggingInUI += OnItemDraggingInUI;
        //Debug.Log(Rstep);
    }
    //u stopped here -------------------------------------------------- u will add something to change to the first rotation state
    // u will need to optimiz this (ram) -------------------------------------------------------------------------------------
    void redoTheParent(Transform item)
    {
        item.SetParent(oldParent, true);
        item.eulerAngles = new Vector3 { z = OldRotation };
        item.GetComponent<items>().ZRotation = (int)OldRotation;
        item.GetComponent<items>().Dimention = oldDimention;

    }
    void OnItemDraggingInUI(Transform itemTransform)
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            itemTransform.GetComponent<items>().Rotate();

        }

    }
    void OnItemUIToWorld(Transform target, Action<Transform> callBack)
    {
        if (!world)
        {
            //TODO add the logic here so that when it turned from or into the world it preserve its nameID if it was a chest, so that every chest be liike constant
            Transform ds = Instantiate(target.GetComponent<items>().data.worldPrefabe).transform;
            ds.gameObject.name = "ss " + Time.time;
            Debug.Log("created object "+ds.gameObject.name+ " from "+ this.transform.name);
            ds.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3 { z = 3 });
            target.SetParent(null);
            Destroy(target.gameObject);
            target = ds;
            world = true;
        }
        callBack?.Invoke(target);


    }
    void OnItemWorldToUI(Transform target, Action<Transform> callBack)
    {
        if (world)
        {

            Transform ds = Instantiate(target.GetComponent<items>().data.UIprefabe, itemTransfareHolder).transform;
            Destroy(target.gameObject);
            target = ds;
            world = false;
            target.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3 { z = 3 });
        }

        callBack?.Invoke(target);

    }


    void saveTheDict(Dictionary<ItemData, float> allPowerUpsItems)
    {
        SavedPowerUp savedItems = new SavedPowerUp(allPowerUpsItems);

        string json = JsonUtility.ToJson(savedItems, true);

        string filePath = Path.Combine(Application.dataPath, "Resources/" + "powerUps.txt");
        File.WriteAllText(filePath, json);

        Debug.Log("Dictionary saved at: " + filePath);
    }
    //if add that means the item is being added to the inventory
    void save(inventoryHolder inventoryholder, ItemData _itemData, bool ADD)
    {
        inventory _inventory = inventoryholder._inventory;
        if (_inventory.alllowItemUse)
        {

            items[] hi;
            hi = inventoryholder.save();
            if (hi != null)
            {

                float EffectMultiplier;
                if (_itemData.UsableRL)
                {
                    foreach (ItemData itemData in _itemData.EffectedItems)
                    {
                        if (ADD)
                        {


                            if (inventory.AllPowerUpsItems.TryGetValue(itemData, out EffectMultiplier))
                            {

                                inventory.AllPowerUpsItems[itemData] *= _itemData.Multiplier;
                            }
                            else
                            {
                                inventory.AllPowerUpsItems.Add(itemData, _itemData.Multiplier);
                            }
                        }
                        else
                        {
                            if (inventory.AllPowerUpsItems.TryGetValue(itemData, out EffectMultiplier))
                            {

                                inventory.AllPowerUpsItems[itemData] /= _itemData.Multiplier;
                                if (inventory.AllPowerUpsItems[itemData] == 1)
                                {
                                    inventory.AllPowerUpsItems.Remove(itemData);
                                }
                            }

                        }
                    }
                    Debug.Log("nuumber of the list items : " + inventory.AllPowerUpsItems.Count);
                    if (inventory.AllPowerUpsItems.Count != 0)
                        Debug.Log("first item " + inventory.AllPowerUpsItems.First());
                    saveTheDict(inventory.AllPowerUpsItems);
                }


            }
        }
        else
        {
            inventoryholder.save();
        }

    }
    bool world = false;
    // Update is called once per frame
    public GraphicRaycaster raycaster;
    IEnumerator FollowMouse(Transform taget, Vector3 toSub)
    {// u want now to make this area just triggers and isolate all the logic to seperate places,
     // (dont be bakhel in the events, make all the events for every detail, like an event for changing from world to ui or the opposite)
     //----------------------------------------------------------------------------------------------------------------------------------------------------
        while (!Input.GetMouseButtonUp(0))
        {
            bool _aboveUI = doRayCast(Input.mousePosition, -1);
           
            if (!_aboveUI && !world)
                {//converted the item from ui to world
                 //add later an object in the outside world to store the objects that are on the floor  change ------------------------ important

                    taget = TriggerItemUIToWorld(taget);
                    yield return null;
                }
                else if (world && _aboveUI)
                {//converted the item from world to ui
                    taget = TriggerItemWorldToUI(taget);
                    yield return null;

                }
            if (world)
            {   //draging the item as a world component
                TriggerItemDraggingInWorld(taget);
                taget.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3 { z = 3 });
            }
            else
            {//dragging the item as a ui component

                TriggerItemDraggingInUI(taget);
                taget.position = Input.mousePosition;
            }
            //Debug.Log(Input.mousePosition.x);
            //Debug.Log("sdsd   " + widthFitter.mostLeft);
            yield return null;

        }
        if (world)
        {
            TriggerItemDroppedInWorld(taget);
            //dropped the item in the world
            yield break;
        }
        List<RaycastResult> results2 = new List<RaycastResult>();
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        raycaster.Raycast(eventData, results2);
        Vector2 result;
        if (results2.Count > 1 && results2[1].gameObject.CompareTag("slot"))
        {
            results2[0].gameObject.GetComponent<Image>().raycastTarget = false;

            if (doRayCast(results2[1].gameObject.transform.position, 0))
            {


                // Debug.Log(results2[1].gameObject.name);
                if (check(results2[0].gameObject.GetComponent<items>().Dimention, results2[1].gameObject.transform.position, out result))
                {//                                                                                                                   the slot ^

                    // taget.position = results2[1].gameObject.transform.position - toSub;
                    taget.SetParent(results2[1].gameObject.transform.parent.GetChild((results2[1].gameObject.transform.parent.childCount - 1)));
                    //u stopped here (old)
                    taget.position = result;
                    yield return null;
                    save(results2[1].gameObject.transform.parent.GetComponent<inventoryHolder>(), taget.GetComponent<items>().data, true);


                    //Debug.Log(result);

                }
                else
                {
                    taget.position = results[1].gameObject.transform.position - toSub;
                    //no enough space
                    redoTheParent(taget);
                    yield return null;
                    save(results[1].gameObject.transform.parent.GetComponent<inventoryHolder>(), taget.GetComponent<items>().data, true);
                }


            }
            else
            {


                redoTheParent(taget);
                taget.position = results[1].gameObject.transform.position - toSub;


                yield return null;
                save(results[1].gameObject.transform.parent.GetComponent<inventoryHolder>(), taget.GetComponent<items>().data, true);
                //there are no slot
                //Debug.Log("heeeeerereeee");
            }
        }
        else
        {
            //it will trigger errpr if it came from the world and put in no slot place or not empty 
            //there are no slot
            try
            {
                taget.position = results[1].gameObject.transform.position - toSub;
            }
            catch (Exception e)
            {
                //here u want to trigger the function of changing from ui to world(implement in it that if it was world already dont do anything, just change world to true)
                TriggerItemUIToWorld(taget);
                //   Debug.Log("dropped in the world");

            }
            redoTheParent(taget);

            yield return null;
            try
            {

                save(results[1].gameObject.transform.parent.GetComponent<inventoryHolder>(), taget.GetComponent<items>().data, true);
            }
            catch (Exception e)
            {

            }
            //  Debug.Log("heeeeerereeeessedwe2");

        }
        try // ----------------------------------------------------------- important u need to check if the position inside the ground so the things doesnt get lost in the ground and be teleported above it
        {

            results2[0].gameObject.GetComponent<Image>().raycastTarget = true;
        }
        catch (Exception e)
        {
            //it is ouside the pannal (maybe if u want to add the drop functionality like this)
            Debug.LogWarning(e);
        }


    }

    static List<RaycastResult> results = new List<RaycastResult>();


    //and make a function that do a raycast ata specific opsition and returns the ful list
    List<RaycastResult> doRayCast(Vector2 position)
    {
        List<RaycastResult> result = new List<RaycastResult>();

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = position;
        raycaster.Raycast(eventData, result);

        return result;



    }
    // x= (-) if u want to check if the dragged item is above ui or not
    bool doRayCast(Vector2 position, int x)
    {
        List<RaycastResult> result = new List<RaycastResult>();

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = position;

        raycaster.Raycast(eventData, result);
       // Debug.Log("ui elemts under the mouse " + result.Count);
        if (x < 0)
        {
            if (result.Count < 3)
            {
                return false; // it is not above ui
            }
            else
            {
                return true;//it is above ui
            }
        }
        if (result.Count > 1)
        {
            //Debug.Log($"{result[x].gameObject.CompareTag("slot")}  the name of the object that was hit {result[x].gameObject.name}");
            // Debug.Log(eventData.position);
            return result[x].gameObject.CompareTag("slot");
        }
        else
            return false;
    }
    public bool right, left, up, down;
    bool check(Vector2 dimensions, Vector2 Newslot, out Vector2 result)
    {
        result = Vector2.zero;
        // Debug.Log(Newslot);
        //   Debug.Log(dimensions);

        right = false; left = false;
        up = false; down = false;


        int x = 1;
        int y = 0;
        float maxRight = 0f;
        float maxTop = 0f;
        float maxLeft = 0f;
        float maxDown = 0f;

        Newslot += new Vector2 { x = step / 2, y = step / 2 };
        //this approach search in each direction if not found it revers it whith the ability to add to what it did
        Vector2 orignalSlot = Newslot;

        //Debug.Log(orignalSlot);
        if (!doRayCast(orignalSlot, 0))
        {
            return false;
        }
        for (int i = 1; i <= dimensions.x; i++)
        {
            // Debug.Log("hereee");
            if (x >= dimensions.x)
            {
                maxRight = (Newslot + Vector2.right * step * (i - 1)).x;
                //Debug.Log(maxRight);

                break;

            }
            // Debug.Log("01");
            if (!doRayCast(Newslot + Vector2.right * step * i, 0))
            {
                right = true;
                maxRight = (Newslot + Vector2.right * step * (i - 1)).x;
                ////Debug.Log(maxRight);


                break;

            }
            x++;

        }
        maxLeft = Newslot.x;
        if (right)
        {
            for (int i = 1; i <= dimensions.x; i++)
            {
                if (x >= dimensions.x)
                {
                    maxLeft = (Newslot + Vector2.left * step * (i - 1)).x;

                    break;

                }
                //Debug.Log("02");

                if (!doRayCast(Newslot + Vector2.left * step * i, 0))
                {
                    left = true;
                    maxLeft = (Newslot + Vector2.left * step * (i - 1)).x;
                    break;

                }
                x++;

            }

        }
        //Debug.Log($"the ;middle; right {right} up {up} down {down} left {left} middle ");

        if (right && left)
        {
            up = true;
            //Debug.Log($"the ;middle; right {right} up {up} down {down} left {left} middle ");

            right = false;
            left = false;
            return false;
        }
        right = false;
        left = false;

        y++;


        for (int j = 1; j <= dimensions.y; j++)
        {
            x = 1;


            if (y >= dimensions.y)
            {
                maxTop = (Newslot).y;

                break;

            }
            //Debug.Log("03");

            if (!doRayCast(Newslot += Vector2.up * step, 0))
            {
                up = true;
                maxTop = (Newslot - Vector2.up * step).y;
                break;

            }



            for (int i = 1; i <= dimensions.x; i++)
            {
                if (Mathf.Abs((Newslot + Vector2.right * step * (i)).x) > Mathf.Abs(maxRight))
                {
                    break;
                }



                if (x >= dimensions.x)
                {
                    maxRight = (Newslot + Vector2.right * step * (i - 1)).x; //test

                    break;

                }
                //Debug.Log($"j  {j}   04");
                //Debug.Log(Newslot + Vector2.right * step * i);
                //Debug.Log("the logeofds");
                if (!doRayCast(Newslot + Vector2.right * step * i, 0))
                {
                    right = true;
                    maxRight = (Newslot + Vector2.right * step * (i - 1)).x;

                    break;

                }
                x++;

            }
            if (right)
            {
                for (int i = 1; i <= dimensions.x; i++)
                {
                    if (Mathf.Abs((Newslot + Vector2.left * step * (i)).x) > Mathf.Abs(maxLeft))
                    {
                        break;
                    }
                    if (x >= dimensions.x)
                    {
                        maxLeft = (Newslot + Vector2.left * step * (i - 1)).x;

                        break;

                    }
                    //Debug.Log("05");

                    if (!doRayCast(Newslot + Vector2.left * step * i, 0))
                    {
                        left = true;


                        maxLeft = (Newslot + Vector2.left * step * (i - 1)).x;
                        //stopped here
                        break;

                    }
                    x++;

                }

            }
            //Debug.Log($"the ;up; right {right} up {up} down {down} left {left} j {j} ");

            if (right && left)
            {
                up = true;
                //Debug.Log($"the ;up; right {right} up {up} down {down} left {left} j {j} ");
                maxTop = (Newslot - Vector2.up * step).y;
                right = false;
                left = false;
                break;
            }
            right = false;
            left = false;

            y++;
        }

        right = false;
        left = false;
        maxDown = orignalSlot.y;
        if (up)
        {
            Newslot = orignalSlot;
            for (int j = 1; j <= dimensions.y; j++)
            {
                x = 1;

                if (y >= dimensions.y)
                {
                    maxDown = (Newslot).y;

                    break;

                }
                //Debug.Log("06");

                if (!doRayCast(Newslot += Vector2.down * step, 0))
                {
                    down = true;
                    maxDown = (Newslot - Vector2.down * step).y;
                    break;

                }



                for (int i = 1; i <= dimensions.x; i++)
                {
                    if (Mathf.Abs((Newslot + Vector2.right * step * (i)).x) > Mathf.Abs(maxRight))
                    {
                        break;
                    }
                    if (x >= dimensions.x)
                    {
                        maxRight = (Newslot + Vector2.right * step * (i - 1)).x;

                        break;

                    }
                    //Debug.Log("07");

                    if (!doRayCast(Newslot + Vector2.right * step * i, 0))
                    {

                        //Debug.Log($"in j {j} the right was closed");

                        right = true;
                        maxRight = (Newslot + Vector2.right * step * (i - 1)).x;

                        break;

                    }
                    x++;

                }
                if (right)
                {
                    //Debug.Log($"in j {j} the right was closed");
                    for (int i = 1; i <= dimensions.x; i++)
                    {
                        if (Mathf.Abs((Newslot + Vector2.left * step * (i)).x) > Mathf.Abs(maxLeft))
                        {
                            break;
                        }
                        if (x >= dimensions.x)
                        {
                            maxLeft = (Newslot + Vector2.left * step * (i - 1)).x;

                            break;

                        }
                        //Debug.Log($" j {j}  08");

                        if (!doRayCast(Newslot + Vector2.left * step * i, 0))
                        {
                            left = true;
                            maxLeft = (Newslot + Vector2.left * step * (i - 1)).x;

                            //Debug.Log($"in j {j} the left was closed");

                            break;

                        }
                        x++;

                    }

                }
                //Debug.Log($"the ;down; right {right} up {up} down {down} left {left} j {j} ");
                if (right && left)
                {
                    down = true;
                    //Debug.Log($"the ;down; right {right} up {up} down {down} left {left} j {j} ");
                    maxDown = (Newslot - Vector2.down * step).y;

                    right = false;
                    left = false;
                    break;
                }
                right = false;
                left = false;
                y++;
            }


        }

        //Debug.Log($" the maxieess  right {maxRight} left {maxLeft} top {maxTop} down {maxDown} ");
        if (up && down)
        {
            return false;
        }
        else
        {
            result = new Vector2 { x = (maxRight + maxLeft) / 2, y = (maxTop + maxDown) / 2 };
            return true;
        }
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            results.Clear();
            raycaster.Raycast(eventData, results);

            if (results.Count > 1)
            {
                GameObject hit = results[0].gameObject;
                Vector3 toSub = results[1].gameObject.transform.position - hit.transform.position; // slot - item
                //Debug.Log("Hit UI element: " + hit.name);
                if (hit.CompareTag("ok") && results[1].gameObject.CompareTag("slot"))
                { //the item   ^                    the slot ^
                    world = false;
                    oldParent = hit.transform.parent;
                    OldRotation = hit.transform.eulerAngles.z;
                    oldDimention = hit.GetComponent<items>().Dimention;
                    hit.transform.SetParent(itemTransfareHolder, true); // temprorarly putting it there
                    save(results[1].gameObject.transform.parent.GetComponent<inventoryHolder>(), hit.GetComponent<items>().data, false);
                    StartCoroutine(FollowMouse(hit.transform, toSub));
                }


                // Perform actions based on the hit UI element
            }
            else
            {

                //based on the hit world element
                Debug.Log("hello im here");
                Vector3 _mouseposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _mouseposition.z = 0;
                Vector3Int _cellPos = tileManager.tilemapssStatic[0].WorldToCell(_mouseposition);
                Debug.Log("hello im here " + _cellPos);

                foreach (Tilemap map in tileManager.tilemapssStatic)
                {

                    ItemData _tile;


                    TileBase _retrivedTile = map.GetTile(_cellPos);
                    Debug.Log("hello im here " + _retrivedTile);

                    // Debug.Log($"mouseposition {mouseposition} cell position {cellpos}  retrived tile {retrivedTile.name} ");
                    if (_retrivedTile != null)
                    {
                        Debug.Log("hello im here " + tileManager.tileBasData.Count);


                        if (tileManager.tileBasData.TryGetValue(_retrivedTile, out _tile))
                        {
                            Debug.Log("hello im here " + _tile);
                            //TODO : remeber to when you implement the put things like chest in the tiles, to now allow to put 2 usable items in Tiles above each other
                            if (_tile.UsableTile)
                            {
                                Debug.Log("he called itttttt");
                                events.TriggerTileUse((_tile, _retrivedTile), _cellPos);
                                Debug.Log("he called itttttt before nowwww ");

                            }

                        }
                    }
                }





                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
                if (hit.collider != null && hit.collider.CompareTag("ok"))
                {
                    oldParent = hit.transform.parent;
                    OldRotation = hit.transform.eulerAngles.z;
                    oldDimention = hit.transform.GetComponent<items>().Dimention;
                    //Debug.Log("hit in the 2d World " + hit.collider.name);
                    StartCoroutine(FollowMouse(hit.transform, Vector3.zero));

                }


            }
        }
    }
}
