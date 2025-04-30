using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//i cant remember what this was destroying
public class Destroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void destroy()
    {
        Destroy(transform.parent.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
