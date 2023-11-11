using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCurveCardLayout : MonoBehaviour
{
    public List<RectTransform> heldCards;
    public RectTransform leftObject;
    public RectTransform rightObject;
    public float curveHeight = 2f;

    private List<Vector2> cardPositions = new List<Vector2>();

    void Start()
    {
        CalculateCardPositions();
        SetCardPositions();
    }

    void CalculateCardPositions()
    {
        int numberOfCards = heldCards.Count;

        Vector2 leftPos = leftObject.anchoredPosition;
        Vector2 rightPos = rightObject.anchoredPosition;
        float distance = Vector2.Distance(leftPos, rightPos);

        for (int i = 0; i < numberOfCards; i++)
        {
            float t = i / (float)(numberOfCards - 1);
            float curveAmount = Mathf.Sin(t * Mathf.PI);
            Vector2 cardPosition = Vector2.Lerp(leftPos, rightPos, t) + new Vector2(0, curveAmount * curveHeight);
            cardPositions.Add(cardPosition);
        }
    }

    void SetCardPositions()
    {
        int numberOfCards = heldCards.Count;

        for (int i = 0; i < numberOfCards; i++)
        {
            heldCards[i].anchoredPosition = cardPositions[i];
        }
    }
}
