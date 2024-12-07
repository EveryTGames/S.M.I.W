
using System.Collections.Generic;

using UnityEngine;
using System;
using static events;


public class Ground_Checker : MonoBehaviour
{
   
    public enum   WTCTypes
    { 
        Ground,
        forwardLeg,
        forwardHead

    }; // what this checker checks for

    [SerializeField] WTCTypes WTC;
    [SerializeField] string compareTag = "Ground"; // what tag this checker checks for

    Action<bool> assignFn;

     
 




    // Start is called before the first frame update
    void Start()
    {
       
       
       
        
         switch (WTC)
        {
            case WTCTypes.Ground:
                assignFn = static (state) => TriggerGroundCheck(state);
                break;

            case WTCTypes.forwardLeg:
                assignFn = (state) => TriggerForwardLegCheck(state);
                break;

            case WTCTypes.forwardHead:
                assignFn = (state) => TriggerForwardHeadCheck(state);
                break;

            default:
                Debug.LogError($"Unknown checkType: {WTC}");
                break;
        }

        

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
