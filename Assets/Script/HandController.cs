using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public List<Card> heldCards = new List<Card>();
    public Transform minPos, maxPos;
    public List<Vector3> cardPositions = new List<Vector3>();
    // Start is called before the first frame update
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
        if(heldCards.Count > 1)
        {
            distanceBetweenPoints = (maxPos.position - minPos.position) / (heldCards.Count - 1);
        }

        for(int i = 0; i < heldCards.Count; i++)
        {
            cardPositions.Add(minPos.position + (distanceBetweenPoints * i));

            heldCards[i].transform.position = cardPositions[i];
        }
    }
}
