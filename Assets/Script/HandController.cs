using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public static HandController instance;

    public List<Card> heldCards = new List<Card>();
    private RectTransform rectTransform;
    public Transform minPos, maxPos;
    public List<Vector3> cardPositions = new List<Vector3>();
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        rectTransform = GetComponent<RectTransform>(); //獲取RectTransform組件
        CollectCards();
    }
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        SetCardPostionsInHand();
    }
    void CollectCards()
    {
        // 讀取所有子物件
        foreach (Transform child in transform)
        {
            // 檢查子物件是否帶有 "Card" 腳本
            Card cardScript = child.GetComponent<Card>();

            if (cardScript != null)
            {
                heldCards.Add(cardScript);
            }
        }

        // 在這裡你可以對收集到的 cardList 做其他操作，例如排序或其他處理
    }
    public void SetCardPostionsInHand()
    {
        cardPositions.Clear();
        Vector3 distanceBetweenPoints = Vector3.zero;  //點之間的距離
        if (heldCards.Count > 1)
        {
            distanceBetweenPoints = (maxPos.position - minPos.position) / (heldCards.Count - 1);
        }

        for (int i = 0; i < heldCards.Count; i++)
        {
            cardPositions.Add(minPos.position + (distanceBetweenPoints * i));

            //heldCards[i].transform.position = cardPositions[i];



            heldCards[i].MoveToPoint(cardPositions[i]);//卡牌移動到對應位置
            heldCards[i].inHand = true;
            heldCards[i].handPosttion = i;
        }
    }

    public void AddCardToHand(Card cardToAdd)
    {
        heldCards.Add(cardToAdd);
        SetCardPostionsInHand();
    }
}
