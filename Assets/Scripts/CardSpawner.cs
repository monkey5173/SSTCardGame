using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;

    [SerializeField]
    Transform[] handSlots;

    private void Start()
    {
        var cardDataList = Resources.LoadAll<CardSobj>("CardAssets");

        for (int i = 0; i < cardDataList.Length && i < handSlots.Length; i++)
        {
            GameObject card = Instantiate(cardPrefab);
            card.GetComponent<CardDisplay>().SetCardData(cardDataList[i]);

            float delay = 0.4f * i;

            card.GetComponent<CardDraw>().AnimateDraw(handSlots[i].position, delay);
        }
    }
}
