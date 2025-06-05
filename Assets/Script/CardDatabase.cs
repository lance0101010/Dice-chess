using System.Collections.Generic;
using UnityEngine;


public static class CardDatabase
{
    public static List<SkillCard> GetAllCards()
    {
        return new List<SkillCard>
        {
            new SkillCard("銅牆鐵壁",  "點擊選擇一格放置金屬牆（阻擋玩家通過）", false, new Color(1f, 0.4f, 0.4f)), // 紅色
            new SkillCard("炸彈", "棋子變陷阱", false, new Color(1f, 0.8f, 0.2f)),   // 黃色
            new SkillCard("鏡像魔法", "反方向移動", true, new Color(0.6f, 0.8f, 1f))  // 藍色

        };
    }
}
