using System;
using UnityEngine;

namespace config
{
    // 命名空間 'config' 中的 ZoomConfig 類別
    [Serializable]
    public class ZoomConfig
    {

        // 是否在懸停時縮放
        [Tooltip("是否在懸停時縮放。")]
        [SerializeField]
        public bool zoomOnHover;

        // 縮放倍率，限定在 1 到 5 之間
        [Range(1f, 5f)]
        [SerializeField]
        [Tooltip("縮放倍率，限定在 1 到 5 之間。")]
        public float multiplier = 1;

        // 覆寫卡片在縮放時的 Y 軸位置，如果設為 -1，則卡片不會在 Y 軸上移動
        [Tooltip("這是卡片在縮放時的 Y 軸位置。如果設為 -1，卡片將不會在 Y 軸上移動。")]
        [SerializeField]
        public float overrideYPosition = -1;

        // UI 層級部分
        [Header("UI 層級")]
        // 卡片未縮放時的排序順序，後續的卡片將有更高的排序順序
        [Tooltip("這是卡片在未縮放時的排序順序。後續的卡片將有更高的排序順序。")]
        [SerializeField]
        public int defaultSortOrder;

        // 懸停時是否將卡片移至最前方
        [Tooltip("懸停時是否將卡片移至最前方")]
        [SerializeField]
        public bool bringToFrontOnHover;

        // 縮放時卡片的排序順序
        [Tooltip("這是卡片在縮放時的排序順序。")]
        [SerializeField]
        public int zoomedSortOrder;

        // 縮放時是否重置旋轉
        [Tooltip("縮放時是否重置旋轉")]
        [SerializeField]
        public bool resetRotationOnZoom;
    }
}
