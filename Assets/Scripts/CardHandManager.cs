using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandManager : MonoBehaviour
{
    // 패에 들어와있는 카드 추적하기 위한 리스트
    private List<Transform> cards = new List<Transform>();

    // 카드 사이의 간격
    private float cardSpacing = 2.0f;

    // 카드 높이 간격
    private float cardHeightSpacing = 0.1f;

    // 카드가 움직이는데 걸리는 시간
    private float moveDuration = 0.3f;

    // 카드 한 장을 손패에 추가하는 함수
    public void AddCard(Transform card)
    {
        cards.Add(card);            // 리스트에 카드 추가
        card.SetParent(transform);  // 카드의 위치를 해당 스크립트 가진 오브젝트 위치로 변경  
        RearrangCards();            // 카드 재정렬
    }

    // 카드 한 장을 제거하는 함수
    public void RemoveCard(Transform card)
    {
        cards.Remove(card);         // 리스트에서 카드 제거
        RearrangCards();            // 카드 재정렬
    }

    // 카드를 재정렬하는 함수
    public void RearrangCards()
    {
        int count = cards.Count;
        float totalWidth = (count - 1) * cardSpacing;
        float startZ = -totalWidth / 2f;

        for (int i = 0; i < count; i++)
        {
            float x = 0f;
            float y = (count - 1 - i) * cardHeightSpacing;
            float z = startZ + i * cardSpacing;

            Vector3 targetPos = new Vector3(x, y, z);
            cards[i].DOLocalMove(targetPos, moveDuration).SetEase(Ease.OutQuad);
        }
    }
}
