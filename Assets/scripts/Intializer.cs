using UnityEngine;
using UnityEngine.UI;

//it is responsible for  the respawn of the info side show
public class Intializer : MonoBehaviour {
    

    [SerializeField] Text name_txt;
    [SerializeField] Text description_txt;
  
    
    public void assign(ItemData data )
    {
        name_txt.text = data.name;
        description_txt.text = data.description;
       

    }
}