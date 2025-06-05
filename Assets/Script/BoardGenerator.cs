using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject playerPrefab;
    public float cellSpacing = 1.1f;
    public float yOffset = 0.5f;

    public Material inactiveMaterial;

    private Vector3[] spawnPositions = new Vector3[]
    {
        new Vector3(0, 0, 0),
        new Vector3(0, 0, 6),
        new Vector3(6, 0, 0),
        new Vector3(6, 0, 6),
    };

    private GameObject[] playerObjects = new GameObject[4];

    void Start()
    {
        // ✅ 產生棋盤格並設定 GridCell 資訊
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 7; y++)
            {
                Vector3 position = new Vector3(x * cellSpacing, 0, y * cellSpacing);
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);
                cell.name = $"Cell_{x}_{y}";
                cell.transform.parent = this.transform;

                // ✅ 自動加上 GridCell 腳本（如果 prefab 裡沒有）
                GridCell cellData = cell.GetComponent<GridCell>();
                if (cellData == null)
                    cellData = cell.AddComponent<GridCell>();

                cell.GetComponent<GridCell>().gridPos = new Vector2Int(x, y);

            }
        }

        // 產生玩家球
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            Vector3 spawnPos = new Vector3(spawnPositions[i].x * cellSpacing, yOffset, spawnPositions[i].z * cellSpacing);
            GameObject player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
            player.name = $"Player_{i + 1}";

            Material personalMat = new Material(inactiveMaterial);
            player.GetComponent<Renderer>().material = personalMat;

            playerObjects[i] = player;
        }

        FindObjectOfType<TurnManager>().SetPlayerObjects(playerObjects);
    }
   

}
