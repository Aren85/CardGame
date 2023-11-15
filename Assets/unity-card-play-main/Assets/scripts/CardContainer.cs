using System.Collections.Generic;
using System.Linq;
using config;
using DefaultNamespace;
using events;
using UnityEngine;
using UnityEngine.UI;

public class CardContainer : MonoBehaviour
{
    // 約束條件部分
    [Header("約束條件")]
    [SerializeField]
    private bool forceFitContainer; // 是否強制容器大小

    // 對齊部分
    [Header("對齊")]
    [SerializeField]
    private CardAlignment alignment = CardAlignment.Center; // 卡片對齊方式

    [SerializeField]
    private bool allowCardRepositioning = true; // 是否允許重新定位卡片

    // 旋轉部分
    [Header("旋轉")]
    [SerializeField]
    [Range(0f, 90f)]
    private float maxCardRotation; // 最大卡片旋轉角度

    [SerializeField]
    private float maxHeightDisplacement; // 最大垂直位移

    [SerializeField]
    private ZoomConfig zoomConfig; // 縮放配置

    [SerializeField]
    private AnimationSpeedConfig animationSpeedConfig; // 動畫速度配置

    [SerializeField]
    private CardPlayConfig cardPlayConfig; // 卡片遊玩配置

    // 事件部分
    [Header("事件")]
    [SerializeField]
    private EventsConfig eventsConfig; // 事件配置

    // 用於保存 CardWrapper 實例的列表
    private List<CardWrapper> cards = new();

    // 對 RectTransform 組件的引用
    private RectTransform rectTransform;

    // 當前被拖曳的卡片
    private CardWrapper currentDraggedCard;

    // Start 方法在第一個幀之前被調用
    private void Start()
    {
        // 獲取附加到此 GameObject 的 RectTransform 組件
        rectTransform = GetComponent<RectTransform>();
        // 初始化卡片
        InitCards();
    }

    // 初始化卡片
    private void InitCards()
    {
        // 設置卡片及其錨點
        SetUpCards();
        SetCardsAnchor();
    }

    // 根據索引設置每張卡片的旋轉
    private void SetCardsRotation()
    {
        // 遍歷卡片列表
        for (var i = 0; i < cards.Count; i++)
        {
            // 將卡片的目標旋轉設置為 GetCardRotation 方法的返回值
            cards[i].targetRotation = GetCardRotation(i);

            // 將卡片的目標垂直位移設置為 GetCardVerticalDisplacement 方法的返回值
            cards[i].targetVerticalDisplacement = GetCardVerticalDisplacement(i);
        }
    }

    // 根據索引獲取卡片的垂直位移
    private float GetCardVerticalDisplacement(int index)
    {
        if (cards.Count < 3) return 0; // 如果卡片數量小於3，直接返回0（不進行垂直位移）
        // 基於卡片列表中的索引關聯垂直位移
        // 這樣中心卡片的位移最大，而邊緣卡片的位移為0
        return maxHeightDisplacement *
               (1 - Mathf.Pow(index - (cards.Count - 1) / 2f, 2) / Mathf.Pow((cards.Count - 1) / 2f, 2));
    }

    // 根據索引獲取卡片的旋轉
    private float GetCardRotation(int index)
    {
        if (cards.Count < 3) return 0;
        // 基於卡片列表中的索引關聯旋轉
        // 這樣第一和最後的卡片旋轉最大，以中心為軸對稱
        return -maxCardRotation * (index - (cards.Count - 1) / 2f) / ((cards.Count - 1) / 2f);
    }

    // Update 方法在每一幀都被調用
    void Update()
    {
        // 更新卡片
        UpdateCards();
    }

    // 設置卡片
    void SetUpCards()//這個函數的作用是確保每個子卡片都有一個 CardWrapper 組件，並且這些卡片的相關配置信息被正確地設置。
    {
        cards.Clear();
        foreach (Transform card in transform)
        {
            var wrapper = card.GetComponent<CardWrapper>();
            if (wrapper == null)
            {
                wrapper = card.gameObject.AddComponent<CardWrapper>();
            }

            cards.Add(wrapper);

            AddOtherComponentsIfNeeded(wrapper);

            // 將額外的配置傳遞給子卡片
            wrapper.zoomConfig = zoomConfig;
            wrapper.animationSpeedConfig = animationSpeedConfig;
            wrapper.eventsConfig = eventsConfig;
            wrapper.container = this;
        }
    }

    // 如果需要，添加其他組件
    //這個方法確保每個卡片都擁有一個 Canvas 組件，並進行了特定的配置，同時確保卡片具有 GraphicRaycaster 組件，
    //以支持與圖形相關的射線事件。這可能是為了確保卡片在 Unity 遊戲中正確地渲染和處理事件。
    private void AddOtherComponentsIfNeeded(CardWrapper wrapper)//
    {
        var canvas = wrapper.GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = wrapper.gameObject.AddComponent<Canvas>();
        }

        canvas.overrideSorting = true;

