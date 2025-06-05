using UnityEngine;

public class DiceController : MonoBehaviour
{
    // �ۭq�� 6 ����]�C�ӼƦr�X�{�⦸�^
    private int[] customStepDice = { 1, 1, 2, 2, 3, 3 };

    /// <summary>
    /// �Y��V�]1~8�^
    /// </summary>
    public int RollDirection()
    {
        return Random.Range(1, 9); // �]�t 8
    }

    /// <summary>
    /// �Y�B�ơ]�q [1,1,2,2,3,3] ����^
    /// </summary>
    public int RollSteps()
    {
        int index = Random.Range(0, customStepDice.Length);
        return customStepDice[index];
    }
}
