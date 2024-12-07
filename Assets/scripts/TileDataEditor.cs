using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(TileData))]
public class TileDataEditor : Editor
{
    private string tileNamePrefix = "World-Tiles_"; // Prefix for the tile sprite names
    private List<Vector2Int> ranges = new List<Vector2Int> { new Vector2Int(0, 10) }; // Default range
    private bool appendToExisting = false; // Option to add instead of overwrite
    private bool isRemoveMode = false; // Toggle between add and remove mode

    public override void OnInspectorGUI()
    {
        // Render default inspector properties for TileData (like Name, tiles, etc.)
        DrawDefaultInspector();

        // Iterate through all selected objects (targets) using SerializedObject
        foreach (SerializedObject serializedObj in serializedObject.targetObjects.Select(t => new SerializedObject(t)))
        {
            TileData tileData = (TileData)serializedObj.targetObject;

            serializedObj.Update(); // Update the serialized object

            GUILayout.Space(10);
            GUILayout.Label(isRemoveMode ? "Remove Tiles" : "Auto-Assign Tiles", EditorStyles.boldLabel);

            // Use the serializedProperty to modify fields
            tileNamePrefix = EditorGUILayout.TextField("Tile Name Prefix", tileNamePrefix);

            GUILayout.Label("Ranges (Start - End):");
            for (int i = 0; i < ranges.Count; i++)
            {
                GUILayout.BeginHorizontal();
                ranges[i] = new Vector2Int(
                    EditorGUILayout.IntField(ranges[i].x, GUILayout.Width(50)),
                    EditorGUILayout.IntField(ranges[i].y, GUILayout.Width(50))
                );

                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    ranges.RemoveAt(i);
                }
                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add New Range"))
            {
                ranges.Add(new Vector2Int(0, 10));
            }

            appendToExisting = EditorGUILayout.Toggle("Append to Existing", appendToExisting);
            isRemoveMode = EditorGUILayout.Toggle("Remove Mode", isRemoveMode);

            if (GUILayout.Button(isRemoveMode ? "Remove Tiles" : "Assign Tiles"))
            {
                if (isRemoveMode)
                    RemoveTileVariations(serializedObj, tileData);
                else
                    AssignTileVariations(serializedObj, tileData);
            }

            serializedObj.ApplyModifiedProperties(); // Apply the changes to the serialized object
        }
    }

    private void AssignTileVariations(SerializedObject serializedObj, TileData tileData)
    {
        string[] guids = AssetDatabase.FindAssets("t:TileBase");
        List<TileBase> matchedTiles = new List<TileBase>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TileBase tile = AssetDatabase.LoadAssetAtPath<TileBase>(path);

            if (tile != null && IsInRanges(tile.name))
            {
                matchedTiles.Add(tile);
            }
        }

        matchedTiles = matchedTiles.OrderBy(tile => ExtractIndex(tile.name)).ToList();

        if (appendToExisting)
        {
            var existingTiles = tileData.tiles != null ? tileData.tiles.ToList() : new List<TileBase>();
            existingTiles.AddRange(matchedTiles.Except(existingTiles)); // Avoid duplicates
            tileData.tiles = existingTiles.ToArray();
        }
        else
        {
            tileData.tiles = matchedTiles.ToArray();
        }

        serializedObj.ApplyModifiedProperties(); // Apply the changes to the serialized object

        EditorUtility.SetDirty(tileData);

        Debug.Log($"Assigned {matchedTiles.Count} tiles to {tileData.Name}.");
    }

    private void RemoveTileVariations(SerializedObject serializedObj, TileData tileData)
    {
        if (tileData.tiles == null || tileData.tiles.Length == 0)
        {
            Debug.LogWarning("No tiles to remove!");
            return;
        }

        var existingTiles = tileData.tiles.ToList();
        var tilesToRemove = new List<TileBase>();

        foreach (var tile in existingTiles)
        {
            if (IsInRanges(tile.name))
            {
                tilesToRemove.Add(tile);
            }
        }

        foreach (var tile in tilesToRemove)
        {
            existingTiles.Remove(tile);
        }

        tileData.tiles = existingTiles.ToArray();
        serializedObj.ApplyModifiedProperties(); // Apply the changes to the serialized object

        EditorUtility.SetDirty(tileData);

        Debug.Log($"Removed {tilesToRemove.Count} tiles from {tileData.Name}.");
    }

    private bool IsInRanges(string tileName)
    {
        if (!tileName.StartsWith(tileNamePrefix)) return false;

        int index = ExtractIndex(tileName);
        foreach (var range in ranges)
        {
            if (index >= range.x && index <= range.y)
                return true;
        }

        return false;
    }

    private int ExtractIndex(string tileName)
    {
        string numberPart = tileName.Substring(tileNamePrefix.Length);
        if (int.TryParse(numberPart, out int index))
        {
            return index;
        }

        return -1;
    }
}
