
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class tileManager : MonoBehaviour
{
    [SerializeField] List<Tilemap> tilemaps;
    [SerializeField] List<TileData> tileDatas;

    Dictionary<TileBase, TileData> tileBasData = new Dictionary<TileBase, TileData>();
    private void Awake()
    {
        foreach (TileData _TileData in tileDatas)
        {
            foreach (TileBase _TileBase in _TileData.tiles)
            {
                tileBasData.Add(_TileBase, _TileData);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {


    }
    [SerializeField] float steadyThreshold = 1f;
    public float timeSinceTheLastSteady = 0;
    bool waitingForTheNextMove = false; // if true that means the mouse didnt movce since the last show of info
    public List<TileData> clickedTileDatas = new List<TileData>();

    Vector3Int lastCellPos = new Vector3Int();
    // Update is called once per frame
    void Update()
    {
        // Debug.Log(Input.GetAxis("Mouse X"));
        bool mouseXZero = Mathf.Approximately(Input.GetAxis("Mouse X"), 0);
        bool mouseYZero = Mathf.Approximately(Input.GetAxis("Mouse Y"), 0);
        if (!mouseXZero || !mouseYZero)
        {//the mouse moved after a steady
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

        if (mouseXZero && mouseYZero && !waitingForTheNextMove && timeSinceTheLastSteady >= steadyThreshold)
        {
            timeSinceTheLastSteady = 0;
            waitingForTheNextMove = true;
            clickedTileDatas.Clear();

            int i = 0;
            foreach (Tilemap map in tilemaps)
            {

                TileData tile;

                Vector3 mouseposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseposition.z = 0;
                Vector3Int cellpos = map.WorldToCell(mouseposition);
                if (i == 0)
                {
                    if (cellpos == lastCellPos)
                    {
                        return;
                    }

                    lastCellPos = cellpos;
                }
                i++;
                TileBase retrivedTile = map.GetTile(cellpos);
                // Debug.Log($"mouseposition {mouseposition} cell position {cellpos}  retrived tile {retrivedTile.name} ");
                if (retrivedTile != null)
                {

                    if (tileBasData.TryGetValue(retrivedTile, out tile))
                    {

                        Debug.Log(tile + " in " + map.name);
                        clickedTileDatas.Add(tile);
                    }
                }
            }
           
                events.TriggerShowItem(clickedTileDatas);
            


        }

    }
}
