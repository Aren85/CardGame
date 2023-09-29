using UnityEngine;
using System.Collections;

/// <summary>
/// This script should be attached to the card game object to display card`s rotation correctly.
/// </summary>

[ExecuteInEditMode]
public class BetterCardRotation : MonoBehaviour {

    //  所有卡牌正面圖形的父遊戲對象
    public RectTransform CardFront;

    // 所有卡牌背面圖形的父遊戲對象
    public RectTransform CardBack;

    // 一個空的遊戲對象，放置在卡牌正面上方一點的位置，位於卡牌的中心
    public Transform targetFacePoint;

    // 連接到卡上的 3d 碰撞器（像 BoxCollider2D 這樣的 2d 碰撞器在這種情況下不起作用）
    public Collider col;

    // 如果這是真的，我們的玩家目前會看到卡牌背面
    private bool showingBack = false;

    // 每幀調用一次更新
    void Update () 
    {
        // 從相機到卡片表面目標點的光線投射
        // 如果它穿過卡片的碰撞器，我們應該顯示卡片的背面
        
        RaycastHit[] hits; //這個陣列將用來存儲射線投射的結果，每個元素都是一個RaycastHit物件。
        
        hits = Physics.RaycastAll(origin: Camera.main.transform.position, 
                                  direction: (-Camera.main.transform.position + targetFacePoint.position).normalized, 
            maxDistance: (-Camera.main.transform.position + targetFacePoint.position).magnitude) ;

        //origin: Camera.main.transform.position：這是射線的起點，它是相機的位置，通常是主攝影機（Camera.main）的位置。

        //direction: (-Camera.main.transform.position + targetFacePoint.position).normalized：
        //這是射線的方向向量，它是由相機位置指向targetFacePoint位置的向量。
        //.normalized用於確保方向向量具有單位長度，因此它只表示方向而不包含距離信息。

        //maxDistance: (-Camera.main.transform.position + targetFacePoint.position).magnitude：
        //這是射線的最大投射距離，它是相機位置和targetFacePoint位置之間的距離。

        //這段程式碼的作用是從相機位置向targetFacePoint位置發射一條射線，
        //檢查射線是否與遊戲世界中的物體相交，並將所有相交點的詳細資訊存儲在hits陣列中。

        bool passedThroughColliderOnCard = false;  //通過卡片上的碰撞器
        
        foreach (RaycastHit h in hits) 
        {
            if (h.collider == col)
                passedThroughColliderOnCard = true;
        }
        //檢查射線投射結果中的每個碰撞點，看是否與特定的碰撞器col相交，
        //如果有相交，則將passedThroughColliderOnCard設置為true，


        //Debug.Log("TotalHits: " + hits.Length); 
        if (passedThroughColliderOnCard!= showingBack)//這行程式碼檢查 passedThroughColliderOnCard 是否與 showingBack 不相等。
                                                      //如果它們不相等，則表示射線是否穿過卡片的碰撞器發生了變化。
        
        {   
            // 如果有變化，則執行以下操作：
            showingBack = passedThroughColliderOnCard;  //將 showingBack 設置為 passedThroughColliderOnCard 的值，以反映卡片是否被穿過
            if (showingBack)
            {
                // show the back side
                CardFront.gameObject.SetActive(false);  //將卡片正面的遊戲物件設置為不啟用（隱藏）。
                CardBack.gameObject.SetActive(true);    //將卡片背面的遊戲物件設置為啟用（顯示）。 
            }
            else
            {
                // show the front side
                CardFront.gameObject.SetActive(true);  //將卡片正面的遊戲物件設置為啟用（顯示）。
                CardBack.gameObject.SetActive(false);  //將卡片背面的遊戲物件設置為不啟用（隱藏）。
            }

        }

	}
}
