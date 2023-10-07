using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardScriptableObject cardSO;

    public string cardName;
    public string cardType;
    public string cardRank;
    public string cardSuit;
    public Image cardSuitSprite;



    public TMP_Text cardTypeText, cardNameText, cardRankText;
    // Start is called before the first frame update
    void Start()
    {
        SetupCard();
    }

    public void SetupCard()
    {
        cardName = cardSO.cardName.ToString();
        cardType = cardSO.cardType.ToString();
        cardRank = cardSO.cardRank.ToString();
        cardSuit = cardSO.cardSuit.ToString();

        cardNameText.text = cardName;
        cardTypeText.text = cardType;
        cardRankText.text = cardRank;
        cardSuitSprite.sprite = cardSO.GetCardSuitSprite();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
