using System;
using UnityEngine;

namespace config
{
    // 命名空間 'config' 中的 CardPlayConfig 類別
    [Serializable]
    public class CardPlayConfig
    {
        // 玩牌區域的 RectTransform，用於確定卡片是否在遊戲區域內
        [SerializeField]
        [Tooltip("玩牌區域的 RectTransform，用於確定卡片是否在遊戲區域內。")]
        public RectTransform playArea;

        // 遊戲中是否銷毀卡片
        [SerializeField]
        [Tooltip("遊戲中是否銷毀卡片。")]
        public bool destroyOnPlay;
    }
}
