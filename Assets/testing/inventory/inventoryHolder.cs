using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct allitems
{
  public GameObject prefab;
}




public class inventoryHolder : MonoBehaviour
{

  Dictionary<ItemData, float> loadAllPowerUpsItems()
  {
    string filePath = Path.Combine(Application.dataPath, "Resources/" + "powerUps.txt");

    if (!File.Exists(filePath))
    {
      Debug.LogWarning("Save file not found! Returning empty dictionary.");
      return new Dictionary<ItemData, float>(); // Return empty if no file exists
    }

    string json = File.ReadAllText(filePath);
    SavedPowerUp savedItems = JsonUtility.FromJson<SavedPowerUp>(json);

    Dictionary<ItemData, float> loadedDictionary = new Dictionary<ItemData, float>();

    if (savedItems != null && savedItems.items != null)
    {
      foreach (var entry in savedItems.items)
      {
        ItemData itemData = allTheItems.allitems[entry.itemID];

        loadedDictionary[itemData] = entry.multiplier;

      }
    }

    Debug.Log("Dictionary loaded successfully!");
    return loadedDictionary;
  }

  // [Header("what i added i dont know i it should be here")]
  //public allitems[] allitems;
  public inventory _inventory;
  private void Start()
  {
    _inventory = GetComponent<PixelImageLoader>().inventory;
  }



  [Header("what was actually there")]
  //public inventory _inventory;
  //public GameObject InventoryPrefab;
  //public GameObject slotPrefab;
  //inventoryClsa PlayerInventory = new inventoryClsa();
  savedItems theLoadedItems;
  // Start is called before the first frame update
  public void load()
  {
    // GameObject Inev = Instantiate(InventoryPrefab, GameObject.Find("Viewport").transform);
    //stop the above tempo as it is for the full inventory instantuation at the end and even not here, but in the main mind

    // u need to replace the below loop for the new (shape bassed inventory grid using the load image pixel) // done in the file pixelImage Loader
    //for (int i = 0; i < _inventory.maxitems; i++)
    //{

    //    Instantiate(slotPrefab, Inev.transform.GetChild(0));
    //}
    if (inventory.AllPowerUpsItems == null)
    {

      inventory.AllPowerUpsItems = loadAllPowerUpsItems();
    }
    theLoadedItems = JsonUtility.FromJson<savedItems>(File.ReadAllText(Path.Combine(Application.dataPath, "Resources/" + transform.name + ".txt")));
    foreach (bro ok in theLoadedItems.items)
    {

      GameObject temp = Instantiate(allTheItems.allitems[ok.ID].UIprefabe, transform.GetChild(transform.childCount - 1), false);
      temp.transform.localPosition = ok.position;
      temp.GetComponent<items>().setDimention(ok.Dimention);
      if (ok.Dimention != allTheItems.allitems[ok.ID].dimention)  // --------------------------needs optimization i think
      {

        temp.GetComponent<items>().Rotate();

      }
      //      Debug.Log(ok.position);
    }

  }



  public items[] save()
  {
    List<(int, Vector2, Vector2)> __ite = new List<(int, Vector2, Vector2)>();
    items[] hi = transform.GetChild(transform.childCount - 1).GetComponentsInChildren<items>();
    foreach (items item in hi)
    {
      //  Debug.Log(item.gameObject);

      __ite.Add((item.data.ID, item.position, item.Dimention));

    }
    savedItems savedItems = new savedItems(__ite);
    string json = JsonUtility.ToJson(savedItems);



    File.WriteAllText(Path.Combine(Application.dataPath, "Resources/" + transform.name + ".txt"), json);

    return hi;
    // PlayerInventory._items = new item(__ite);
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.H))
    {
      save();



    }
    if (Input.GetKeyDown(KeyCode.G))
    {
      load();



    }

  }
}

//[Serializable]
//public class inventoryClsa
//{

//  public  item _items;

//}


[Serializable]
public class savedItems
{

  public List<bro> items = new List<bro>();
  public savedItems(List<(int, Vector2, Vector2)> input)
  {
    items.Clear();
    foreach ((int, Vector2, Vector2) ins in input)
    {
      items.Add(new bro(ins.Item1, ins.Item2, ins.Item3));


    }

  }
}

[Serializable]
public class bro
{
  public int ID;
  public Vector2 position;

  public Vector2 Dimention;


  public bro(int id, Vector2 position, Vector2 Dimention)
  {
    ID = id;
    this.position = position;
    this.Dimention = Dimention;


  }

}