using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class tileManagerInputHandler : MonoBehaviour
{





        [SerializeField] float steadyThreshold = 0.01f;
     float timeSinceTheLastSteady = 0;
    bool waitingForTheNextMove = false; // if true that means the mouse didnt movce since the last show of info
   
    Vector3Int lastCellPos = new Vector3Int();
    Vector3Int lastCellPos2 = new Vector3Int();
    // Start is called before the first frame update
    void Start()
    {
         events.onInventoryToggle += OnInventoryToggle;
         events.onMouseUp += OnMouseUpp;
        events.onMouseDown += OnMouseDownn;
        
    }


      private void OnMouseDownn(Vector3Int cellPos)
    {
        events.TriggerTileHoverStart(cellPos);

    }
    private void OnMouseUpp()
    {
        tileManager.stopBeraking();
    }


      bool enableTileManuplation = true;
    void OnInventoryToggle(bool toggleTo)
    {
        enableTileManuplation = !toggleTo;
    }

    // Update is called once per frame
    void Update()
    {
         if (!enableTileManuplation)
        {
            return;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            events.TriggerMouseUp();

        }
        // Debug.Log(Input.GetAxis("Mouse X"));
        bool mouseXZero = Mathf.Approximately(Input.GetAxis("Mouse X"), 0);
        bool mouseYZero = Mathf.Approximately(Input.GetAxis("Mouse Y"), 0);


        Vector3 mouseposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseposition.z = 0;
        Vector3Int cellpos2 = tileManager.tilemapssStatic[0].WorldToCell(mouseposition);
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //Debug.Log("sajwakwj a "+ cellpos2);
            events.TriggerMouseDown(cellpos2);
        }

        if (!mouseXZero || !mouseYZero)
        {//the mouse moved after a steady


            if (cellpos2 == lastCellPos2)
            {
                //the hover continues
                // Debug.Log("the hover continues /fast);
                events.TriggerTileMovingStay(cellpos2);

            }
            else
            {
                events.TriggerTileLeave(lastCellPos2);
                // TriggerTileEnter(cellpos2);
                //  Debug.Log("the hover starts/ fast");

            }

            lastCellPos2 = cellpos2;




            if (waitingForTheNextMove)
            {

                waitingForTheNextMove = false;
            }
            timeSinceTheLastSteady = 0;
        }
        else
        { // the mouse is in the steady state
            timeSinceTheLastSteady += Time.deltaTime;
        }

        if (mouseXZero && mouseYZero && timeSinceTheLastSteady >= steadyThreshold)
        {

            // Debug.Log("continues hover (not in the same tile, just hovering)  / fast"); here
            // TriggerTileHoverStayFast();


            if (!waitingForTheNextMove)
            {


                timeSinceTheLastSteady = 0;
                waitingForTheNextMove = true;
                tileManager.clickedTileDatas.Clear();



                if (cellpos2 == lastCellPos)
                {
                    //the hover continues
                    // Debug.Log("the hover continues /slow");
                    events.TriggerTileHoverStay(cellpos2);
                    return;
                }
                else
                {
                    events.TriggerTileHoverStart(cellpos2);
                    //  Debug.Log("the hover starts/ slow");

                }

                lastCellPos = cellpos2;
            }
        }


        
    }
}
