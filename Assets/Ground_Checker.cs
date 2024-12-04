using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;


public class Ground_Checker : MonoBehaviour
{
    static playerControler pc;
    public enum   WTCTypes
    { 
        Ground,
        forwardLeg,
        forwardHead

    }; // what this checker checks for

    public WTCTypes WTC;
    [SerializeField] string compareTag = "Ground"; // what this checker checks for

    Action<bool> assignFn;

     // in the tuble the first string is what will be added in WTC, and
    // the other one is the tag of the check will be "Ground" by default
    static Dictionary<string, Action<bool>> fieldMap = new Dictionary<string, Action<bool>>(){
        {"Ground", (statue) => pc.grounded = statue},
        {"forwardLeg", (statue) => pc.forwardLeg = statue},
        {"forwardHead", (statue) => pc.forwardHead = statue}

    };

    // Start is called before the first frame update
    void Start()
    {
        if (pc == null)
        {
            pc = transform.parent.parent.GetComponent<playerControler>();
        }
        if (!fieldMap.TryGetValue(WTC.ToString(), out assignFn))
        {
            Debug.LogError("u need to add the field in the field map");
        }

    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(compareTag))
        {

            assignFn.Invoke(true);
            //custom code for ground only
            if(WTC == WTCTypes.Ground)
            {
                pc.rb.velocity = new Vector3(0,0,0);
                pc.an.SetBool("Grounded_bool",true);
                pc.an.SetTrigger("grounded");
                pc.an.ResetTrigger("jump");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag(compareTag))
        {
            

            assignFn.Invoke(true);
            //custom code for ground only
            if(WTC == WTCTypes.Ground)
            {
                pc.an.SetBool("Grounded_bool",true);
                
            }
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Ground"))
        {
            assignFn.Invoke(false);
             //custom code for ground only
            if(WTC == WTCTypes.Ground)
            {
                pc.timeSinceLastGround = Time.time;
                pc.an.SetBool("Grounded_bool",false);
            }
        }
    }
}
