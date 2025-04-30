
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

public class tileManager : MonoBehaviour
{
    [SerializeField] List<Tilemap> tilemaps;

    public static List<Tilemap> tilemapssStatic;
    private List<ItemData> tileDatas;

    Dictionary<TileBase, ItemData> tileBasData = new Dictionary<TileBase, ItemData>();
    private void Awake()
    {
        tilemapssStatic = tilemaps;
   
    }
    // Start is called before the first frame update
    void Start()
    {
        new ItemManager();
        tileDatas = ItemManager.GetAllTileItems();
        //Debug.Log("tile data count " + tileDatas.Count);
        foreach (ItemData _TileData in tileDatas)
        {
            foreach (TileBase _TileBase in _TileData.tiles)
            {
                tileBasData.Add(_TileBase, _TileData);
            }
        }






        events.onTileLeave += OnTileLeave;

        events.onTileBreakStart += OnTileBreakStart;
        events.onTileHoverStart += OnTileHoverStart;
    }

    public static List<(ItemData, TileBase)> clickedTileDatas = new List<(ItemData, TileBase)>();





    // Update is called once per frame
    void Update() // this is the responsible for all the tile manuiplation methods invokation
    {



    }

    void OnTileHoverStart(Vector3Int cellpos)
    {
        // it means that it went out of the old tile

        foreach (Tilemap map in tilemapssStatic)
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


        events.TriggerShowItem(clickedTileDatas, cellpos);
        clickedTileDatas.Clear();

    }


    static Sequence sequence;
    static int ff = 0;
    void OnTileBreakStart((ItemData, TileBase) data, Vector3Int cellPos, float currentUsedMultiplier)
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
            sequence.AppendCallback(() => tilemapssStatic[0].SetTile(cellPos, animationFrames[frameIndex]))
                    .AppendInterval(frameDuration);
        }



        // Remove the tiles
        sequence.AppendCallback(() =>
        {
            tilemapssStatic[0].SetTile(cellPos, null);
            findWhichTileMap(data.Item2, cellPos).SetTile(cellPos, null);
            events.TriggerTileBreakEnd(data, cellPos);
        }).OnKill(() =>
        {
            tilemapssStatic[0].SetTile(cellPos, null);
            sequence = null;
        });


    }




    void OnTileLeave(Vector3Int cellPos)
    {
        stopBeraking();

    }

    public static void stopBeraking()
    {
        if (sequence != null && sequence.IsPlaying())
        {
            // Debug.Log("it has been killed");
            DOTween.Kill(ff - 1);

        }
    }

    Tilemap findWhichTileMap(TileBase tile, Vector3Int cellPos)
    {
        foreach (Tilemap map in tilemapssStatic)
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

