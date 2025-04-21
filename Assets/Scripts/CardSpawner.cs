using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;

    [SerializeField] Transform spawnPoint;

    [SerializeField] float zOffset = 2.5f;

    CardSobj[] cardDataList;

    private void Start()
    {
        cardDataList = Resources.LoadAll<CardSobj>("CardAssets");

        for (int i = 0; i < cardDataList.Length; i++) 
        {
            Vector3 pos = new Vector3(0, 3, i * zOffset);

            GameObject card = Instantiate(cardPrefab, pos, Quaternion.identity, spawnPoint);
            CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
            cardDisplay.SetCardData(cardDataList[i]);
        }
    }
}
