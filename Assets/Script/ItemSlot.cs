using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{

    
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrag");

        DragDrop dragDrop = eventData.pointerDrag.GetComponent<DragDrop>();
        if (eventData.pointerDrag != null)
        {
            // 記錄原始大小

            // 將被拖曳物件的 RectTransform 的錨定位置設置為當前物件 RectTransform 的錨定位置。
            //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            dragDrop.parentToReturnTo = this.transform;
            eventData.pointerDrag.GetComponent<RectTransform>().localScale = Vector2.one * 0.6f;
        }
    }


}
