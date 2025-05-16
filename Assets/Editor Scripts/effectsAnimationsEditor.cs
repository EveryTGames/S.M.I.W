using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(effectsAnimations))]
public class effectsAnimationsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        effectsAnimations script = (effectsAnimations)target;

        if (GUILayout.Button("Auto Populate Animations from Assets"))
        {
            foreach (var anim in script._allAnimations)
            {
                anim.AnimationFrames = LoadTilesByName(anim.Name);
            }

            EditorUtility.SetDirty(script);
            Debug.Log("Tile animations auto-filled!");
        }
    }

    private TileBase[] LoadTilesByName(string baseName)
    {
        List<TileBase> tiles = new List<TileBase>();
        int index = 0;

        while (true)
        {
            string assetName = $"{baseName}_{index}";
            string[] guids = AssetDatabase.FindAssets(assetName + " t:TileBase");

            if (guids.Length == 0)
                break;

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            TileBase tile = AssetDatabase.LoadAssetAtPath<TileBase>(path);
            if (tile != null)
                tiles.Add(tile);

            index++;
        }

        return tiles.ToArray();
    }
}
