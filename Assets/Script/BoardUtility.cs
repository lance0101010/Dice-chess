using UnityEngine;
using System.Collections.Generic;
public static class BoardUtility
{
    public static float spacing = 1.1f;
    public static float yOffset = 1f;

    // 根據你提供的中心點偏移計算得出
    private static float offsetX = 0.081961f;
    private static float offsetZ = -0.03159f;

    public static Dictionary<Vector2Int, GameObject> wallMap = new Dictionary<Vector2Int, GameObject>();

    public static bool HasWallAt(Vector2Int gridPos)
    {
        return wallMap.ContainsKey(gridPos);
    }

    public static void RegisterWall(Vector2Int gridPos, GameObject wallObj)
    {
        wallMap[gridPos] = wallObj;
    }

    public static void RemoveWall(Vector2Int gridPos)
    {
        if (wallMap.ContainsKey(gridPos))
            wallMap.Remove(gridPos);
    }

    public static Vector3 GridToWorld(Vector2Int gridPos)
    {
        float x = gridPos.x * spacing + offsetX;
        float y = 1.0f;
        float z = gridPos.y * spacing + offsetZ;
        return new Vector3(x, y, z);
    }

    public static GameObject GetWallAt(Vector2Int pos)
    {
        if (wallMap.TryGetValue(pos, out GameObject wall))
            return wall;
        return null;
    }
    public static Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / 1.1f); // 注意 spacing 應該與棋盤一致
        int y = Mathf.RoundToInt(worldPos.z / 1.1f);
        return new Vector2Int(x, y);
    }

}
