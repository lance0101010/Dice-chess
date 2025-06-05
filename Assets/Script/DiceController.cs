using UnityEngine;

public class DiceController : MonoBehaviour
{
    // 自訂的 6 面骰（每個數字出現兩次）
    private int[] customStepDice = { 1, 1, 2, 2, 3, 3 };

    /// <summary>
    /// 擲方向（1~8）
    /// </summary>
    public int RollDirection()
    {
        return Random.Range(1, 9); // 包含 8
    }

    /// <summary>
    /// 擲步數（從 [1,1,2,2,3,3] 中選）
    /// </summary>
    public int RollSteps()
    {
        int index = Random.Range(0, customStepDice.Length);
        return customStepDice[index];
    }
}
