using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDarg : MonoBehaviour,IPointerDownHandler,IDragHandler,IEndDragHandler
{
    private CardManager cardManager;
    private Card card;  // 添加這一行，用來保存 CardDarg 關聯的 Card
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public Canvas canvas;

    // 初始化 cardManager 和 card，這部分可能在你的代碼中有所不同
    private void Awake()
    {
        cardManager = FindObjectOfType<CardManager>();
        card = GetComponent<Card>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = FindObjectOfType<Canvas>();
        canvasGroup.blocksRaycasts = true;
    }
    void Start()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        card.isSelected = true;
        Debug.Log("OnPointerDown");
        cardManager.SelectCard(card);
        canvasGroup.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        // 當卡牌被拖曳時，更新卡牌的位置
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        card.isSelected = false;
        canvasGroup.blocksRaycasts = true;
        // 在這裡可能還需要檢查是否在有效的區域進行拖曳，這取決於你的需求
        PlacementController placementController = eventData.pointerEnter.GetComponent<PlacementController>();
        if (placementController != null)
        {
            cardManager.DropCardInZone(placementController);
        }
    }
}
