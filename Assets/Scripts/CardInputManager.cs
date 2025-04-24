using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class CardInputManager : MonoBehaviour
{
    private CardHighlight lastHovered;          // ���������� �ǵ帰 ī�� ����
    private Transform selectedCard;             // ���� ���õ� ī�� ��ġ ����

    private Vector3 selectedCardOriginScale;    // ���� ���õ� ī���� ���� ������(ũ��)
    private Vector3 selectedCardOriginPos;      // ���� ���õ� ī���� ���� ��ġ

    private Vector3 dragStartPos;               // �巡�� ���۵� ��ġ
    private Vector3 dragOffset;                 // �巡�� �� ���콺�� ī�� ���� �Ÿ� ������

    private bool isMouseDown = false;           // ���콺 ��ư�� ������ �ִ��� �Ǵ�
    private bool isDragging = false;            // ���� �巡�� ������ ����

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
                if (lastHovered != null && lastHovered != cardHighlight)
                {
                    lastHovered.SetHighlight(false);
                }

                // ���� ī�� ���̶���Ʈ �ѱ�
                cardHighlight.SetHighlight(true);
                lastHovered = cardHighlight;
            }
            else
            {
                // �浹�� ������Ʈ�� ī�尡 �ƴ϶�� -> ���� ǥ�� ����
                if (lastHovered != null)
                {
                    lastHovered.SetHighlight(false);
                    lastHovered = null;
                }

                if (Input.GetMouseButtonDown(0) && selectedCard != null)
                {
                    selectedCard.DOScale(Vector3.one, 0.2f);
                    selectedCard.DOLocalMove(selectedCardOriginPos, 0.2f);
                    selectedCard = null;
                }
            }

            // ī�� Ŭ�� ���� ( ���� ���콺 ��ư Ŭ�� )
            if (Input.GetMouseButtonDown(0) && cardHighlight != null)
            {
                // ���õ� ī�忡 ���� ���콺 ������ ��ġ�� ���� ������Ʈ(ī��) ��ġ ���� ����
                selectedCard = hit.transform;

                // ���õ� ī���� ���� ��ġ, ũ�� ���� ( ���� ��ġ, ũ��� �����ϱ� ���ؼ� )
                selectedCardOriginScale = selectedCard.localScale;
                selectedCardOriginPos = selectedCard.localPosition;

                // �巡�� ���� ��ġ�� ���� ���콺 ��ġ�� ����
                dragStartPos = Input.mousePosition;
                // ���콺 ��ư �����ٰ� �Ǵ�
                isMouseDown = true;
            }
        }
        else
        {
            // ���콺�� ������Ʈ ���� ���� ���� ��� -> ���� ����
            if (lastHovered != null)
            {
                lastHovered.SetHighlight(false);
                lastHovered = null;
            }

            if (Input.GetMouseButtonDown(0) && selectedCard != null)
            {
                ResetCardSelection();
            }
        }

        // ���콺�� ���� ���¿���, �巡�� ���� ��Ȳ�� �ƴϸ�
        if (isMouseDown && !isDragging)
        {
            // �巡�׷� �Ǵ��� �Ÿ� ���
            float dragDistance = Vector3.Distance(dragStartPos, Input.mousePosition);

            // ���콺�� ���� ���¿��� �巡�׸� �����Ÿ� �̻� �ϸ�?
            if (dragDistance > 10f)
            {
                isDragging = true;

                // �巡�� ���۽� ������ ���
                Ray dragRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(dragRay, out RaycastHit dargHit))
                {
                    dragOffset = selectedCard.position - dargHit.point;
                }

                // ���õ� ī�� ���� ũ��� ����
                selectedCard.DOScale(selectedCardOriginScale, 0.2f);
            }
        }

        // �巡�� ���̸鼭, ���� ���õ� ī�尡 �ִٸ�? -> �巡��
        if (isDragging && selectedCard != null)
        {
            Ray dragRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(dragRay , out RaycastHit dragHit))
            {
                Vector3 targetPos = dragHit.point + dragOffset;
                targetPos.y = selectedCardOriginPos.y;

                selectedCard.position = targetPos;
            }
        }

        // ���콺 ��ư ���� ���¿��� ( Ŭ���� �ߴ�, �巡�׸� �ߴ�) ��ư�� ���ٸ�
        if (Input.GetMouseButtonUp(0) && isMouseDown)
        {
            isMouseDown = false;

            Ray dropRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(dropRay, out RaycastHit dropHit))
            {
                // �巡�� ���¿��ٸ�? -> �ʵ� ���� or ���� ���·� ����
                if (isDragging)
                {
                    isDragging = false;

                    // ���콺 ���� ��ġ�� �ݶ��̴��� Field �±׶��? ( ���� �ʵ� ��ġ )
                    if (dropHit.collider.CompareTag("Field"))
                    {
                        // ī�� ���� ó��
                        var handCard = FindObjectOfType<CardHandManager>();
                        handCard.RemoveCard(selectedCard);

                        selectedCard.DOMove(dropHit.point, 0.2f).SetEase(Ease.OutQuad);
                        selectedCard.SetParent(null);
                        selectedCard = null;
                        return;
                    }
                    else
                    {
                        // Field �±װ� �ƴ϶�� ���� ��ġ�� ���� ó��
                        selectedCard.DOLocalMove(selectedCardOriginPos, 0.2f).SetEase(Ease.OutQuad);
                        selectedCard = null;
                        return;
                    }
                }
                else if (selectedCard != null)
                {
                    // ���õ� ī�尡 �ִµ�, �巡�� ���°� �ƴϴ�? �� Ŭ�� ���¶�� �Ҹ�
                    Vector3 offset = new Vector3(4.5f, 3f, 1.5f);
                    selectedCard.DOScale(Vector3.one * 1.4f, 0.2f);
                    selectedCard.DOLocalMove(selectedCardOriginPos + offset, 0.2f);
                }
            }
        }
    }

    // ���� ũ��, ���·� ���� ó���ϴ� �Լ�
    public void ResetCardSelection()
    {
        if (selectedCard != null)
        {
            selectedCard.DOScale(Vector3.one, 0.2f);
            selectedCard.DOLocalMove(selectedCardOriginPos, 0.2f);
            selectedCard = null;
        }
    }
}
