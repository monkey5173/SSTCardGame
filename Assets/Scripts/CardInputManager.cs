using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInputManager : MonoBehaviour
{
    private CardHighlight lastHovered;
    private Transform selectedCard;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            CardHighlight cardHighlight = hit.collider.GetComponent<CardHighlight>();

            if (cardHighlight != null)
            {
                if(lastHovered != null && lastHovered != cardHighlight)
                {
                    lastHovered.SetHighlight(false);
                }

                cardHighlight.SetHighlight(true);
                lastHovered = cardHighlight;

                if(Input.GetMouseButtonDown(0))
                {
                    if(selectedCard != null)
                    {
                        selectedCard.DOScale(Vector3.one, 0.2f);
                    }

                    selectedCard = hit.transform;
                    selectedCard.DOScale(Vector3.one * 1.4f, 0.2f);
                }
            }
        }

        else
        {
            if (lastHovered != null)
            {
                lastHovered.SetHighlight(false);
                lastHovered = null;
            }

            if(Input.GetMouseButtonDown(0) && selectedCard != null)
            {
                selectedCard.DOScale(Vector3.one, 0.2f);
                selectedCard = null;
            }
        }
    }
}
