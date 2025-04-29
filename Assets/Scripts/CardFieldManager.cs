using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFieldManager : MonoBehaviour
{
    List<FieldCard> fieldCards = new List<FieldCard>();     // �ʵ� ī��� ������ ����Ʈ

    private float cardSpacing = 5.5f;       // ī�� ���� ( Z�� )
    private float moveDuration = 0.3f;      // �̵� �ִϸ��̼� �ð�

    // �ʵ� ����Ʈ�� ī�带 �߰��ϰ�, ��ġ�� �Ŵ��� �ڽ�����, ������
    public void AddCard(FieldCard card)
    {
        fieldCards.Add(card);
        card.transform.SetParent(transform);
        RearrangeCards();
    }

    // �ʵ� ����Ʈ���� ī�带 �����ϰ�, ������
    public void RemoveCard(FieldCard card)
    {
        fieldCards.Remove(card);
        RearrangeCards();
    }

    // ī�� ������ (Z��)
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

    // �ʵ� ī�� ����Ʈ�� �޾ƿ��� �Լ�
    public List<FieldCard> GetFieldCards()
    {
        return fieldCards;
    }
}
