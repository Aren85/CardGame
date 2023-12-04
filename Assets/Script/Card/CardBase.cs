using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Unity.Burst.Intrinsics.X86.Avx;
using UnityEngine.UI;
using Unity.VisualScripting;

public class CardBase : MonoBehaviour
{
    public CardScriptableObject cardSO;
    public string cardName;
    public string description;

    public SpriteRenderer cardType;
    public Image cardSuit;
    public Image cardGraphic;
    
    public TMP_Text cardNameText;
    public TMP_Text descirptionText;
    public TMP_Text cardRankText;

    private Vector3 targetPoint;
    //private Quaternion targetRot;
    public float moveSpeed = 2f;
    public bool inHand;
    public int handPosition;
    private HandController handController;

    public List<CardEffect> effects;



    private void Awake()
    {
        handController = FindObjectOfType<HandController>();
        cardType = GetComponent<SpriteRenderer>();
        cardName = cardSO.cardName.ToString();
        description = cardSO.cardDescription.ToString();
        
        cardNameText.text = cardName;
        descirptionText.text = description;

        cardRankText.text = cardSO.GetCardRank();

        cardType.sprite = cardSO.GetCardTypeSprite();
        cardSuit.sprite = cardSO.GetCardSuitSprite();
        cardGraphic.sprite = cardSO.cardImage;
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPoint, moveSpeed * Time.deltaTime);
    }
    public void MoveToPoint(Vector3 pointToMoveTo)
    {
        targetPoint = pointToMoveTo;
    }
    private void OnMouseOver()
    {
        if(inHand)
        {
            MoveToPoint(handController.cardPositions[handPosition] + new Vector3(0f, 1f ,10f));
        }
    }
    private void OnMouseExit()
    {
        if(inHand)
        {
            MoveToPoint(handController.cardPositions[handPosition]);
        }
    }
}




