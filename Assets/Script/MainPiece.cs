using UnityEngine;
using TMPro;

public class MainPiece : MonoBehaviour
{
    public float spacing = 1.1f;
    public float yOffset = 0.5f;

    private Vector2Int gridPos = new Vector2Int(3, 3); // 初始位置
   
    public GameObject bombEffectVisual;               // 指向特效（如閃爍的物件）

    public TextMeshProUGUI bombStatusText;

    public GameObject explosionPrefab;
    void Start()
    {
        UpdateWorldPosition();
  
    }

 public void Move(Vector2Int direction, int steps)
{
    Vector2Int nextPos = gridPos;

    for (int i = 0; i < steps; i++)
    {
        Vector2Int stepPos = nextPos + direction;

        // ✅ 檢查是否超出邊界
        if (!IsWithinBounds(stepPos))
        {
            Debug.Log("🚧 移動超出邊界，停止！");
            break;
        }

            // ✅ 檢查是否被牆擋住
            bool blocked = false;

            // 若是對角線移動，檢查相鄰兩個直方向
            if (Mathf.Abs(direction.x) == 1 && Mathf.Abs(direction.y) == 1)
            {
                Vector2Int horizontal = nextPos + new Vector2Int(direction.x, 0);
                Vector2Int vertical = nextPos + new Vector2Int(0, direction.y);

                if (BoardUtility.HasWallAt(horizontal) || BoardUtility.HasWallAt(vertical))
                    blocked = true;
            }
            else
            {
                // 一般上下左右的移動，照舊
                Vector2Int target = nextPos + direction;
                if (BoardUtility.HasWallAt(target))
                    blocked = true;
            }

            if (blocked)
            {
                Debug.Log($"🧱 前方有牆，無法往 {direction} 移動！");
                break;
            }


            nextPos = stepPos;
    }

    gridPos = nextPos;
    UpdateWorldPosition();
}


  public void MoveOneStep(Vector2Int direction)
{
    Vector2Int nextPos = gridPos + direction;

    if (!IsWithinBounds(nextPos))
    {
        Debug.Log("🚧 自由步：超出邊界！");
        return;
    }

    // ✅ 強化：對角線不能穿過直牆
    bool blocked = false;

    if (Mathf.Abs(direction.x) == 1 && Mathf.Abs(direction.y) == 1)
    {
        Vector2Int horizontal = gridPos + new Vector2Int(direction.x, 0);
        Vector2Int vertical = gridPos + new Vector2Int(0, direction.y);

        if (BoardUtility.HasWallAt(horizontal) || BoardUtility.HasWallAt(vertical))
            blocked = true;
    }
    else
    {
        if (BoardUtility.HasWallAt(nextPos))
            blocked = true;
    }

    if (blocked)
    {
        Debug.Log($"🧱 自由步擋住：無法往 {direction} 方向前進！");
        return;
    }

    gridPos = nextPos;
    UpdateWorldPosition();
}



    public Vector2Int GetGridPos()
    {
        return gridPos;
    }

    public void ResetToCenter()
    {
        gridPos = new Vector2Int(3, 3);
        UpdateWorldPosition();
    }

    private void UpdateWorldPosition()
    {
        transform.position = new Vector3(gridPos.x * spacing, yOffset, gridPos.y * spacing);
        Debug.Log($"🎯 棋子位置更新 → 邏輯：{gridPos}，實體：{transform.position}");
    }

