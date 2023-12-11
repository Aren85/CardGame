using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandController : MonoBehaviour
{
    public static HandController instance;
    
    public float radius = 100f; // 扇形的半徑

    public List<CardBase> heldCards = new List<CardBase>();
    public Transform minPos, maxPos;
    public List<Vector3> cardPositions = new List<Vector3>();

    private void Awake()
    {
        instance = this;

    }
    void Start()
    {
        SetCardPostionsInHand();


    }
  
    // Update is called once per frame
    void Update()
    {
        
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
            heldCards[i].handPosition = i;
        }
    }
    public void RemoveCardFromHand(CardBase cardToRemove)
    {
        if (heldCards[cardToRemove.handPosition] == cardToRemove)
        {
            heldCards.RemoveAt(cardToRemove.handPosition);
        }
        else
        {
            Debug.LogError("Card at position " + cardToRemove.handPosition + " is not the card being removed from hand");
        }
        SetCardPostionsInHand();
    }
    public void RemoveCardToHand(CardBase cardToAdd)
    {
        heldCards.Remove(cardToAdd);
        SetCardPostionsInHand();
    }

    public void AddCardToHand(CardBase cardToAdd)
    {
        heldCards.Add(cardToAdd);
        SetCardPostionsInHand();
    }




}
