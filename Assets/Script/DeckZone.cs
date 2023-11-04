using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop DeckZone");

        DragDrop dragDrop = eventData.pointerDrag.GetComponent<DragDrop>();
        if (dragDrop != null)
        {
            // 記錄原始大小

            // 將被拖曳物件的 RectTransform 的錨定位置設置為當前物件 RectTransform 的錨定位置。
            //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            //eventData.pointerDrag.GetComponent<Transform>().parent = GetComponent<Transform>().parent;
            dragDrop.isDeck = true;
            dragDrop.transform.SetParent(this.transform);
            dragDrop.rectTransform.position = GetComponent<RectTransform>().position;
            //dragDrop.rectTransform.SetParent(GetComponent<RectTransform>().transform);
            dragDrop.rectTransform.localScale = Vector3.one * 0.6f;
        }
    }
}
