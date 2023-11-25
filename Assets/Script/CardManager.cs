using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private Card selectedCard;
    public PlacementController placementController;
    public void SelectCard(Card card)
    {
        // 選擇卡牌，可能執行一些相應的操作
        selectedCard = card;
    }

    public void UnselectCard()
    {
        // 取消選擇卡牌
        selectedCard = null;
    }

    public void DropCardInZone(PlacementController placementController)
    {
        if (selectedCard != null)
        {
            // 告訴 PlacementController 處理被拖曳的卡牌
            placementController.HandleDroppedCard(selectedCard);

            // 在這裡可以執行一些全局的卡牌被拖曳後的操作
            // 例如，更新其他UI元素，或者觸發其他事件

            // 將卡牌從手牌移除
            // 注意：這裡的 RemoveCard 需要在你的 CardManager 中實現
            RemoveCard(selectedCard);

            // 取消選擇卡牌
            UnselectCard();
        }
    }

    private void RemoveCard(Card card)
    {
        // 在這裡實現從手牌移除卡牌的邏輯
        // 這裡需要根據你的組織結構和需求進行具體實現
    }
}
