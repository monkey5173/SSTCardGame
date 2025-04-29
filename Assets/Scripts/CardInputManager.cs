using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class CardInputManager : MonoBehaviour
{
    [SerializeField] Transform fieldZonePos;    // 출전 필드 위치

    private CardHighlight lastHovered;          // 마지막으로 건드린 카드 정보
    private Transform selectedCard;             // 현재 선택된 카드 위치 정보
    
    private Vector3 selectedCardOriginScale;    // 현재 선택된 카드의 원래 스케일(크기)
    private Vector3 selectedCardOriginPos;      // 현재 선택된 카드의 원래 위치

    private Vector3 dragStartPos;               // 드래그 시작된 위치
    private Vector3 dragOffset;                 // 드래그 시 마우스와 카드 사이 거리 보정값

    private bool isMouseDown = false;           // 마우스 버튼을 누르고 있는지 판단
    private bool isDragging = false;            // 현재 드래그 중인지 여부
    private bool isCardExpanded = false;        // 현재 카드가 확대된 상태인지 판단

    private void Update()
    {
        // 매 프레임마다 카메라에서 마우스 위치로 광선(Ray)를 쏨
        // 어떤 오브젝트와 충돌하는지 검사
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Ray가 어떤 오브젝트와 충돌했는지 검사
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // 부딪힌 오브젝트에 CardHighlight 컴포넌트가 있는지 확인
            CardHighlight cardHighlight = hit.collider.GetComponent<CardHighlight>();

            // CardHighlight가 null이 아니란 소리는 즉, 충돌한 오브젝트가 카드라는 뜻
            if (cardHighlight != null)
            {
                // 이전에 Hover된 카드가 있고, 현재 카드와 다르다면? -> 이전 카드의 하이라이트 끄기
                if (lastHovered != null && lastHovered != cardHighlight)
                {
                    lastHovered.SetHighlight(false);
                }

                // 현재 카드 하이라이트 켜기
                cardHighlight.SetHighlight(true);
                lastHovered = cardHighlight;
            }
            else
            {
                // 충돌한 오브젝트가 카드가 아니라면 -> 강조 표시 끄기
                if (lastHovered != null)
                {
                    lastHovered.SetHighlight(false);
                    lastHovered = null;
                }
            }

            // 카드 클릭 감지 ( 왼쪽 마우스 버튼 클릭 )
            if (Input.GetMouseButtonDown(0) && cardHighlight != null)
            {
                // 이미 선택된 카드고 확대된 상태라면 -> 다시 클릭하지 않음
                if (selectedCard == hit.transform && isCardExpanded)
                {
                    return;
                }

                // 선택된 카드에 현재 마우스 포인터 위치에 맞은 오브젝트(카드) 위치 정보 담음
                selectedCard = hit.transform;

                // 선택된 카드의 원래 위치, 크기 저장 ( 원래 위치, 크기로 복귀하기 위해서 )
                selectedCardOriginScale = selectedCard.localScale;
                selectedCardOriginPos = selectedCard.position;

                // 드래그 시작 위치를 현재 마우스 위치로 설정
                dragStartPos = Input.mousePosition;
                // 마우스 버튼 눌렀다고 판단
                isMouseDown = true;
            }
            else if (Input.GetMouseButtonDown(0) && selectedCard != null)
            {
                // 카드 외부 클릭 시 원래 상태로 복귀 처리
                ResetCardSelection();
            }
        }
        else
        {
            // 마우스가 오브젝트 위에 있지 않을 경우 -> 강조 해제
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

        // 마우스를 누른 상태에서, 드래그 중인 상황이 아니면
        if (isMouseDown && !isDragging)
        {
            // 드래그로 판단할 거리 계산
            float dragDistance = Vector3.Distance(dragStartPos, Input.mousePosition);

            // 마우스를 누른 상태에서 드래그를 일정거리 이상 하면?
            if (dragDistance > 10f)
            {
                isDragging = true;

                // 드래그 시작시 오프셋 계산
                Ray dragRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(dragRay, out RaycastHit dargHit))
                {
                    dragOffset = selectedCard.position - dargHit.point;
                }

                // 드래그 중 확대 상태 해제
                if (isCardExpanded)
                {
                    selectedCard.DOScale(selectedCardOriginScale, 0.2f);
                    isCardExpanded = false;
                }
            }
        }

        // 드래그 중이면서, 현재 선택된 카드가 있다면? -> 드래그
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

        // 마우스 버튼 눌린 상태에서 ( 클릭을 했던, 드래그를 했던) 버튼을 땐다면
        if (Input.GetMouseButtonUp(0) && isMouseDown)
        {
            isMouseDown = false;

            Ray dropRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(dropRay, out RaycastHit dropHit))
            {
                // 드래그 상태였다면? -> 필드 출전 or 원래 상태로 복귀
                if (isDragging)
                {
                    isDragging = false;

                    // 마우스 놓은 위치에 콜라이더가 Field 태그라면? ( 출전 필드 위치 )
                    if (dropHit.collider.CompareTag("Field"))
                    {
                        // 카드 출전 처리
                        var handCard = FindObjectOfType<CardHandManager>();
                        handCard.RemoveCard(selectedCard);

                        var fieldManager = fieldZonePos.GetComponent<CardFieldManager>();

                        FieldCard fieldCard = selectedCard.GetComponent<FieldCard>();
                        CardDisplay cardDisplay = selectedCard.GetComponent<CardDisplay>();

                        if (fieldCard != null && cardDisplay != null)
                        {
                            fieldManager.AddCard(fieldCard);

                            int cost = cardDisplay.CardCost;
                            int damage = cardDisplay.CardDamage;
                            int health = cardDisplay.CardHealth;

                            fieldCard.Init(cost, damage, health);
                        }


                        return;
                    }
                    else
                    {
                        // Field 태그가 아니라면 원래 위치로 복귀 처리
                        ResetCardSelection();
                        return;
                    }
                }
                else if (selectedCard != null && !isCardExpanded)
                {
                    // 선택된 카드가 있는데, 드래그 상태가 아니다? 즉 클릭 상태라는 소리
                    Vector3 offset = new Vector3(4.5f, 3f, 1.5f);
                    selectedCard.DOScale(Vector3.one * 1.4f, 0.2f);
                    selectedCard.DOMove(selectedCardOriginPos + offset, 0.2f);
                    isCardExpanded = true;
                }
            }
            else if (selectedCard != null)
            {
                ResetCardSelection();
            }
        }
    }

    // 원래 크기, 상태로 복귀 처리 + 강조 표시 해제하는 함수
    public void ResetCardSelection()
    {
        // 최근 강조 표시된 카드 있다면? -> 강조 표시 해제
        if (lastHovered != null)
        {
            lastHovered.SetHighlight(false);
            lastHovered = null;
        }

        // 선택된 카드가 있다면? -> 원래 크기, 위치로 복귀 처리
        if (selectedCard != null)
        {
            selectedCard.DOScale(Vector3.one, 0.2f);
            selectedCard.DOMove(selectedCardOriginPos, 0.2f);
            selectedCard = null;
            isDragging = false;
            isCardExpanded = false;
        }
    }
}
