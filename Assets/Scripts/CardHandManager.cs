using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandManager : MonoBehaviour
{
    // �п� �����ִ� ī�� �����ϱ� ���� ����Ʈ
    private List<Transform> cards = new List<Transform>();

    // ī�� ������ ����
    private float cardSpacing = 2.0f;

    // ī�� ���� ����
    private float cardHeightSpacing = 0.1f;

    // ī�尡 �����̴µ� �ɸ��� �ð�
    private float moveDuration = 0.3f;

    // ī�� �� ���� ���п� �߰��ϴ� �Լ�
    public void AddCard(Transform card)
    {
        cards.Add(card);            // ����Ʈ�� ī�� �߰�
        card.SetParent(transform);  // ī���� ��ġ�� �ش� ��ũ��Ʈ ���� ������Ʈ ��ġ�� ����  
        RearrangCards();            // ī�� ������
    }

    // ī�� �� ���� �����ϴ� �Լ�
    public void RemoveCard(Transform card)
    {
        cards.Remove(card);         // ����Ʈ���� ī�� ����
        RearrangCards();            // ī�� ������
    }

    // ī�带 �������ϴ� �Լ�
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
