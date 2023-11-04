using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
                                            
public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private Canvas canvas;
    public RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public Transform parentToReturnTo = null;
    public bool isDeck = false;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");

        canvasGroup.alpha = 0.6f;
        
        
        //parentToReturnTo = this.transform.parent;
        //this.transform.SetParent(this.transform.parent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;

    }

    public void OnDrag(PointerEventData eventData)
    {

        Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        rectTransform.localScale = Vector3.one * 1.5f;


    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");

        canvasGroup.alpha = 1f;

        
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (!isDeck)
        {
            rectTransform.position = GameObject.Find("HandController").transform.position;
            rectTransform.localScale = Vector3.one;
        }

    }

    public void OnPointerDown(PointerEventData eventData)//點擊
    {
        isDeck = false;
        transform.SetParent(GameObject.Find("HandController").transform);
        Debug.Log("OnPointerDown");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!isDeck)
        {
            Debug.Log("OnPointerEnter");
            rectTransform.localScale = Vector3.one * 1.5f;
        }
        
        //eventData.pointerDrag.GetComponent<RectTransform>().sizeDelta = new Vector2(240, 360);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isDeck)
        {
            Debug.Log("OnPointerExit");
            rectTransform.localScale = Vector3.one;
        }


    }
}
