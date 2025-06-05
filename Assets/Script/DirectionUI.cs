using UnityEngine;

public class DirectionUI : MonoBehaviour
{
    public GameObject scorePanel; // 👈 拖入 ScorePanel UI


    public GameObject panel; // 指向 DirectionPanel 本體
    private TurnManager turnManager;

    private void Start()
    {
        panel.SetActive(false); // 一開始先隱藏
        turnManager = FindObjectOfType<TurnManager>(); // 找 TurnManager 連動
    }

    public void Show()
    {
        panel.SetActive(true);

        if (scorePanel != null)
            scorePanel.SetActive(false); // ✅ 隱藏分數 UI
    }

    public void Hide()
    {
        panel.SetActive(false);

        if (scorePanel != null)
            scorePanel.SetActive(true); // ✅ 顯示回分數 UI
    }


    // 被按鈕呼叫，輸入 1~8 對應八方向
    public void OnDirectionSelected(int dir)
    {
        Vector2Int direction = turnManager.GetDirectionVector(dir);
        turnManager.FreeStep(direction); // 呼叫 TurnManager 執行移動
        Hide();
    }
}
