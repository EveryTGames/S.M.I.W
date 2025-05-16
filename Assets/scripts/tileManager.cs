
using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;





public class TileAnimator
{
    static int _animationIndex = 0;
    public static Sequence sequence;

    public TileAnimator(String animationName, (ItemData, TileBase) data, Vector3Int cellPos, bool keepLastFrame, float animationMultiplier, float? frameDuration = null, Action<(ItemData, TileBase), Vector3Int> onFinishing = null)
    {
        if (sequence != null)
        {

            return;
        }
        onFinishing = onFinishing ?? ((a, b) => { });

        TileAnimations animation = effectsAnimations.AllAnimations.FirstOrDefault(tileAnimation => tileAnimation.Name == animationName);

        if (animation == null)
        {

            Debug.LogError("didnt find animation " + animationName);
            return;
        }
        TileBase[] animationFrames = animation.AnimationFrames;

        frameDuration = frameDuration ?? data.Item1.TimeToBreak / (animationFrames.Length * animationMultiplier); //if not specified time i will just consider it for breaking animation for now
        sequence = DOTween.Sequence();
        sequence.intId = _animationIndex++;

        // Add the frames to the sequence
        for (int i = 0; i < animationFrames.Length; i++)
        {
            int frameIndex = i;
            sequence.AppendCallback(() => tileManager.tilemapssStatic[0].SetTile(cellPos, animationFrames[frameIndex]))
                    .AppendInterval((float)frameDuration);
        }



        // Remove the tiles
        sequence.AppendCallback(() =>
        {
            if (!keepLastFrame)
            {

                tileManager.tilemapssStatic[0].SetTile(cellPos, null); // removes the last frame effect
            }
            onFinishing.Invoke(data, cellPos);
            //  tileManager.findWhichTileMap(data.Item2, cellPos).SetTile(cellPos, null);
            // events.TriggerTileBreakEnd(data, cellPos);
        }).OnKill(() =>
        {
            if (!keepLastFrame)
            {
                tileManager.tilemapssStatic[0].SetTile(cellPos, null);
            }
            _animationIndex--;

            sequence = null;

        });

    }

    public static void stopLastAnimation()
    {
        if (sequence != null && sequence.IsPlaying())
        {
            // Debug.Log("it has been killed");
            DOTween.Kill(_animationIndex - 1);

        }
    }



}
public class tileManager : MonoBehaviour
{
    [SerializeField] List<Tilemap> tilemaps;

    public static List<Tilemap> tilemapssStatic;
    private List<ItemData> tileDatas;

    public static Dictionary<TileBase, ItemData> tileBasData = new Dictionary<TileBase, ItemData>();
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

    void OnTileBreakStart((ItemData, TileBase) data, Vector3Int cellPos, float currentUsedMultiplier)
    {
        try
        {

            new TileAnimator("breakingAnimation", data, cellPos, false, currentUsedMultiplier, null, (a, b) =>
            {
                Debug.Log("arrived hereee");

                tileManager.findWhichTileMap(data.Item2, cellPos).SetTile(cellPos, null);
                events.TriggerTileBreakEnd(data, cellPos);
            });
        }
        catch (Exception e)
        {
            Debug.Log("this?? " + e);
        }


    }




    void OnTileLeave(Vector3Int cellPos)
    {
        stopBeraking();

    }

    public static void stopBeraking()
    {
        TileAnimator.stopLastAnimation();
    }

    public static Tilemap findWhichTileMap(TileBase tile, Vector3Int cellPos)
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

