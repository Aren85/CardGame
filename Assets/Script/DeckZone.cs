using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckZone : MonoBehaviour, IDropHandler
{
    // 當有物體被拖放到這個區域時，此方法會被呼叫
    public void OnDrop(PointerEventData eventData)
    {
        // 在控制台中輸出一條訊息以確認 OnDrop 方法被呼叫
        Debug.Log("OnDrop DeckZone");

        // 從拖曳事件的物體中取得 DragDrop 腳本的參考
        DragDrop dragDrop = eventData.pointerDrag.GetComponent<DragDrop>();


        // 如果成功取得 DragDrop 腳本的參考
        if (dragDrop != null)
        {
            // 設定 dragDrop 的 isDeck 屬性為 true，表示它現在在 Deck 區域
            dragDrop.isDeck = true;

            // 設定 dragDrop 的父物體為 DeckZone 物件，即將其放在這個區域中
            dragDrop.transform.SetParent(this.transform);

            // 將 dragDrop 的位置設定為 DeckZone 的中心位置
            dragDrop.rectTransform.position = GetComponent<RectTransform>().position;

            // 縮放 dragDrop，使其變為原本大小的 0.6 倍
            dragDrop.rectTransform.localScale = Vector3.one * 0.6f;
        }
    }
}
