using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffect : MonoBehaviour
{
    // 應用卡牌效果到玩家和敵對單位
    public abstract void Apply(Player player, Enemy enemy);
}
