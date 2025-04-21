using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDraw : MonoBehaviour
{
    public void AnimateDraw(Vector3 targetPos, float delay = 0f)
    {
        transform.localScale = Vector3.zero;
        transform.position = new Vector3(3, 2, 0);

        transform.DOMove(targetPos, 0.5f).SetDelay(delay).SetEase(Ease.OutQuad);
        transform.DOScale(Vector3.one, 0.5f).SetDelay(delay).SetEase(Ease.OutBack);
    }
}
