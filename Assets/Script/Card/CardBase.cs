using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Unity.Burst.Intrinsics.X86.Avx;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;
using Unity.Burst.CompilerServices;

public class CardBase : MonoBehaviour
{
    public CardScriptableObject cardSO;
    public string cardName;
    public string description;

    public SpriteRenderer cardType;
    public Image cardSuit;
    public Image cardGraphic;
    
    public TMP_Text cardNameText;
    public TMP_Text descirptionText;
    public TMP_Text cardRankText;

    private Vector3 targetPoint;
    //private Quaternion targetRot;
    public float moveSpeed = 2f;
    public bool inHand;
    public int handPosition;
    private HandController handController;

    private bool isSelected;
    private Collider2D theCollider2D;

    public LayerMask whatIsDesktop;
    public LayerMask whatIsPlacement;
    private bool justPressed;

    public CardPlacePoint assignedPlace;

    public List<CardEffect> effects;



    private void Awake()
    {
        theCollider2D = GetComponent<Collider2D>();
        handController = FindObjectOfType<HandController>();
        cardType = GetComponent<SpriteRenderer>();

    }
    private void Start()
    {
        SetupCard();
    }
    private void Update()
    {

        transform.position = Vector3.Lerp(transform.position, targetPoint, moveSpeed * Time.deltaTime);



        //if (isSelected)
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        //    if (Physics.Raycast(ray, out hit, 100f, whatIsDesktop))
        //    {
        //        MoveToPoint(hit.point + new Vector3());
        //    }
        //}
        if (isSelected)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            MoveToPoint(new Vector3(mousePos.x,mousePos.y,transform.position.z));
            transform.position = targetPoint;
            Debug.Log(hit);
            if(Input.GetMouseButtonDown(1))
            {
                ReturnToHand();
            }
            if (Input.GetMouseButtonDown(0) && justPressed == false )
            {
                
                if (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100f, whatIsPlacement) && BattleController.instance.currentPhase == BattleController.TurnOrder.playerActive)
                {
                    
                    CardPlacePoint selectedPoint = hit.collider.GetComponent<CardPlacePoint>();
                    if(selectedPoint.activeCard == null && selectedPoint.isPlayerPoint)
                    {
                        selectedPoint.activeCard = this;
                        assignedPlace = selectedPoint;

                        MoveToPoint(selectedPoint.transform.position);
                        inHand = false;
                        isSelected = false;
                        handController.RemoveCardFromHand(this);
                    }
                }
                else
                {
                    ReturnToHand();
                }

            }
        }
        justPressed = false;
    }
    public void SetupCard()
    {
        cardName = cardSO.cardName.ToString();
        description = cardSO.cardDescription.ToString();

        cardNameText.text = cardName;
        descirptionText.text = description;

        cardRankText.text = cardSO.GetCardRank();

        cardType.sprite = cardSO.GetCardTypeSprite();
        cardSuit.sprite = cardSO.GetCardSuitSprite();
        cardGraphic.sprite = cardSO.cardImage;
    }
    public void MoveToPoint(Vector3 pointToMoveTo)
    {
        targetPoint = pointToMoveTo;
    }
    private void OnMouseOver()
    {
        if(inHand)
        {
            MoveToPoint(handController.cardPositions[handPosition] + new Vector3(0f, 1f ,20f));
        }
    }
    private void OnMouseExit()
    {
        if(inHand)
        {
            MoveToPoint(handController.cardPositions[handPosition]);
        }
    }

    private void OnMouseDown()
    {
        if(inHand && BattleController.instance.currentPhase == BattleController.TurnOrder.playerActive)
        {
            isSelected = true;
            theCollider2D.enabled = false;

            justPressed = true;
        }
    }

    public void ReturnToHand()
    {
        isSelected = false;
        theCollider2D.enabled = true;
        MoveToPoint(handController.cardPositions[handPosition]);
    }
}




