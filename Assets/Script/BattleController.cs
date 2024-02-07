using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController instance;
    private void Awake()
    {
        instance = this;
    }
    public int startingCardAmount = 5;
    public int cardsToDrawPlayerTurn = 3;
    public enum TurnOrder
    {
        playerActive,
        playerCardAttacks,
        enemyActive,
        enemyCardAttacks
    } 
    public TurnOrder currentPhase;

    void Start()
    {
        DeckController.instance.DrawMultipleCards(startingCardAmount);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            AdvanceTurn();
        }
    }
    public void AdvanceTurn()
    {
        currentPhase++;
        if((int)currentPhase >= System.Enum.GetValues(typeof(TurnOrder)).Length)
        {
            currentPhase = 0;
        }
        
        switch(currentPhase)
        {
            case TurnOrder.playerActive:
                UIController.instance.endTurnButton.SetActive(true);

                DeckController.instance.DrawMultipleCards(cardsToDrawPlayerTurn);
                break;

            case TurnOrder.playerCardAttacks:

                //Debug.Log("skipping player card attacks");
                //AdvanceTurn();
                CardPointsController.instance.PlayerAttack();

                break; 

            case TurnOrder.enemyActive:
                Debug.Log("skipping enemy actions");
                AdvanceTurn();
                break;

            case TurnOrder.enemyCardAttacks:
                Debug.Log("skipping enemy card attacks");
                AdvanceTurn();
                break;
        }
    }

    public void EndPlayerTurn()
    {
        UIController.instance.endTurnButton.SetActive(false);
        AdvanceTurn();
    }
}
