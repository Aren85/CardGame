using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class HandController : MonoBehaviour
{
    public List<Card> heldCards = new List<Card>();
    public Transform minPos, maxPos;
    public List<Vector3> cardPositions = new List<Vector3>();
    public float curveTValue = 1.0f; // 可在Inspector視窗中調整的曲線參數
    // Start is called before the first frame update
    void Start()
    {
        SetCardPostionsInHand();
        //GenerateArcHand();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void SetCardPostionsInHand()
    //{
    //    cardPositions.Clear();
    //    Vector3 center = (maxPos.localPosition+ minPos.localPosition) * 0.5f;
    //    center -= new Vector3(0, 0.5f, 0);
    //    Debug.Log("CENTER: "+center);
    //    Vector3 vecA = maxPos.localPosition - center;
    //    Debug.Log("vecA: " + vecA);
    //    Vector3 vecB = minPos.localPosition - center;
    //    Debug.Log("vecB: " + vecB);
    //    for (int i = 0; i < heldCards.Count; i++)
    //    {
    //        center -= new Vector3(0, 20f, 0);

    //        Vector3 drawVec = Vector3.Slerp(vecA, vecB, 0.1f *i);
    //        drawVec += center;
    //        Debug.Log("drawVec: " + drawVec);
    //        Debug.DrawLine(center, drawVec, Color.yellow,10);

    //        cardPositions.Add(drawVec);
    //        heldCards[i].MoveToPoint(cardPositions[i]);
    //        heldCards[i].inHand = true;
    //        heldCards[i].handPosttion = i;
    //    }
    //}

    public void SetCardPostionsInHand()
    {
        cardPositions.Clear(); // 清除卡牌位置列表

        Vector3 distanceBetweenPoints = Vector3.zero;  // 定義一個 Vector3 變數，表示點之間的距離


        if (heldCards.Count > 1)
        {
            // 計算點之間的距離，以便將卡牌均勻分佈在最小和最大位置之間
            distanceBetweenPoints = (maxPos.position - minPos.position) / (heldCards.Count - 1);
        }

        for (int i = 0; i < heldCards.Count; i++)
        {
            //Vector3 v3 = Vector3.Slerp(minPos.position, maxPos.position, i * (1f/heldCards.Count));
            //Vector3 cardPosition = minPos.position + (distanceBetweenPoints * i);

            float t = Mathf.Pow(i / (float)(heldCards.Count - 1), curveTValue); // 使用Mathf.Pow調整曲線形狀
            Vector3 cardPosition = Vector3.Slerp(minPos.position, maxPos.position, t);
            // 計算每張卡牌的位置並添加到卡牌位置列表中
            //cardPositions.Add(minPos.position + (distanceBetweenPoints * i));
            cardPositions.Add(cardPosition);

            // 設定卡牌的移動目標位置
            //heldCards[i].MoveToPoint(cardPositions[i]);

            // 標記卡牌為手牌中的卡牌
            heldCards[i].inHand = true;

            // 設定卡牌在手牌中的位置
            heldCards[i].handPosttion = i;
        }
    }




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
