using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class TilemapRecentreTool : EditorWindow
{
    [MenuItem("Tools/Tilemap Recentre Tool")]
    public static void ShowWindow()
    {
        GetWindow<TilemapRecentreTool>("Tilemap Recentre Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Tilemap Recentre Options", EditorStyles.boldLabel);
        GUILayout.Space(10);

        if (GUILayout.Button("🔵 Recentre Selected (Before Prefab)", GUILayout.Height(40)))
        {
            RecentreSelectedTilemaps(false);
        }

        GUILayout.Space(10);

        if (GUILayout.Button("🟣 Recentre Selected (Apply to Prefab)", GUILayout.Height(40)))
        {
            RecentreSelectedTilemaps(true);
        }
    }

    private void RecentreSelectedTilemaps(bool applyPrefabChanges)
    {
        GameObject selected = Selection.activeGameObject;
        if (selected == null)
        {
            Debug.LogWarning("No GameObject selected!");
            return;
        }

        Tilemap[] tilemaps = selected.GetComponentsInChildren<Tilemap>();
        if (tilemaps.Length == 0)
        {
            Debug.LogWarning("Selected GameObject does not have any Tilemaps!");
            return;
        }

        string actionDescription = applyPrefabChanges ?
            "This will move ALL child Tilemaps closer to (0,0) and APPLY prefab changes automatically." :
            "This will move ALL child Tilemaps closer to (0,0) without applying any prefab changes.";

        if (!EditorUtility.DisplayDialog(
            "Recentre Tilemaps",
            actionDescription + "\n\nAre you sure you want to continue?",
            "Yes, recentre",
            "Cancel"))
        {
            Debug.Log("Recentre cancelled.");
            return;
        }

        Undo.RegisterCompleteObjectUndo(selected, "Recentre All Tilemaps");

        // Step 1: Find combined total bounds
        BoundsInt totalBounds = new BoundsInt();
        bool firstBounds = true;
        foreach (Tilemap tilemap in tilemaps)
        {
            tilemap.CompressBounds();
            if (firstBounds)
            {
                totalBounds = tilemap.cellBounds;
                firstBounds = false;
            }
            else
            {
                totalBounds.xMin = Mathf.Min(totalBounds.xMin, tilemap.cellBounds.xMin);
                totalBounds.yMin = Mathf.Min(totalBounds.yMin, tilemap.cellBounds.yMin);
                totalBounds.xMax = Mathf.Max(totalBounds.xMax, tilemap.cellBounds.xMax);
                totalBounds.yMax = Mathf.Max(totalBounds.yMax, tilemap.cellBounds.yMax);
            }
        }

        Vector3Int centreOffset = new Vector3Int(
            Mathf.FloorToInt((totalBounds.xMin + totalBounds.xMax) / 2f),
            Mathf.FloorToInt((totalBounds.yMin + totalBounds.yMax) / 2f),
            0);

        // Step 2: Move all tiles
        foreach (Tilemap tilemap in tilemaps)
        {
            BoundsInt bounds = tilemap.cellBounds;
            TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
            tilemap.ClearAllTiles();

            int index = 0;
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {
                    TileBase tile = allTiles[index++];
                    if (tile != null)
                    {
                        Vector3Int newPos = new Vector3Int(x - centreOffset.x, y - centreOffset.y, 0);
                        tilemap.SetTile(newPos, tile);
                    }
                }
            }
        }

        Debug.Log("Tilemaps recentred successfully!");

        if (applyPrefabChanges)
        {
            PrefabInstanceStatus prefabStatus = PrefabUtility.GetPrefabInstanceStatus(selected);
            if (prefabStatus == PrefabInstanceStatus.Connected || prefabStatus == PrefabInstanceStatus.Disconnected)
            {
                GameObject prefabRoot = PrefabUtility.GetNearestPrefabInstanceRoot(selected);
                PrefabUtility.ApplyPrefabInstance(prefabRoot, InteractionMode.UserAction);
                Debug.Log("Prefab changes applied automatically.");
            }
        }

        if (!EditorApplication.isPlaying)
        {
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
    }
}