        if (wrapper.GetComponent<GraphicRaycaster>() == null)
        {
            wrapper.gameObject.AddComponent<GraphicRaycaster>();
        }
    }

    // 更新卡片
    //這個方法是為了保持卡片的狀態和顯示是最新的，根據子物體的數量變化執行不同的初始化操作，然後更新卡片的位置、旋轉、UI 層級和渲染順序。
    private void UpdateCards()
    {
        if (transform.childCount != cards.Count)
        {
            InitCards();
        }

        if (cards.Count == 0)
        {
            return;
        }

        SetCardsPosition();
        SetCardsRotation();
        SetCardsUILayers();
        UpdateCardOrder();
    }

    private void SetCardsUILayers() // 設置卡片的 UI 層級
    {
        for (var i = 0; i < cards.Count; i++)
        {
            cards[i].uiLayer = zoomConfig.defaultSortOrder + i;
        }
    }

    // 更新卡片的順序
    private void UpdateCardOrder()// 更新卡片的順序
    {
        if (!allowCardRepositioning || currentDraggedCard == null) return;

        // 根據位置獲取被拖曳卡片的索引
        var newCardIdx = cards.Count(card => currentDraggedCard.transform.position.x > card.transform.position.x);
        var originalCardIdx = cards.IndexOf(currentDraggedCard);
        if (newCardIdx != originalCardIdx)
        {
            cards.RemoveAt(originalCardIdx);
            if (newCardIdx > originalCardIdx && newCardIdx < cards.Count - 1)
            {
                newCardIdx--;
            }

            cards.Insert(newCardIdx, currentDraggedCard);
        }
        // 在層次結構中重新排序
        currentDraggedCard.transform.SetSiblingIndex(newCardIdx);
    }

    // 設置卡片的位置
    private void SetCardsPosition()
    {
        // 計算全局空間中所有卡片的總寬度
        var cardsTotalWidth = cards.Sum(card => card.width * card.transform.lossyScale.x);
        // 計算容器的全局寬度
        var containerWidth = rectTransform.rect.width * transform.lossyScale.x;
        if (forceFitContainer && cardsTotalWidth > containerWidth)
        {
            DistributeChildrenToFitContainer(cardsTotalWidth);
        }
        else
        {
            DistributeChildrenWithoutOverlap(cardsTotalWidth);
        }
    }

    // 將子卡片均勻分佈以適應容器
    private void DistributeChildrenToFitContainer(float childrenTotalWidth)
    {
        // 獲取容器的寬度
        var width = rectTransform.rect.width * transform.lossyScale.x;
        // 獲取每個子項之間的距離
        var distanceBetweenChildren = (width - childrenTotalWidth) / (cards.Count - 1);
        // 將所有子卡片的位置設置為均勻分佈
        var currentX = transform.position.x - width / 2;
        foreach (CardWrapper child in cards)
        {
            var adjustedChildWidth = child.width * child.transform.lossyScale.x;
            child.targetPosition = new Vector2(currentX + adjustedChildWidth / 2, transform.position.y);
            currentX += adjustedChildWidth + distanceBetweenChildren;
        }
    }

    // 將子卡片均勻分佈而不重疊
    private void DistributeChildrenWithoutOverlap(float childrenTotalWidth)
    {
        var currentPosition = GetAnchorPositionByAlignment(childrenTotalWidth);
        foreach (CardWrapper child in cards)
        {
            var adjustedChildWidth = child.width * child.transform.lossyScale.x;
            child.targetPosition = new Vector2(currentPosition + adjustedChildWidth / 2, transform.position.y);
            currentPosition += adjustedChildWidth;
        }
    }

    // 根據對齊方式和寬度獲取錨點位置
    private float GetAnchorPositionByAlignment(float childrenWidth)
    {
        var containerWidthInGlobalSpace = rectTransform.rect.width * transform.lossyScale.x;
        switch (alignment)
        {
            case CardAlignment.Left:
                return transform.position.x - containerWidthInGlobalSpace / 2;
            case CardAlignment.Center:
                return transform.position.x - childrenWidth / 2;
            case CardAlignment.Right:
                return transform.position.x + containerWidthInGlobalSpace / 2 - childrenWidth;
            default:
                return 0;
        }
    }

    // 設置卡片的錨點
    private void SetCardsAnchor()
    {
        foreach (CardWrapper child in cards)
        {
            child.SetAnchor(new Vector2(0, 0.5f), new Vector2(0, 0.5f));
        }
    }

    // 當卡片拖曳開始時調用
    public void OnCardDragStart(CardWrapper card)
    {
        currentDraggedCard = card;
    }

    // 當卡片拖曳結束時調用
    public void OnCardDragEnd()
    {
        // 如果卡片在遊玩區域內，進行遊玩操作
        if (IsCursorInPlayArea())
        {
            eventsConfig?.OnCardPlayed?.Invoke(new CardPlayed(currentDraggedCard));
            if (cardPlayConfig.destroyOnPlay)
            {
                DestroyCard(currentDraggedCard);
            }
        }
        currentDraggedCard = null;
    }

    // 銷毀卡片
    public void DestroyCard(CardWrapper card)
    {
        cards.Remove(card);
        eventsConfig.OnCardDestroy?.Invoke(new CardDestroy(card));
        Destroy(card.gameObject);
    }

    // 檢查光標是否在遊玩區域內
    private bool IsCursorInPlayArea()
    {
        if (cardPlayConfig.playArea == null) return false;

        var cursorPosition = Input.mousePosition;
        var playArea = cardPlayConfig.playArea;
        var playAreaCorners = new Vector3[4];
        playArea.GetWorldCorners(playAreaCorners);
        return cursorPosition.x > playAreaCorners[0].x &&
               cursorPosition.x < playAreaCorners[2].x &&
               cursorPosition.y > playAreaCorners[0].y &&
               cursorPosition.y < playAreaCorners[2].y;

    }
}
