using System;
using UnityEngine;

namespace config
{
    // 命名空間 'config' 中的 AnimationSpeedConfig 類別
    [Serializable]
    public class AnimationSpeedConfig
    {
        // 旋轉的動畫速度，單位為度每秒
        [SerializeField]
        [Tooltip("動畫速度，以度每秒為單位")]
        public float rotation = 60f;

        // 位置變化的動畫速度，單位為每秒的像素數
        [SerializeField]
        [Tooltip("位置變化的動畫速度，單位為每秒的像素數。")]
        public float position = 500f;

        // 釋放時位置變化的動畫速度，單位為每秒的像素數
        [SerializeField]
        [Tooltip("釋放時位置變化的動畫速度，單位為每秒的像素數。")]
        public float releasePosition = 2000f;

        // 縮放的動畫速度，單位為每秒的縮放比例
        [SerializeField]
        [Tooltip("縮放的動畫速度，單位為每秒的縮放比例。")]
        public float zoom = 0.3f;
    }
}