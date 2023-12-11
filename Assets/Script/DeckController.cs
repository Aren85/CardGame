using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckController : MonoBehaviour
{
    public static DeckController instance;
    public List<CardScriptableObject> deckToUse = new List<CardScriptableObject>();
    private List<CardScriptableObject> activeCards = new List<CardScriptableObject>();
    public CardBase cardToSpawn;
    private HandController handController;
    private void Awake()
    {
        instance = this;
        handController = FindObjectOfType<HandController>();
    }

    void Start()
    {
        SetupDeck();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            DrawCardToHand();
        }
    }

    public void SetupDeck()
    {
        // 清空活動卡牌列表
        activeCards.Clear();

        // 創建臨時牌組，並將指定牌組的卡牌加入其中
        List<CardScriptableObject> tempDeck = new List<CardScriptableObject>();
        tempDeck.AddRange(deckToUse);

        // 迭代次數計數器
        int iterations = 0;

        // 當臨時牌組不為空且迭代次數未超過300時進行選牌處理
        while (tempDeck.Count > 0 && iterations < 300)
        {
            // 選擇一張隨機卡牌
            int selected = Random.Range(0, tempDeck.Count);

            // 將選擇的卡牌加入活動卡牌列表，並在臨時牌組中移除該卡牌
            activeCards.Add(tempDeck[selected]);
            tempDeck.RemoveAt(selected);

            // 迭代次數加一
            iterations++;
        }
    }

    public void DrawCardToHand()
    {
        // 如果活動卡牌列表為空，重新設定牌組
        if (activeCards.Count == 0)
        {
            SetupDeck();
        }

        // 實例化新卡牌，將其設置在手牌控制器的位置
        CardBase newCard = Instantiate(cardToSpawn, this.transform);

        // 將新卡牌的卡牌腳本對象設置為活動卡牌列表的第一張卡牌
        newCard.cardSO = activeCards[0];

        // 設置新卡牌的屬性
        newCard. SetupCard();

        // 從活動卡牌列表中移除第一張卡牌
        activeCards.RemoveAt(0);

        // 將新卡牌添加到手牌控制器的手牌中
        HandController.instance.AddCardToHand(newCard);
    }
}
