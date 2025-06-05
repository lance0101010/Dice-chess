using UnityEngine;

[System.Serializable]
public class SkillCard
{
    public string cardName;
    public string description;
    public bool canUseOnOpponentTurn;
    public Color cardColor;
    public int cardId; // ✅ 自動遞增 ID

    private static int nextId = 1; // ✅ 靜態遞增編號，每張卡唯一

    public SkillCard(string name, string desc, bool canUseAnytime = false, Color? color = null)
    {
        cardName = name;
        description = desc;
        canUseOnOpponentTurn = canUseAnytime;
        cardColor = color ?? Color.white;
        cardId = nextId++; // ✅ 指派唯一 ID
    }

    public override string ToString()
    {
        return $"{cardName}: {description}";
    }

    public static SkillCard CloneCard(SkillCard original)
    {
        return new SkillCard(original.cardName, original.description, original.canUseOnOpponentTurn, original.cardColor);
    }
}
