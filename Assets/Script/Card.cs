using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardScriptableObject cardSO;
    private HandController handController;
    private Transform card;
    private RectTransform rectTransform;
    public Canvas canvas;  // 引用你的 Canvas

    public string cardName; //卡牌名稱
    public string cardType; //卡牌類型
    public string cardDescription; //卡牌說明
    public string cardRank; //卡牌數字(階級)
    public string cardSuit; //卡牌花色
    public Image cardSuitSprite; //卡牌花色圖案
    public Image cardGraphicSprite; //卡牌圖案 

    private Vector3 targetPoint;
    private Quaternion targetRot;
    public float moveSpeed = 2f;
    public float rotateSpeed = 540f;
    public bool inHand;
    public bool isSelected; //檢測是否已選擇卡牌 
    public int handPosttion;

    public TMP_Text cardTypeText, cardNameText, cardRankText, cardDescirptionText; //實例化類型、名字、數字(階級)、卡牌說明的文字檔

    private Collider2D thecollider;

    public LayerMask whatIsDesktop;
    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        SetupCard();
        handController = FindObjectOfType<HandController>();

        thecollider = GetComponent<Collider2D>();
    }

    public void SetupCard()
    {
        cardName = cardSO.cardName.ToString();
        cardType = cardSO.cardType.ToString();
        cardRank = cardSO.GetCardRank();
        cardSuit = cardSO.cardSuit.ToString();

        cardDescription = cardSO.cardDescription.ToString();

        cardNameText.text = cardName;
        cardTypeText.text = cardType;
        cardRankText.text = cardRank;
        cardDescirptionText.text = cardDescription;
        cardSuitSprite.sprite = cardSO.GetCardSuitSprite();
        cardGraphicSprite.sprite = cardSO.cardImage;
    }



    // Update is called once per frame
    void Update()
    {
        if(!isSelected)
        {
            rectTransform.position = Vector3.Slerp(rectTransform.position, targetPoint, moveSpeed * Time.deltaTime);
            rectTransform.rotation = Quaternion.RotateTowards(rectTransform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }

        //if (isSelected)
        //{

        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit, 100f, whatIsDesktop))
        //    {
        //        MoveToPoint(hit.point,Quaternion.identity);
        //    }
        //}
    }

    public void MoveToPoint(Vector3 pointToMoveTo)
    {
        targetPoint = pointToMoveTo;
        //targetRot = rotToMatch;
        rectTransform.position = Vector3.Slerp(rectTransform.position, targetPoint, moveSpeed * Time.deltaTime);
        //rectTransform.rotation = Quaternion.RotateTowards(rectTransform.rotation, targetRot, rotateSpeed * Time.deltaTime);
    }

    //private void OnMouseOver()
    //{
    //    if (inHand)
    //    {
    //        MoveToPoint(handController.cardPositions[handPosttion] + new Vector3(0f, 1f, -5f),Quaternion.identity);
    //        transform.localScale = new Vector3(1.5f, 1.5f, 1f);
    //    }
    //}

    //private void OnMouseExit()
    //{
    //    if (inHand)
    //    {
    //        MoveToPoint(handController.cardPositions[handPosttion], handController.minPos.rotation);
    //        transform.localScale = new Vector3(1f, 1f, 1f);

    //    }
    //}

    //private void OnMouseDown()
    //{
    //    if (inHand)
    //    {
    //        isSelected = true;
    //        //thecollider.enabled = false;
    //        Debug.Log(isSelected);
    //    }
    //}
}
