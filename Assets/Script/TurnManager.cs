using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;



public class TurnManager : MonoBehaviour
{
    public MainPiece mainPiece;
    public int currentPlayer = 0;
    public int totalPlayers = 4;

    private bool isTurnInProgress = false;

    public int[] playerScores = new int[4];
    public TextMeshProUGUI[] scoreTexts;
    public TextMeshProUGUI turnText;

    public Material activeMaterial;
    public Material inactiveMaterial;

    private GameObject[] playerObjects;
    private List<SkillCard> cardPool;
    private List<SkillCard>[] playerHands = new List<SkillCard>[4];

    void Start()
    {
        UpdateScoreUI();
        UpdateTurnUI();
        cardPool = CardDatabase.GetAllCards();

        for (int i = 0; i < totalPlayers; i++)
            playerHands[i] = new List<SkillCard>();
    }

    void Update()
    {
        if (isTurnInProgress) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isTurnInProgress = true;

            int directionRoll = Random.Range(1, 9);
            int[] customDice = { 1, 1, 2, 2, 3, 3 };
            int stepsRoll = customDice[Random.Range(0, customDice.Length)];

            Vector2Int dir = GetDirectionVector(directionRoll);
            string dirName = GetDirectionName(directionRoll);

            Debug.Log($"🎲 玩家 {currentPlayer + 1} 擲出方向 {dirName}、步數 {stepsRoll}");

            FindObjectOfType<DiceResultPanel>().ShowResult(
                dirName,
                stepsRoll,
                3f, // 倒數秒數
                () => {
                    Vector2Int finalDir = mainPiece.ApplyMirror(dir); // ✅ 加入鏡像支援
            mainPiece.Move(finalDir, stepsRoll);
                    EndTurn();
                }
            );
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Vector2Int center = mainPiece.GetGridPos();
            Vector2Int goal = GetPlayerGoal(currentPlayer);

            for (int i = 1; i <= 8; i++)
            {
                if (center + GetDirectionVector(i) == goal)
                {
                    Debug.Log("🚫 不能用自由步直接得分，請使用骰子！");
                    return;
                }
            }

            isTurnInProgress = true;
            FindObjectOfType<DirectionUI>().Show();
            Debug.Log($"🎯 玩家 {currentPlayer + 1} 自由走一步（請點選方向）");
        }
    }

    void EndTurn()
    {
        CheckCorner(mainPiece.GetGridPos());
        mainPiece.TickBomb();

        currentPlayer = (currentPlayer + 1) % totalPlayers;
        UpdateTurnUI();
        UpdatePlayerHighlight();

        SkillCard template = cardPool[Random.Range(0, cardPool.Count)];
        SkillCard drawn = SkillCard.CloneCard(template);
        playerHands[currentPlayer].Add(drawn);

        FindObjectOfType<CardUIManager>().ShowCards(playerHands[currentPlayer]);
        isTurnInProgress = false;
    }

    void CheckCorner(Vector2Int pos)
    {
        Debug.Log($"📍 檢查座標：{pos}");

        if ((pos.x == 0 || pos.x == 6) && (pos.y == 0 || pos.y == 6))
        {
            int scoringPlayer = GetPlayerIndexByGoal(pos);

            if (scoringPlayer != -1)
            {
                int damage = mainPiece.ConsumeBomb();

                if (damage > 0)
                {
                    playerScores[scoringPlayer] -= damage;
                    Debug.Log($"💥 玩家 {scoringPlayer + 1} 被炸！扣 {damage} 分！");
                    if(playerScores[scoringPlayer]< 0){
                        playerScores[scoringPlayer] = 0;
                    }
                }
                else
                {
                    playerScores[scoringPlayer]++;
                    Debug.Log($"🏆 玩家 {scoringPlayer + 1} 成功得分！");
                }

                UpdateScoreUI();
                mainPiece.ResetToCenter();
            }
        }
    }

    int GetPlayerIndexByGoal(Vector2Int pos)
    {
        for (int i = 0; i < totalPlayers; i++)
        {
            if (GetPlayerGoal(i) == pos)
                return i;
        }
        return -1;
    }

    public void UpdateScoreUI()
    {
        for (int i = 0; i < scoreTexts.Length; i++)
            scoreTexts[i].text = $"P{i + 1}: {playerScores[i]}";
    }

    public void UpdateTurnUI()
    {
        turnText.text = $"P{currentPlayer + 1} Turn";
    }

    public Vector2Int GetDirectionVector(int dir)
    {
        return dir switch
        {
            1 => Vector2Int.up,
            2 => new Vector2Int(1, 1),
            3 => Vector2Int.right,
            4 => new Vector2Int(1, -1),
            5 => Vector2Int.down,
            6 => new Vector2Int(-1, -1),
            7 => Vector2Int.left,
            8 => new Vector2Int(-1, 1),
            _ => Vector2Int.zero,
        };
    }

    Vector2Int GetPlayerGoal(int playerIndex)
    {
        return playerIndex switch
        {
            0 => new Vector2Int(0, 0),
            1 => new Vector2Int(0, 6),
            2 => new Vector2Int(6, 0),
            3 => new Vector2Int(6, 6),
            _ => Vector2Int.zero
        };
    }

    public void FreeStep(Vector2Int dir)
    {
        mainPiece.MoveOneStep(dir);
        Debug.Log($"🎯 玩家 {currentPlayer + 1} 自由走一步（方向 {dir}）");
        EndTurn();
    }

    public void SetPlayerObjects(GameObject[] objs)
    {
        playerObjects = objs;
        UpdatePlayerHighlight();
    }

    void UpdatePlayerHighlight()
    {
        if (playerObjects == null) return;

        for (int i = 0; i < playerObjects.Length; i++)
        {
            Renderer rend = playerObjects[i].GetComponent<Renderer>();
            rend.material.color = (i == currentPlayer)
                ? activeMaterial.color
                : inactiveMaterial.color;

            rend.material.EnableKeyword("_EMISSION");
            rend.material.SetColor("_EmissionColor", (i == currentPlayer)
                ? activeMaterial.GetColor("_EmissionColor")
                : inactiveMaterial.GetColor("_EmissionColor"));
        }
    }

    public void RemoveCardFromHand(SkillCard card)
    {
        int targetId = card.cardId;
        playerHands[currentPlayer].RemoveAll(c => c.cardId == targetId);
        Debug.Log($"🧹 從 P{currentPlayer + 1} 的手牌移除卡片：{card.cardName} (id={targetId})");

        FindObjectOfType<CardUIManager>().ShowCards(playerHands[currentPlayer]);
    }

    public void UseCard(SkillCard card)
    {
        Debug.Log($"✅ Used Card: {card.cardName} - {card.description}");

        if (card.cardName == "炸彈")
        {
            mainPiece.AddBomb();
            Debug.Log("💣 技能效果：棋子變成炸彈狀態！");
        }
        else if (card.cardName == "鏡像魔法")
        {
            mainPiece.ActivateMirror();
            Debug.Log("🪞 鏡像魔法已啟動，下一步移動將被反轉！");
        }
        else if (card.cardName == "銅牆鐵壁")
        {
            StartCoroutine(EnterWallPlacementMode(card)); // ✅ 進入放置牆模式
        }

        RemoveCardFromHand(card);
    }

    public string GetDirectionName(int dir)
    {
        return dir switch
        {
            1 => "↑",
            2 => "↗",
            3 => "→",
            4 => "↘",
            5 => "↓",
            6 => "↙",
            7 => "←",
            8 => "↖",
            _ => "未知"
        };
    }

    private IEnumerator
        EnterWallPlacementMode(SkillCard card)
    {
        Debug.Log("🧱 銅牆鐵壁模式啟動，請點選要放牆的位置...");

        bool placed = false;
        Vector2Int targetPos = Vector2Int.zero;

        // 👉 建立暫時監聽點擊
        while (!placed)
        {
            if (Input.GetMouseButtonDown(0)) // 左鍵
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    GridCell cell = hit.collider.GetComponent<GridCell>();
                    if (cell != null && !cell.HasWall)
                    {
                        targetPos = cell.gridPos;
                        WallManager.Instance.PlaceWallAt(targetPos);
                        cell.HasWall = true;
                        placed = true;
                    }
                }
            }

            yield return null;
        }

        RemoveCardFromHand(card);
    }
    public Vector2Int GetPlayerGridPos(int index)
    {
        return BoardUtility.WorldToGrid(playerObjects[index].transform.position);
    }


}

