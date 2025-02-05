
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;
using static events;
public class tileManager : MonoBehaviour
{
    [SerializeField] List<Tilemap> tilemaps;
    [SerializeField] List<ItemData> tileDatas;

    Dictionary<TileBase, ItemData> tileBasData = new Dictionary<TileBase, ItemData>();
    private void Awake()
    {
        foreach (ItemData _TileData in tileDatas)
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
        onInventoryToggle += OnInventoryToggle;
        onTileLeave += OnTileLeave;
        onMouseUp += OnMouseUp;
        onMouseDown += OnMouseDown;
        onTileBreakStart += OnTileBreakStart;
        onTileHoverStart += OnTileHoverStart;
    }
    [SerializeField] float steadyThreshold = 1f;
    public float timeSinceTheLastSteady = 0;
    bool waitingForTheNextMove = false; // if true that means the mouse didnt movce since the last show of info
    List<(ItemData, TileBase)> clickedTileDatas = new List<(ItemData, TileBase)>();

    Vector3Int lastCellPos = new Vector3Int();
    Vector3Int lastCellPos2 = new Vector3Int();


    bool enableTileManuplation = true;
    void OnInventoryToggle(bool toggleTo)
    {
        enableTileManuplation = !toggleTo;
    }

    // Update is called once per frame
    void Update() // this is the responsible for all the tile manuiplation methods invokation
    {

        if(!enableTileManuplation)
        {
            return;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            TriggerMouseUp();

        }
        // Debug.Log(Input.GetAxis("Mouse X"));
        bool mouseXZero = Mathf.Approximately(Input.GetAxis("Mouse X"), 0);
        bool mouseYZero = Mathf.Approximately(Input.GetAxis("Mouse Y"), 0);


        Vector3 mouseposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseposition.z = 0;
        Vector3Int cellpos2 = tilemaps[0].WorldToCell(mouseposition);
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //Debug.Log("sajwakwj a "+ cellpos2);
            TriggerMouseDown(cellpos2);
        }

        if (!mouseXZero || !mouseYZero)
        {//the mouse moved after a steady


            if (cellpos2 == lastCellPos2)
            {
                //the hover continues
                // Debug.Log("the hover continues /fast);
                TriggerTileMovingStay(cellpos2);

            }
            else
            {
                TriggerTileLeave(lastCellPos2);
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
                clickedTileDatas.Clear();



                if (cellpos2 == lastCellPos)
                {
                    //the hover continues
                    // Debug.Log("the hover continues /slow");
                    TriggerTileHoverStay(cellpos2);
                    return;
                }
                else
                {
                    TriggerTileHoverStart(cellpos2);
                    //  Debug.Log("the hover starts/ slow");

                }

                lastCellPos = cellpos2;
            }
        }



    }

    void OnTileHoverStart(Vector3Int cellpos)
    {
        // it means that it went out of the old tile

        foreach (Tilemap map in tilemaps)
        {

            ItemData tile;


            TileBase retrivedTile = map.GetTile(cellpos);
            // Debug.Log($"mouseposition {mouseposition} cell position {cellpos}  retrived tile {retrivedTile.name} ");
            if (retrivedTile != null)
            {

                if (tileBasData.TryGetValue(retrivedTile, out tile))
                {

                    //Debug.Log(tile + " in " + map.name);
                    clickedTileDatas.Add((tile, retrivedTile));
                }
            }
        }


        TriggerShowItem(clickedTileDatas, cellpos);
        clickedTileDatas.Clear();

    }

    
    static Sequence sequence;
    int ff = 0;
    void OnTileBreakStart((ItemData, TileBase) data, Vector3Int cellPos,float currentUsedMultiplier)
    {
        if (sequence != null)
        {
            // Debug.Log(sequence.intId+ " " + data.Item2);
            return;
        }
        //Debug.Log("in the start tile break ");
        TileBase[] animationFrames = effectsAnimations.BreakingAnimationFrames;
        float frameDuration = data.Item1.TimeToBreak / (animationFrames.Length * currentUsedMultiplier);
        sequence = DOTween.Sequence();
        sequence.intId = ff++;

        // Add the frames to the sequence
        for (int i = 0; i < animationFrames.Length; i++)
        {
            int frameIndex = i;
            sequence.AppendCallback(() => tilemaps[0].SetTile(cellPos, animationFrames[frameIndex]))
                    .AppendInterval(frameDuration);
        }



        // Remove the tiles
        sequence.AppendCallback(() =>
        {
            tilemaps[0].SetTile(cellPos, null);
            findWhichTileMap(data.Item2, cellPos).SetTile(cellPos, null);
            TriggerTileBreakEnd(data, cellPos);
        }).OnKill(() =>
        {
            tilemaps[0].SetTile(cellPos, null);
            sequence = null;
        });


    }



    private void OnMouseDown(Vector3Int cellPos)
    {
        TriggerTileHoverStart(cellPos);

    }
    private void OnMouseUp()
    {
        stopBeraking();
    }
    void OnTileLeave(Vector3Int cellPos)
    {
        stopBeraking();

    }

    void stopBeraking()
    {
        if (sequence != null && sequence.IsPlaying())
        {
            // Debug.Log("it has been killed");
            DOTween.Kill(ff - 1);

        }
    }

    Tilemap findWhichTileMap(TileBase tile, Vector3Int cellPos)
    {
        foreach (Tilemap map in tilemaps)
        {
            if (map.GetTile(cellPos) == tile)
            {
                return map;
            }
        }
        Debug.LogError("no map found for " + tile + " at " + cellPos);
        return null;
    }

}

