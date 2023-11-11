using System;
using events;
using UnityEngine;
using UnityEngine.Events;

namespace config
{
    // 命名空間 'config' 中的 EventsConfig 類別
    [Serializable]
    public class EventsConfig
    {
        // 當卡片被遊玩時觸發的 UnityEvent，帶有 CardPlayed 類型的參數
        [SerializeField]
        [Tooltip("當卡片被遊玩時觸發的 UnityEvent，帶有 CardPlayed 類型的參數")]
        public UnityEvent<CardPlayed> OnCardPlayed;

        // 當光標懸停在卡片上時觸發的 UnityEvent，帶有 CardHover 類型的參數
        [SerializeField]
        [Tooltip("當光標懸停在卡片上時觸發的 UnityEvent，帶有 CardHover 類型的參數")]
        public UnityEvent<CardHover> OnCardHover;

        // 當光標離開卡片時觸發的 UnityEvent，帶有 CardUnhover 類型的參數
        [SerializeField]
        [Tooltip("當光標離開卡片時觸發的 UnityEvent，帶有 CardUnhover 類型的參數")]
        public UnityEvent<CardUnhover> OnCardUnhover;

        // 當卡片被銷毀時觸發的 UnityEvent，帶有 CardDestroy 類型的參數
        [SerializeField]
        [Tooltip("當卡片被銷毀時觸發的 UnityEvent，帶有 CardDestroy 類型的參數")]
        public UnityEvent<CardDestroy> OnCardDestroy;
    }
}
