using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Card", menuName = "Custom/Card")]
public class Card : ScriptableObject
{
    public string cardName;     // 卡牌的名稱
    public string description;  // 卡牌的描述
    public Sprite cardImage;    // 卡牌的圖像
    public CardSuit suit;       // 使用CardSuite枚举来花色
    public CardValue cardValue; // 使用CardValue枚举来存储点数

    public enum CardSuit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    public enum CardValue
    {
        Ace = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }


}
