using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class CardInputManager : MonoBehaviour
{
    private CardHighlight lastHovered;
    private Transform selectedCard;

    private Vector3 selectedCardOriginScale;
    private Vector3 selectedCardOriginPos;

    private void Update()
    {
        // �� �����Ӹ��� ī�޶󿡼� ���콺 ��ġ�� ����(Ray)�� ��
        // � ������Ʈ�� �浹�ϴ��� �˻�
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Ray�� � ������Ʈ�� �浹�ߴ��� �˻�
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // �ε��� ������Ʈ�� CardHighlight ������Ʈ�� �ִ��� Ȯ��
            CardHighlight cardHighlight = hit.collider.GetComponent<CardHighlight>();

            // CardHighlight�� null�� �ƴ϶� �Ҹ��� ��, �浹�� ������Ʈ�� ī���� ��
            if (cardHighlight != null)
            {
                // ������ Hover�� ī�尡 �ְ�, ���� ī��� �ٸ��ٸ�? -> ���� ī���� ���̶���Ʈ ����
                if(lastHovered != null && lastHovered != cardHighlight)
                {
                    lastHovered.SetHighlight(false);
                }

                // ���� ī�� ���̶���Ʈ �ѱ�
                cardHighlight.SetHighlight(true);
                lastHovered = cardHighlight;

                // ���콺 ���� ��ư Ŭ������ ��� -> ī�� ���� ó��
                if(Input.GetMouseButtonDown(0))
                {
                    // �̹� ���õ� ī�尡 �ٸ� ī�忴�ٸ� -> ���� ���·� ���� ó��
                    if (selectedCard != hit.transform)
                    {
                        // ������ ���õ� ī�尡 �־��ٸ�? -> ���� ���·� ����
                        if(selectedCard != null)
                        {
                            selectedCard.DOScale(Vector3.one, 0.2f);    // ũ�� ����
                            selectedCard.DOLocalMove(selectedCardOriginPos, 0.2f);   // ��ġ ����
                        }

                        // �� ī�� ����
                        selectedCard = hit.transform;

                        // ���õ� ī���� ���� ���� ���� (�� ����)
                        selectedCardOriginScale = selectedCard.transform.localScale;
                        selectedCardOriginPos = selectedCard.transform.localPosition;

                        // ���õ� ī�� Ȯ��, Y�� ��¦ ���� ���
                        Vector3 offset = new Vector3(4.5f, 3f, 1.5f);
                        selectedCard.DOScale(Vector3.one * 1.4f, 0.2f);
                        selectedCard.DOLocalMove(selectedCardOriginPos + offset, 0.2f);
                    }

                }
            }
            else
            {
                // ���õ� ī�尡 �ִ� ���¿���
                // ī�尡 �ƴ� �ٸ� Collider�� ���콺 ���� Ŭ�� ���� ��
                if (Input.GetMouseButtonDown(0) && selectedCard != null)
                {
                    // ���� ī�� ���� ���·� ����
                    selectedCard.DOScale(Vector3.one, 0.2f);
                    selectedCard.DOLocalMove(selectedCardOriginPos, 0.2f);
                    selectedCard = null;
                }
            }
        }
        else
        {
            // ���콺�� � Collider�͵� �浹���� �ʾ��� �� (�� ���� ���� ���� ��)

            // ���� ���̶���Ʈ �Ǿ��ִ� ī�尡 �ִٸ�? -> ���̶���Ʈ ����
            if (lastHovered != null)
            {
                lastHovered.SetHighlight(false);
                lastHovered = null;
            }

            // �� ���� Ŭ�� �� -> ���õ� ī�尡 �ִٸ�? ���� ���·� ����
            if(Input.GetMouseButtonDown(0) && selectedCard != null)
            {
                selectedCard.DOScale(Vector3.one, 0.2f);
                selectedCard.DOLocalMove(selectedCardOriginPos, 0.2f);
                selectedCard = null;
            }
        }
    }
}
