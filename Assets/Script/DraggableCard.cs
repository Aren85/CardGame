using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //private RectTransform cardTransform;  // 存儲卡牌的 RectTransform 組件，用於更新卡牌的位置
    private Transform parentToReturnTo = null;
    //private CanvasGroup canvasGroup;      // 存儲卡牌的 CanvasGroup 組件，用於控制卡牌的交互性

    //private Vector2 originalPosition;    // 保存卡牌的原始位置，以在拖曳結束時返回到這個位置

    // Start is called before the first frame update
    void Start()
    {
        // 獲取卡牌的 RectTransform 組件，用於更新卡牌的位置
        //cardTransform = GetComponent<RectTransform>();
        // 獲取 CanvasGroup 組件，用於控制卡牌的交互性
        //canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 記錄卡牌原始位置
        parentToReturnTo = this.transform.parent;

        this.transform.SetParent(this.transform.parent.parent);

        // 允許卡牌被拖曳
        //canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 根據滑鼠或觸摸輸入更新卡牌位置
        this.transform.position = eventData.position;

        //cardTransform.anchoredPosition += eventData.delta / canvasGroup.transform.lossyScale.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(parentToReturnTo);

        // 結束拖曳，將卡牌恢復到原始位置
        //cardTransform.anchoredPosition = originalPosition;

        // 不再允許卡牌被拖曳
        //canvasGroup.blocksRaycasts = true;
    }
}
