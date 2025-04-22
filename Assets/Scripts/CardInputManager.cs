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
                if(lastHovered != null && lastHovered != cardHighlight)
                {
                    lastHovered.SetHighlight(false);
                }

                // 현재 카드 하이라이트 켜기
                cardHighlight.SetHighlight(true);
                lastHovered = cardHighlight;

                // 마우스 왼쪽 버튼 클릭했을 경우 -> 카드 선택 처리
                if(Input.GetMouseButtonDown(0))
                {
                    // 이미 선택된 카드가 다른 카드였다면 -> 원래 상태로 복귀 처리
                    if (selectedCard != hit.transform)
                    {
                        // 이전에 선택된 카드가 있었다면? -> 원래 상태로 복귀
                        if(selectedCard != null)
                        {
                            selectedCard.DOScale(Vector3.one, 0.2f);    // 크기 복귀
                            selectedCard.DOLocalMove(selectedCardOriginPos, 0.2f);   // 위치 복귀
                        }

                        // 새 카드 선택
                        selectedCard = hit.transform;

                        // 선택된 카드의 원래 상태 저장 (한 번만)
                        selectedCardOriginScale = selectedCard.transform.localScale;
                        selectedCardOriginPos = selectedCard.transform.localPosition;

                        // 선택된 카드 확대, Y축 살짝 위로 띄움
                        Vector3 offset = new Vector3(4.5f, 3f, 1.5f);
                        selectedCard.DOScale(Vector3.one * 1.4f, 0.2f);
                        selectedCard.DOLocalMove(selectedCardOriginPos + offset, 0.2f);
                    }

                }
            }
            else
            {
                // 선택된 카드가 있는 상태에서
                // 카드가 아닌 다른 Collider에 마우스 왼쪽 클릭 했을 때
                if (Input.GetMouseButtonDown(0) && selectedCard != null)
                {
                    // 선택 카드 원래 상태로 복귀
                    selectedCard.DOScale(Vector3.one, 0.2f);
                    selectedCard.DOLocalMove(selectedCardOriginPos, 0.2f);
                    selectedCard = null;
                }
            }
        }
        else
        {
            // 마우스가 어떤 Collider와도 충돌하지 않았을 때 (빈 공간 위에 있을 때)

            // 만약 하이라이트 되어있던 카드가 있다면? -> 하이라이트 끄기
            if (lastHovered != null)
            {
                lastHovered.SetHighlight(false);
                lastHovered = null;
            }

            // 빈 공간 클릭 시 -> 선택된 카드가 있다면? 원래 상태로 복귀
            if(Input.GetMouseButtonDown(0) && selectedCard != null)
            {
                selectedCard.DOScale(Vector3.one, 0.2f);
                selectedCard.DOLocalMove(selectedCardOriginPos, 0.2f);
                selectedCard = null;
            }
        }
    }
}
