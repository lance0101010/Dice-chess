using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class WallManager : MonoBehaviour
{
    public static WallManager Instance;
    public GameObject wallPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaceWallAt(Vector2Int gridPos)
    {
        Vector3 worldPos = BoardUtility.GridToWorld(gridPos); // 你已經有這個工具！
        GameObject wall = Instantiate(wallPrefab, worldPos, Quaternion.identity);
        wall.name = $"Wall_{gridPos.x}_{gridPos.y}";
        Debug.Log($"🧱 牆要放在邏輯座標 {gridPos}");
        BoardUtility.RegisterWall(gridPos, wall); // ✅ 註冊進牆資料表


    }
}
