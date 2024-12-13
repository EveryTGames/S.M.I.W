using UnityEngine;
using UnityEngine.UI;

public class Intializer : MonoBehaviour {
    

    [SerializeField] Text name_txt;
    [SerializeField] Text description_txt;
  
    public ItemData _data;
    public void assign(ItemData data )
    {
        name_txt.text = data.name;
        description_txt.text = data.description;
        _data=data;

    }
}