using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementController : MonoBehaviour
{
    public delegate void CardDroppedHandler(Card droppedCard);
    public event CardDroppedHandler OnCardDropped;

    // 這個方法用於處理被拖曳的卡牌
    public void HandleDroppedCard(Card droppedCard)
    {
        // 在這裡處理被拖曳的卡牌
        Debug.Log("Card Dropped in PlacementController!");

        // 可以執行其他相應的操作，例如將卡牌加入到放置區域的 List 中等
        // 這裡僅為示例，實際邏輯需要根據你的需求進行具體實現

        // 觸發事件通知其他物件卡牌已被拖曳
        OnCardDropped?.Invoke(droppedCard);
    }
}
