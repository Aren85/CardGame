using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDarg : MonoBehaviour,IPointerDownHandler,IDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler,IPointerUpHandler
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
    public void OnPointerEnter(PointerEventData eventData)
    {
        card.isSelected = true;
        rectTransform.localScale = Vector3.one * 1.3f;
        Vector3.Slerp(rectTransform.localPosition, rectTransform.localPosition += new Vector3(0f, 50f, 0f), 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        card.isSelected = false;
        rectTransform.localScale = Vector3.one;
        rectTransform.localPosition -= new Vector3(0f, 2f, 0f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        card.isSelected = true;
        //rectTransform.anchoredPosition = eventData.position;
        Debug.Log("OnPointerDown");
        cardManager.SelectCard(card);
        canvasGroup.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        card.isSelected = true;
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



    public void OnPointerUp(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        HandController.instance.SetCardPostionsInHand();
    }
}
