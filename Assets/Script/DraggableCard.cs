using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public SkillCard cardData; // 這張卡的內容
    private Transform originalParent;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Image img = GetComponent<Image>();
        if (img != null && cardData != null)
        {
            img.color = cardData.cardColor;  // ✅ 把卡片背景設成指定顏色
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root); // 拉到畫面最上層
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        canvasGroup.blocksRaycasts = true;
    }
    public void Init(SkillCard data)
    {
        cardData = data;

        Image img = GetComponent<Image>();
        if (img != null)
        {
            img.color = cardData.cardColor;
        }
    }

}
