using config;
using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.UI;

public class HandController : MonoBehaviour
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

    //[SerializeField]
    //private CardPlayConfig cardPlayConfig; // 卡片遊玩配置

    // 用於保存 CardWrapper 實例的列表
    private List<CardWrapper> cards = new();

    // 對 RectTransform 組件的引用
    private RectTransform rectTransform;

    // 當前被拖曳的卡片
    private CardWrapper currentDraggedCard;
    void Start()
    {
        //SetCardPostionsInHand();
        //GenerateArcHand();
        rectTransform = GetComponent<RectTransform>();

    }
    private void InitCards()
    {
        // 設置卡片及其錨點
        SetUpCards();
        SetCardsAnchor();
    }
    void Update()
    {
        UpdateCards();
    }
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
            wrapper.handController = this;
        }
    }
    private void SetCardsAnchor()
    {
        foreach (CardWrapper child in cards)
        {
            child.SetAnchor(new Vector2(0, 0.5f), new Vector2(0, 0.5f));
        }
    }

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
    private float GetCardRotation(int index)
    {
        if (cards.Count < 3) return 0;
        // 基於卡片列表中的索引關聯旋轉
        // 這樣第一和最後的卡片旋轉最大，以中心為軸對稱
        return -maxCardRotation * (index - (cards.Count - 1) / 2f) / ((cards.Count - 1) / 2f);
    }

    private void SetCardsUILayers() // 設置卡片的 UI 層級
    {
        for (var i = 0; i < cards.Count; i++)
        {
            cards[i].uiLayer = zoomConfig.defaultSortOrder + i;
        }
    }
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
    // Update is called once per frame



    //public void SetCardPostionsInHand()
    //{
    //    cardPositions.Clear(); // 清除卡牌位置列表

    //    Vector3 distanceBetweenPoints = Vector3.zero;  // 定義一個 Vector3 變數，表示點之間的距離


    //    if (heldCards.Count > 1)
    //    {
    //        // 計算點之間的距離，以便將卡牌均勻分佈在最小和最大位置之間
    //        distanceBetweenPoints = (maxPos.position - minPos.position) / (heldCards.Count - 1);
    //    }

    //    for (int i = 0; i < heldCards.Count; i++)
    //    {
    //        //Vector3 v3 = Vector3.Slerp(minPos.position, maxPos.position, i * (1f/heldCards.Count));
    //        //Vector3 cardPosition = minPos.position + (distanceBetweenPoints * i);

    //        float t = Mathf.Pow(i / (float)(heldCards.Count - 1), curveTValue); // 使用Mathf.Pow調整曲線形狀
    //        Vector3 cardPosition = Vector3.Slerp(minPos.position, maxPos.position, t);
    //        // 計算每張卡牌的位置並添加到卡牌位置列表中
    //        //cardPositions.Add(minPos.position + (distanceBetweenPoints * i));
    //        cardPositions.Add(cardPosition);

    //        // 設定卡牌的移動目標位置
    //        //heldCards[i].MoveToPoint(cardPositions[i]);

    //        // 標記卡牌為手牌中的卡牌
    //        heldCards[i].inHand = true;

    //        // 設定卡牌在手牌中的位置
    //        heldCards[i].handPosttion = i;
    //    }




    //void GenerateArcHand()
    //{
    //    for (int i = 0; i < heldCards.Count; i++)
    //    {
    //        // 在0到1之间插值，用于计算手牌位置的比例
    //        float t = i / (float)(heldCards.Count - 1);
    //        // 使用Vector3.Lerp在最小和最大位置之间插值得到当前位置
    //        Vector3 lerpedPos = Vector3.Lerp(minPos.position, maxPos.position, t);
    //        // 计算弧形位置
    //        Vector3 arcPos = CalculateArcPosition(lerpedPos, center.position, t);

    //        // 将计算得到的位置添加到列表中
    //        cardPositions.Add(arcPos);

    //        // 将手牌移动到计算得到的位置
    //        heldCards[i].MoveToPoint(cardPositions[i]);
    //        // 将手牌标记为在手中
    //        heldCards[i].inHand = true;
    //        // 设置手牌在手中的位置
    //        heldCards[i].handPosttion = i;
    //    }
    //}

    //Vector3 CalculateArcPosition(Vector3 position, Vector3 center, float t)
    //{
    //    // 在0到π之间插值，用于计算弧形的角度
    //    float angle = Mathf.Lerp(0, Mathf.PI, t);
    //    // 计算弧形位置的x坐标
    //    float radius = Vector3.Distance(center, position);
    //    float x = center.x + radius * Mathf.Cos(angle);
    //    // 计算弧形位置的y坐标
    //    float y = center.y + radius * Mathf.Sin(angle);

    //    // 返回计算得到的弧形位置
    //    return new Vector3(x, y, position.z);
    //}
}
