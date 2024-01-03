using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card", menuName = "CustomCard/Card")]
public class CardScriptableObject : ScriptableObject
{
    [Header("卡牌名稱設定")]
    public string cardName;     // 卡牌的名稱
    [TextArea]
    [Header("卡牌描述設定")]
    public string cardDescription;  // 卡牌的描述
    [Header("卡牌圖案設定")]
    public Sprite cardImage;    // 卡牌的圖像

    [Header("花色設定")]
    public CardSuit cardSuit;       // 使用CardSuite枚舉来表示花色
    
    public Sprite heartsSprite;
    public Sprite diamondsSprite;
    public Sprite clubsSprite;
    public Sprite spadesSprite;
    
    [Header("花色邊框設定")]
    public Sprite heartsSpriteFrame;
    public Sprite diamondsSpriteFrame;
    public Sprite clubsSpriteFrame;
    public Sprite spadesSpriteFrame;

    [Header("卡牌點數設定")]
    public CardRank cardRank; // 使用CardValue枚舉来存储点数
    [Header("卡牌類型設定")]
    public CardType cardType;

    public Sprite AttackSprite;
    public Sprite SkillSprite;
    public Sprite AbilitySprite;
    [Header("卡牌效果設定")]
    public List<CardEffect> effects; // 卡牌效果列表




    public enum CardType
    {
        Attack,//攻擊
        Skill,//技能
        Ability,//能力
    }
    public enum CardSuit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    public enum CardRank
    {
        A = 1,
        _2,
        _3,
        _4,
        _5,
        _6,
        _7,
        _8,
        _9,
        _10,
        J,
        Q,
        K
    }

    public Sprite GetCardSuitSprite()
    {
        switch (cardSuit)
        {
            case CardSuit.Hearts:
                return heartsSprite;
            case CardSuit.Diamonds:
                return diamondsSprite;
            case CardSuit.Clubs:
                return clubsSprite;
            case CardSuit.Spades:
                return spadesSprite;
            default:
                return null; // 如果没有匹配的花色，返回空
        }
    }
    public Sprite GetCardSuitSpriteFrame()
    {
        switch (cardSuit)
        {
            case CardSuit.Hearts:
                return heartsSpriteFrame;
            case CardSuit.Diamonds:
                return diamondsSpriteFrame;
            case CardSuit.Clubs:
                return clubsSpriteFrame;
            case CardSuit.Spades:
                return spadesSpriteFrame;
            default:
                return null; // 如果没有匹配的花色，返回空
        }
    }
    public Sprite GetCardTypeSprite()
    {
        switch(cardType)
        {
            case CardType.Attack:
                return AttackSprite;
            case CardType.Skill:
                return SkillSprite;
            case CardType.Ability:
                return AbilitySprite;
            default:
                return null;
        }
    }
    public string GetCardRank()
    {
        switch(cardRank)
        {
            case CardRank.A:
                return "A";
            case CardRank._2:
                return "2";
            case CardRank._3:
                return "3";
            case CardRank._4:
                return "4";
            case CardRank._5:
                return "5";
            case CardRank._6:
                return "6";
            case CardRank._7:
                return "7";
            case CardRank._8:
                return "8";
            case CardRank._9:
                return "9";
            case CardRank._10:
                return "10";
            case CardRank.J:
                return "j";
            case CardRank.Q:
                return "Q";
            case CardRank.K:
                return "K";

            default: return null;
        }
    }

}
