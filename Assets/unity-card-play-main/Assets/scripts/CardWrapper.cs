using config;
using events;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardWrapper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private const float EPS = 0.01f;

    // 目標旋轉角度
    public float targetRotation;
    // 目標位置
    public Vector2 targetPosition;
    // 目標垂直位移
    public float targetVerticalDisplacement;
    // UI層級
    public int uiLayer;

    private RectTransform rectTransform;
    private Canvas canvas;

    // 用於卡片縮放的配置
    public ZoomConfig zoomConfig;
    // 用於動畫速度的配置
    public AnimationSpeedConfig animationSpeedConfig;
    // 卡片所在的容器
    public CardContainer container;

    private bool isHovered;
    private bool isDragged;
    private Vector2 dragStartPos;
    // 事件配置
    public EventsConfig eventsConfig;

    // 卡片的寬度
    public float width
    {
        get => rectTransform.rect.width * rectTransform.localScale.x;
    }

    private void Awake()
    {
        // 獲取 RectTransform 組件
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        // 獲取 Canvas 組件
        canvas = GetComponent<Canvas>();
    }

    private void Update()
    {
        // 更新旋轉、位置、縮放、UI層級
        UpdateRotation();
        UpdatePosition();
        UpdateScale();
        UpdateUILayer();
    }

    private void UpdateUILayer()
    {
        // 當卡片既未被懸停也未被拖動時，設置Canvas的sortingOrder為uiLayer
        if (!isHovered && !isDragged)
        {
            canvas.sortingOrder = uiLayer;
        }
    }

    private void UpdatePosition()
    {
        if (!isDragged)
        {
            // 計算卡片的目標位置，並根據懸停和配置進行調整
            var target = new Vector2(targetPosition.x, targetPosition.y + targetVerticalDisplacement);
            if (isHovered && zoomConfig.overrideYPosition != -1)
            {
                target = new Vector2(target.x, zoomConfig.overrideYPosition);
            }

            var distance = Vector2.Distance(rectTransform.position, target);
            var repositionSpeed = rectTransform.position.y > target.y || rectTransform.position.y < 0
                ? animationSpeedConfig.releasePosition
                : animationSpeedConfig.position;
            // 使用Lerp進行平滑的位置過渡
            rectTransform.position = Vector2.Lerp(rectTransform.position, target,
                repositionSpeed / distance * Time.deltaTime);
        }
        else
        {
            // 如果卡片正在被拖動，根據滑鼠位置設置卡片位置
            var delta = ((Vector2)Input.mousePosition + dragStartPos);
            rectTransform.position = new Vector2(delta.x, delta.y);
        }
    }

    private void UpdateScale()
    {
        // 計算目標縮放值，根據配置進行適當的縮放
        var targetZoom = (isDragged || isHovered) && zoomConfig.zoomOnHover ? zoomConfig.multiplier : 1;
        var delta = Mathf.Abs(rectTransform.localScale.x - targetZoom);
        // 使用Lerp進行平滑的縮放過渡
        var newZoom = Mathf.Lerp(rectTransform.localScale.x, targetZoom,
            animationSpeedConfig.zoom / delta * Time.deltaTime);
        rectTransform.localScale = new Vector3(newZoom, newZoom, 1);
    }

    private void UpdateRotation()
    {
        var crtAngle = rectTransform.rotation.eulerAngles.z;
        // 如果角度為負數，則添加360以獲得正數等價值
        crtAngle = crtAngle < 0 ? crtAngle + 360 : crtAngle;
        // 如果卡片被懸停並且應重置旋轉，將目標旋轉設置為0
        var tempTargetRotation = (isHovered || isDragged) && zoomConfig.resetRotationOnZoom
            ? 0
            : targetRotation;
        tempTargetRotation = tempTargetRotation < 0 ? tempTargetRotation + 360 : tempTargetRotation;
        var deltaAngle = Mathf.Abs(crtAngle - tempTargetRotation);
        if (!(deltaAngle > EPS)) return;

        // 調整當前角度和目標角度，以便旋轉是在最短的方向上完成的
        var adjustedCurrent = deltaAngle > 180 && crtAngle < tempTargetRotation ? crtAngle + 360 : crtAngle;
        var adjustedTarget = deltaAngle > 180 && crtAngle > tempTargetRotation
            ? tempTargetRotation + 360
            : tempTargetRotation;
        var newDelta = Mathf.Abs(adjustedCurrent - adjustedTarget);

        // 使用Lerp進行平滑的旋轉過渡
        var nextRotation = Mathf.Lerp(adjustedCurrent, adjustedTarget,
            animationSpeedConfig.rotation / newDelta * Time.deltaTime);
        rectTransform.rotation = Quaternion.Euler(0, 0, nextRotation);
    }

    // 設置錨點位置
    public void SetAnchor(Vector2 min, Vector2 max)
    {
        rectTransform.anchorMin = min;
        rectTransform.anchorMax = max;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDragged)
        {
            // 拖動時避免懸停事件
            return;
        }
        if (zoomConfig.bringToFrontOnHover)
        {
            // 懸停時將Canvas的sortingOrder設置為zoomConfig.zoomedSortOrder
            canvas.sortingOrder = zoomConfig.zoomedSortOrder;
        }

        // 觸發卡片懸停事件
        eventsConfig?.OnCardHover?.Invoke(new CardHover(this));
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDragged)
        {
            // 拖動時避免懸停事件
            return;
        }
        // 離開時將Canvas的sortingOrder恢復為uiLayer
        canvas.sortingOrder = uiLayer;
        isHovered = false;
        // 觸發卡片離開事件
        eventsConfig?.OnCardUnhover?.Invoke(new CardUnhover(this));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 設置卡片為被拖動狀態
        isDragged = true;
        // 記錄拖動開始位置
        dragStartPos = new Vector2(transform.position.x - eventData.position.x,
            transform.position.y - eventData.position.y);
        // 通知容器開始拖動卡片
        container.OnCardDragStart(this);
        // 觸發卡片離開事件
        eventsConfig?.OnCardUnhover?.Invoke(new CardUnhover(this));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 拖動結束，設置卡片為非拖動狀態
        isDragged = false;
        // 通知容器拖動卡片結束
        container.OnCardDragEnd();
    }
}