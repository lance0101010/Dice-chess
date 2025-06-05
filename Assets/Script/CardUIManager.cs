using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardUIManager : MonoBehaviour
{
    public GameObject cardPrefab;   // 改成 draggable 預製體
    public Transform cardPanel;     // 卡片要被生成在哪裡

    public void ShowCards(List<SkillCard> hand)
    {
        foreach (Transform child in cardPanel)
        {
            DraggableCard dc = child.GetComponent<DraggableCard>();
            if (dc != null)
            {
                Debug.Log($"👋 銷毀卡片：{dc.cardData.cardName}, id = {dc.cardData.GetHashCode()}");
            }
            Destroy(child.gameObject);
        }


        // 生成每一張手牌卡片
        foreach (SkillCard card in hand)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardPanel);
            TextMeshProUGUI text = cardObj.GetComponentInChildren<TextMeshProUGUI>();
            text.text = $"{card.cardName}\n<size=70%>{card.description}</size>";

            DraggableCard draggable = cardObj.GetComponent<DraggableCard>();
            if (draggable != null)
            {
                draggable.Init(card); // ✅ 正確初始化並設定顏色
            }
            Debug.Log($"[UI] 🎴 產生卡片 UI：{card.cardName} / {card.description}");

        }
    }

    public void HideCards()
    {
        foreach (Transform child in cardPanel)
        {
            Destroy(child.gameObject);
        }
    }
}
