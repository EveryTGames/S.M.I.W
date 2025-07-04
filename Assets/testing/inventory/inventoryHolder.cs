using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;





public class inventoryHolder : MonoBehaviour
{
  public bool[][] availableSlots;
  public bool[][] originalAvailableSlots;

  public void DestroyTheInventory()
  {
    GenericInventories.unloadInventory(transform.parent.parent.parent.gameObject);
    TileAnimator.stopLastAnimation();
  }

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
  public void loodRandomItems()
  {

    Debug.Log("Loading random items");
    int max = allTheItems.allitems.Length;
    for (int i = 0; i < UnityEngine.Random.Range(1, 4); i++)//number of items
    {
      int randomIndex = UnityEngine.Random.Range(0, max);//the random item index

      // Debug.Log("Loading item with ID: " + randomIndex);
      ItemData item = allTheItems.allitems[randomIndex];
      bool placed = false; // flag to track if item was placed

      for (int x = 0; x < availableSlots[0].Length; x++)
      {
        for (int y = 0; y < availableSlots.Length; y++)
        {
          if (availableSlots[y][x])
          {
            // Debug.Log("Found available slot at: " + new Vector2(x, y));
            Vector2 orignalPosition = new Vector2(x, y);
            Vector2 result;
            int[] maxs;
            Debug.Log("Loading item with ID: " + randomIndex + " at position: " + orignalPosition + " with dimension: " + item.dimention);
            if (DragAndDrop.check(item.dimention, availableSlots, orignalPosition, out result, out maxs))
            {//                                                                                            the slot ^


              GameObject taget = Instantiate(allTheItems.allitems[item.ID].UIprefabe, transform.GetChild(transform.childCount - 1), false);

              taget.GetComponent<RectTransform>().anchoredPosition = result;


              taget.GetComponent<items>().setDimention(item.dimention);
              taget.GetComponent<items>().data = item;
              taget.GetComponent<items>().maxs = maxs;
              save();
              placed = true; // item was successfully placed
              break; // exit the y loop after placing the item


              //Debug.Log(result);

            }

          }
        }

        if (placed) break; // exit the x loop if item was placed
      }
      


    }
  }
  // Start is called before the first frame update
  public void load(bool[][] _availableSlots)
  {
    // GameObject Inev = Instantiate(InventoryPrefab, GameObject.Find("Viewport").transform);
    //stop the above tempo as it is for the full inventory instantuation at the end and even not here, but in the main mind

    // u need to replace the below loop for the new (shape bassed inventory grid using the load image pixel) // done in the file pixelImage Loader
    //for (int i = 0; i < _inventory.maxitems; i++)
    //{

    //    Instantiate(slotPrefab, Inev.transform.GetChild(0));
    //}
    availableSlots = (bool[][])_availableSlots.Clone();
    availableSlots[0] = (bool[])_availableSlots[0].Clone();
    availableSlots[1] = (bool[])_availableSlots[1].Clone();
    availableSlots[2] = (bool[])_availableSlots[2].Clone();
    availableSlots[3] = (bool[])_availableSlots[3].Clone();
    availableSlots[4] = (bool[])_availableSlots[4].Clone();
    originalAvailableSlots = (bool[][])_availableSlots.Clone();
    if (inventory.AllPowerUpsItems == null)
    {

      inventory.AllPowerUpsItems = loadAllPowerUpsItems();
    }
    if (!File.Exists(Path.Combine(Application.dataPath, "Resources/" + transform.name + ".txt")))
    {
      if (_inventory.randomeItems)
      {
        loodRandomItems();
      }



      Debug.LogWarning("No saved items found for this inventory: " + transform.name);
      return;
    }
    JsonSerializerSettings settings = new JsonSerializerSettings();
    settings.Converters.Add(new Vector2Converter());
    theLoadedItems = JsonConvert.DeserializeObject<savedItems>(File.ReadAllText(Path.Combine(Application.dataPath, "Resources/" + transform.name + ".txt")), settings);

    availableSlots = theLoadedItems.availableSlots;
    foreach (theLoadedItem item in theLoadedItems.items)
    {
      Debug.Log("Loading item with ID: " + item.ID + " at position: " + item.position + " with dimension: " + item.Dimention + "with maxs[0] " + item.maxs[0]);

      GameObject temp = Instantiate(allTheItems.allitems[item.ID].UIprefabe, transform.GetChild(transform.childCount - 1), false);
      temp.transform.localPosition = item.position;
      temp.GetComponent<items>().setDimention(item.Dimention);
      temp.GetComponent<items>().maxs = item.maxs;
      if (item.Dimention != allTheItems.allitems[item.ID].dimention)  // --------------------------needs optimization i think
      {

        temp.GetComponent<items>().Rotate();

      }
      //      Debug.Log(item.position);
    }

  }


