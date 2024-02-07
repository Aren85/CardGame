using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPointsController : MonoBehaviour
{
    public static CardPointsController instance;
    private void Awake()
    {
        instance = this;
    }
    public CardPlacePoint[] playerCardPoints;
    public CardPlacePoint[] ememyCardPoints;
    public float timeBetweenAttacks = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerAttack()
    {
        StartCoroutine(PlayerAttackCo());
    }
    IEnumerator PlayerAttackCo()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        for(int i = 0; i<playerCardPoints.Length; i++)
        {
            if (playerCardPoints[i].activeCard != null)
            {
                if (ememyCardPoints[i].activeCard != null)
                {
                    //Attack the enemy
                }
                else
                {
                    //攻擊怪物本體
                }
                yield return new WaitForSeconds(timeBetweenAttacks);
            }
        }
        BattleController.instance.AdvanceTurn();
    }
}
