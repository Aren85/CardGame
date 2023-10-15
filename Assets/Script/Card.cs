using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardScriptableObject cardSO;
    private HandController handController;
    private Transform card;

    public string cardName;
    public string cardType;
    public string cardDescription;
    public string cardRank;
    public string cardSuit;
    public Image cardSuitSprite;
    public Image cardGraphicSprite;

    private Vector3 targetPoint;
    public float moveSpeed = 10f;
    public bool inHand;
    public int handPosttion;

    public TMP_Text cardTypeText, cardNameText, cardRankText, cardDescirptionText;
    // Start is called before the first frame update
    void Start()
    {
        SetupCard();
        handController = FindObjectOfType<HandController>();
    }

    public void SetupCard()
    {
        cardName = cardSO.cardName.ToString();
        cardType = cardSO.cardType.ToString();
        cardRank = cardSO.GetCardRank();
        cardSuit = cardSO.cardSuit.ToString();

        cardDescription = cardSO.cardDescription.ToString();

        cardNameText.text = cardName;
        cardTypeText.text = cardType;
        cardRankText.text = cardRank;
        cardDescirptionText.text = cardDescription;
        cardSuitSprite.sprite = cardSO.GetCardSuitSprite();
        cardGraphicSprite.sprite = cardSO.cardImage;
    }



    // Update is called once per frame
    void Update()
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
            MoveToPoint(handController.cardPositions[handPosttion] + new Vector3(0f, 1f, -5f));
            transform.localScale = new Vector3(1.5f,1.5f,1f);
        }
    }

    private void OnMouseExit()
    {
        if(inHand) 
        {
            MoveToPoint(handController.cardPositions[handPosttion]);
            transform.localScale = new Vector3(1f, 1f, 1f);

        }
    }
}
