using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

public enum InventoryTypes
{
    normalChest
}
public class GenericInventories : MonoBehaviour
{


    static int _lastID;//must be loaded/saved from a file
    public GameObject InventoryPrefab;
    static GameObject _inventoryPrefab;

    public Transform Canvas;
    static Transform _canvas;

    static List<string> _loadedInventories = new List<string>();
    [SerializeField] InventoryItem[] m_inventoriesDefinitions;
    static InventoryItem[] _inventoriesDefinitions;
    static HashSet<string> worldInventoriesIDs;//must be saved in a file so that all the inventories be saved, it will be like this 
    //inv_0    inv_1    inv_2


    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("im wawawawawadsda");
        _inventoryPrefab = InventoryPrefab;
        _canvas = Canvas;
        _inventoriesDefinitions = m_inventoriesDefinitions;
        LoadCellMap();
        LoadLastID();
    }

    public static void SaveLastID()
    {
        File.WriteAllText(Application.persistentDataPath + "/lastID.txt", _lastID.ToString());
    }
    public static void LoadLastID()
    {
        string path = Application.persistentDataPath + "/lastID.txt";
        if (!File.Exists(path))
            _lastID = 0; // Default value

        string content = File.ReadAllText(path);
        if (int.TryParse(content, out int id))
            _lastID = id;

       Debug.Log("loaded lastID " + _lastID + " from " + path);
    }



    public static void SaveCellMap()
    {
        List<CellData> data = new List<CellData>();
        foreach ((Vector3Int, int) item in _cellPosMapp)
            data.Add(new CellData(item.Item1, item.Item2));

        string json = JsonUtility.ToJson(new CellDataList { list = data }, true);
        File.WriteAllText(Application.persistentDataPath + "/cellMap.json", json);
        Debug.Log("saved cellmap " + Application.persistentDataPath);
    }

    [System.Serializable]
    public class CellDataList
    {
        public List<CellData> list;
    }
    public static void LoadCellMap()
    {
        string path = Application.persistentDataPath + "/cellMap.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            CellDataList data = JsonUtility.FromJson<CellDataList>(json);
            List<(Vector3Int, int)> cellMap = new List<(Vector3Int, int)>();
            foreach (var item in data.list)
                cellMap.Add((item.ToVector3Int(), item.value));
            _cellPosMapp = cellMap;
            Debug.Log("loaded cell map from " + path);
            return;
        }
        _cellPosMapp = new List<(Vector3Int, int)>();
    }

    static List<(Vector3Int, int)> _cellPosMapp = new List<(Vector3Int, int)>();//it must be saved/loaded in a file

    public static void AddToMap(Vector3Int cellPos, int ID = -1)//called when it is placed as a tile (didnt add this functinality yet ), but i used it when opening chest just for the test
    {
        foreach ((Vector3Int, int) item in _cellPosMapp)
        {
            if (item.Item1 == cellPos)
            {
                Debug.Log("it is already in the map");
                return;
            }

        }
        _cellPosMapp.Add((cellPos, (ID < 0) ? _lastID++ : ID));
        SaveCellMap();
        SaveLastID();

    }
    public static void removeFromMap(Vector3Int cellPos)//called when the chest tile is destroyed
    {
        for (int i = 0; i < _cellPosMapp.Count; i++)
        {
            if (_cellPosMapp[i].Item1 == cellPos)
            {
                _cellPosMapp.RemoveAt(i);
                SaveCellMap();
                return;
            }
        }
    }



    public static bool isLoaded(string nameID)
    {
        foreach (string id in _loadedInventories)
        {
            if (id == nameID)
            {
                return true;
            }
        }
        return false;
    }

    public static int CellPosMap(Vector3Int cellPos)
    {
        foreach ((Vector3Int, int) item in _cellPosMapp)
        {
            if (item.Item1 == cellPos)
            {

                return item.Item2;
            }

        }

        return -1;


    }

    public static void unloadInventory(GameObject gameObject)
    {
        _loadedInventories.Remove(gameObject.name);
        gameObject.transform.parent = null;
        Destroy(gameObject);
    }
    public static GameObject loadInventory(InventoryTypes inventoryType, string nameID = "inv_-1")
    {

        int ID = int.Parse(nameID.Substring(4));
        if (ID == -1) //that means the inventory is completly new and it will be empty, spomething is wrong with this
        {
            ID = _lastID++;
            Debug.Log("the id shiouyd be saved now 234q23t6wtwe4targ");
            SaveLastID();

        }

        foreach (InventoryItem item in _inventoriesDefinitions)
        {
            if (item.InventoryType == inventoryType)
            {


                //create a new inventory object in UI of the type item.Inventory
                GameObject newInventory = Instantiate(_inventoryPrefab, Input.mousePosition, Quaternion.identity, _canvas);
                newInventory.transform.SetSiblingIndex(newInventory.transform.parent.childCount - 2);
                newInventory.name = "inv_" + ID;
                PixelImageLoader x = newInventory.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<PixelImageLoader>();
                x.transform.name = "inv_" + ID;
                x.inventory = item.Inventory;

                _loadedInventories.Add("inv_" + ID);



                return newInventory;
            }


        }
        return null;




    }

    // Update is called once per frame
    void Update()
    {

    }
}

[Serializable]
public class InventoryItem
{
    public InventoryTypes InventoryType;
    public inventory Inventory;



}


[System.Serializable]
public class CellData
{
    public int x, y, z;
    public int value;

    public CellData(Vector3Int position, int val)
    {
        x = position.x;
        y = position.y;
        z = position.z;
        value = val;
    }

    public Vector3Int ToVector3Int() => new Vector3Int(x, y, z);
}
