


using UnityEngine;
using System;


//this class is a parent for the checker
//if you puta  checker inheriting from this class you dont have to put this script too on the gameobject bc it will be added automaticilly or else it will give error
public class Collision_Checker : MonoBehaviour
{
   

    [SerializeField] string compareTag = "Ground"; // what tag this checker checks for

  [SerializeField]  Action<bool> assignFn;

     
 
    public void  assignFunction(Action<bool> _assignFn)
    {
        Debug.Log("i was called");
        assignFn = _assignFn;
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(compareTag))
        {

            assignFn.Invoke(true);
       
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag(compareTag))
        {
                assignFn.Invoke(true);
         
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag(compareTag))
        {
            assignFn.Invoke(false);
            
        }
    }
}