  //TODO: it will not be preintialised after i finish it
  //TODO: add the logic of changing the availableslots array here
  public items[] save(/*bool[][] _availableSlots = null*/)
  {
    List<(int, Vector2, Vector2, int[])> __ite = new List<(int, Vector2, Vector2, int[])>();
    items[] hi = transform.GetChild(transform.childCount - 1).GetComponentsInChildren<items>();

    //TODO: if encountered any weird error remember to try to clone the array instead of equal
    Debug.Log(availableSlots.Length);
    availableSlots = (bool[][])originalAvailableSlots.Clone();
    availableSlots[0] = (bool[])originalAvailableSlots[0].Clone();
    availableSlots[1] = (bool[])originalAvailableSlots[1].Clone();
    availableSlots[2] = (bool[])originalAvailableSlots[2].Clone();
    availableSlots[3] = (bool[])originalAvailableSlots[3].Clone();
    availableSlots[4] = (bool[])originalAvailableSlots[4].Clone();
    foreach (items item in hi)
    {

      // Debug.Log($"the first slot in available slots is {availableSlots[0][0]}");
      // Debug.Log($"the first slot in orignal available slots is {originalAvailableSlots[0][0]}");
      // Debug.Log($"the item maxs[3] is {item.maxs[3]}");
      // Debug.Log($"the item maxs[2] is {item.maxs[2]}");
      // Debug.Log($"the item maxs[1] is {item.maxs[1]}");
      // Debug.Log($"the item maxs[0] is {item.maxs[0]}");
      for (int x = item.maxs[0]; x <= item.maxs[1]; x++)
      {
        for (int y = item.maxs[2]; y <= item.maxs[3]; y++)
        {
          Debug.Log(x + " " + y + "pos " + item.position);
          availableSlots[y][x] = false;

        }
      }
      // Debug.Log($"the first slot in available slots is {availableSlots[0][0]}");
      // Debug.Log($"the first slot in orignal available slots is {originalAvailableSlots[0][0]}");
      //  Debug.Log(item.gameObject);

      __ite.Add((item.data.ID, item.position, item.Dimention, item.maxs));

    }
    savedItems savedItems = new savedItems(__ite, availableSlots);
    JsonSerializerSettings settings = new JsonSerializerSettings();
    settings.Converters.Add(new Vector2Converter());

    string json = JsonConvert.SerializeObject(savedItems, Formatting.Indented, settings);



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
  public bool[][] availableSlots;
  public List<theLoadedItem> items = new List<theLoadedItem>();

  public savedItems() { }
  public savedItems(List<(int, Vector2, Vector2, int[])> _input, bool[][] _availableSlots)
  {
    items.Clear();
    availableSlots = _availableSlots;
    Debug.Log("number of items: " + _input.Count);
    foreach ((int, Vector2, Vector2, int[]) ins in _input)
    {
      items.Add(new theLoadedItem(ins.Item1, ins.Item2, ins.Item3, ins.Item4));


    }

  }
}

[Serializable]
public class theLoadedItem
{
  public int ID;
  public Vector2 position;

  public Vector2 Dimention;
  public int[] maxs;


  public theLoadedItem(int id, Vector2 position, Vector2 Dimention, int[] _maxs)
  {
    ID = id;
    this.position = position;
    this.Dimention = Dimention;
    this.maxs = _maxs;


  }







}
public class Vector2Converter : JsonConverter<Vector2>
{
  public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
  {
    float[] coords = serializer.Deserialize<float[]>(reader);
    return new Vector2(coords[0], coords[1]);
  }

  public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
  {
    serializer.Serialize(writer, new float[] { value.x, value.y });
  }
}