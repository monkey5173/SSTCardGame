using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFieldManager : MonoBehaviour
{
    List<FieldCard> fieldCards = new List<FieldCard>();     // 필드 카드들 저장할 리스트

    private float cardSpacing = 5.5f;       // 카드 간격 ( Z축 )
    private float moveDuration = 0.3f;      // 이동 애니메이션 시간

    // 필드 리스트에 카드를 추가하고, 위치를 매니저 자식으로, 재정렬
    public void AddCard(FieldCard card)
    {
        fieldCards.Add(card);
        card.transform.SetParent(transform);
        RearrangeCards();
    }

    // 필드 리스트에서 카드를 제거하고, 재정렬
    public void RemoveCard(FieldCard card)
    {
        fieldCards.Remove(card);
        RearrangeCards();
    }

    // 카드 재정렬 (Z축)
    public void RearrangeCards()
    {
        int count = fieldCards.Count;

        if (count == 0)
        {
            return;
        }

        float totalWidth = (count - 1) * cardSpacing;
        float startZ = -totalWidth * 2f;

        for (int i = 0; i < count; i++)
        {
            float z = startZ + i * cardSpacing;
            Vector3 targetPos = new Vector3(0f, 0f, z);
            fieldCards[i].transform.DOLocalMove(targetPos, moveDuration).SetEase(Ease.OutQuad);
        }
    }

    // 필드 카드 리스트를 받아오는 함수
    public List<FieldCard> GetFieldCards()
    {
        return fieldCards;
    }
}
