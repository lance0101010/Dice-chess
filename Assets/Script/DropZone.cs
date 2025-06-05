using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped != null)
        {
            DraggableCard cardUI = dropped.GetComponent<DraggableCard>();
            if (cardUI != null)
            {
                SkillCard card = cardUI.cardData;

                Debug.Log($"[DropZone] 🔥 使用的卡片 GameObject 名稱：{dropped.name}");
                Debug.Log($"[DropZone] 📦 對應 cardData：{card.cardName} / {card.description}");
                Debug.Log($"[使用] {card.cardName}, id = {card.GetHashCode()}");


                // 觸發卡片技能邏輯
                TurnManager tm = FindObjectOfType<TurnManager>();
                tm.UseCard(card);
                tm.RemoveCardFromHand(card);
                Debug.Log($"cardData 是否為 null？{cardUI.cardData == null}");
                Debug.Log($"cardName: {cardUI.cardData?.cardName}");
                GameObject.Destroy(dropped);

            }
        }
    }
}
