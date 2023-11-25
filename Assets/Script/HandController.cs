using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandController : MonoBehaviour, IPointerDownHandler
{
    public static HandController instance;

    public List<Card> heldCards = new List<Card>();
    private RectTransform rectTransform;
    public Transform minPos, maxPos;
    public List<Vector3> cardPositions = new List<Vector3>();

    public float fanAngle = 60f; // FAN 型的角度
    public float radius = 2f; // 卡牌在 FAN 中的半径
    public float slerpFactor = 0.5f; // Slerp 插值因子
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        rectTransform = GetComponent<RectTransform>(); //獲取RectTransform組件
        CollectCards();
        SetCardPostionsInHand();
    }
    void Start()
    {
        SetCardPostionsInHand();
        ArrangeCardsInFan();

    }
    void ArrangeCardsInFan()
    {
        int numberOfCards = heldCards.Count;

        for (int i = 0; i < numberOfCards; i++)
        {
            // 计算每个卡牌在 FAN 中的角度
            float angle = i * (fanAngle / (numberOfCards - 1)) - (fanAngle / 2f);

            // 将极坐标转换为笛卡尔坐标
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            // 计算目标旋转
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

            // 确保目标旋转是单位四元数
            if (targetRotation != Quaternion.identity)
            {
                targetRotation.Normalize();
            }

            // 使用 Slerp 插值旋转
            heldCards[i].transform.localRotation = Quaternion.Slerp(heldCards[i].transform.localRotation, targetRotation, slerpFactor);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
        ArrangeCardsInFan();
    }

    public void AddCardToHand(Card cardToAdd)
    {
        heldCards.Add(cardToAdd);
        SetCardPostionsInHand();
    }

    public void RemoveCardToHand(Card cardToAdd)
    {
        heldCards.Remove(cardToAdd);
        SetCardPostionsInHand();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
}
