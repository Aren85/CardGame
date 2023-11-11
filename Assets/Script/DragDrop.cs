using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
                                            
public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private Canvas canvas;//為了計算canvas縮放比例需要取得canvas物件
    public RectTransform rectTransform;//取得當前物件rectTransform
    private CanvasGroup canvasGroup;//取得當前物件canvasGroup
    //public Transform parentToReturnTo = null;//暫時好像沒用到
    public bool isDeck = false;//用來判定當前卡牌是否處於放置的狀態


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();//取得當前物件rectTransform
        canvasGroup = GetComponent<CanvasGroup>();//取得當前物件canvasGroup
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");

        canvasGroup.alpha = 0.6f;//讓卡牌拖曳時透明化
        
        
        //parentToReturnTo = this.transform.parent;
        //this.transform.SetParent(this.transform.parent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;//暫時不跟其他物件互相交互

    }

    public void OnDrag(PointerEventData eventData)
    {

        //Debug.Log("OnDrag");
        Debug.Log(rectTransform.anchoredPosition);
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;//拖曳功能移動計算
        rectTransform.localScale = Vector3.one * 1.5f;//拖曳時卡牌放大1.5倍
        


    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");

        canvasGroup.alpha = 1f;//讓卡牌透明度恢復正常

        
        GetComponent<CanvasGroup>().blocksRaycasts = true;//開啟跟其他物件交互功能
        if (!isDeck)
        {
            rectTransform.position = GameObject.Find("HandController").transform.position;//回歸手牌控制物件位置
            rectTransform.localScale = Vector3.one;//卡牌比例回歸正常
        }

    }

    public void OnPointerDown(PointerEventData eventData)//點擊
    {
        isDeck = false;//目前不是放置卡牌狀態
        transform.SetParent(GameObject.Find("HandController").transform);//點擊瞬間改變父物件為手牌控制物件
        Debug.Log("OnPointerDown");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        
    }

    public void OnPointerEnter(PointerEventData eventData)//游標移入
    {
        if(!isDeck)//目前不是放置卡牌狀態
        {
            Debug.Log("OnPointerEnter");
            rectTransform.localScale = Vector3.one * 1.5f;//游標移入放大卡牌
        }
        
        //eventData.pointerDrag.GetComponent<RectTransform>().sizeDelta = new Vector2(240, 360);
        
    }

    public void OnPointerExit(PointerEventData eventData)//游標移出
    {
        if (!isDeck)//目前不是放置卡牌狀態
        {
            Debug.Log("OnPointerExit");
            rectTransform.localScale = Vector3.one;//游標離開恢復卡牌大小
        }


    }
}