    private bool IsWithinBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x <= 6 && pos.y >= 0 && pos.y <= 6;
    }


    // 🔥 新增欄位
    private int bombTimer = 0;
    private int bombDamage = 0;
    public void AddBomb()
    {      

        if (bombTimer > 0)
        {
            bombDamage++;
            bombTimer = 5; // ✅ 回合重置
            Debug.Log($"💣 炸彈強化！現在傷害：{bombDamage}，剩餘回合：{bombTimer}");
        }
        else
        {
            bombDamage = 1;
            bombTimer = 5;
            Debug.Log($"💣 新炸彈設置！傷害：{bombDamage}，回合：{bombTimer}");
        }

        if (bombEffectVisual != null)
            bombEffectVisual.SetActive(true);
        UpdateBombUI();
    }

    public void TickBomb()
    {
        if (bombTimer > 0)
        {
            bombTimer--;
            Debug.Log($"⏳ 炸彈倒數中… 剩餘回合：{bombTimer}");
            UpdateBombUI();

            if (bombTimer == 0)
            {
                Debug.Log("💥 炸彈時間到！爆炸範圍開始處理...");
                ExplodeAround();
                bombDamage = 0;

                if (bombEffectVisual != null)
                    bombEffectVisual.SetActive(false);

                UpdateBombUI();
            }
        }
    }

    private void ExplodeAround()
    {
        Vector2Int center = gridPos;

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                Vector2Int pos = new Vector2Int(center.x + dx, center.y + dy);

                if (!IsWithinBounds(pos)) continue;

                // ✅ 摧毀牆
                if (BoardUtility.HasWallAt(pos))
                {
                    GameObject wall = BoardUtility.GetWallAt(pos);
                    if (wall != null)
                    {
                        Debug.Log($"💣 爆炸摧毀牆體：{pos}");
                        BoardUtility.RemoveWall(pos);
                        Destroy(wall);
                    }
                }

                // ✅ 檢查玩家位置（會傷害所有在爆炸區內的玩家）
                // ✅ 檢查玩家位置（會傷害所有在爆炸區內的玩家）
                foreach (var tm in FindObjectsOfType<TurnManager>())
                {
                    for (int i = 0; i < tm.totalPlayers; i++)
                    {
                        Vector2Int playerPos = tm.GetPlayerGridPos(i);
                        if (playerPos == pos)
                        {
                            tm.playerScores[i] -= bombDamage;
                            if (tm.playerScores[i] < 0) tm.playerScores[i] = 0;
                            Debug.Log($"🔥 玩家 {i + 1} 被爆炸波及，扣 {bombDamage} 分！");
                            tm.UpdateScoreUI();
                        }
                    }
                }
                if (explosionPrefab != null)
                {
                    Debug.Log("嘗試播放爆炸動畫");
                    GameObject fx = Instantiate(
                        explosionPrefab,
                        transform.position + Vector3.up * 0.5f,
                        Quaternion.Euler(-90, 0, 0)
                    );
                    Destroy(fx, 2f);
                }

            }
        }
    }




    public int ConsumeBomb()
    {
        UpdateBombUI();
        int dmg = bombDamage;

        if (bombDamage > 0)
        {
            bombTimer = 0;
            bombDamage = 0;

            if (bombEffectVisual != null)
                bombEffectVisual.SetActive(false);

            Debug.Log($"💥 引爆炸彈！造成 {dmg} 點傷害！");

            if (explosionPrefab != null)
            {
                Debug.Log("嘗試播放爆炸動畫");
                GameObject fx = Instantiate(
                    explosionPrefab,
                    transform.position + Vector3.up * 0.5f,
                    Quaternion.Euler(-90, 0, 0)
                );
                Destroy(fx, 2f);
            }
        }

        return dmg;
    }


    public bool IsDangerous()
    {
        return bombTimer > 0;
    }

    private void UpdateBombUI()
    {
        if (bombStatusText != null)
        {
            if (bombTimer > 0)
            {
                bombStatusText.text = $"炸彈：{bombDamage}傷害 / {bombTimer}回合";
            }
            else
            {
                bombStatusText.text = "";
            }
        }
    }

    private bool mirrorActive = false;
    public Vector2Int ApplyMirror(Vector2Int inputDir)
    {
        // 如果未啟動鏡像魔法，就原樣返回
        if (!mirrorActive) return inputDir;

        // 反轉方向
        Vector2Int mirrored = new Vector2Int(-inputDir.x, -inputDir.y);
        Debug.Log($"🪞 鏡像魔法發動！原方向：{inputDir} → 鏡像方向：{mirrored}");

        mirrorActive = false; // 用完就失效
        return mirrored;
    }

    public void ActivateMirror()
    {
        mirrorActive = true;
        Debug.Log("🪞 鏡像魔法準備就緒，下一次移動將反方向！");
    }





}
