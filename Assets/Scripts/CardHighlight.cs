using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHighlight : MonoBehaviour
{
    [SerializeField] private Outline outline;   //frontCard�� Outline ������Ʈ ����

    private void Awake()
    {
        if (outline != null)
        {
            outline.enabled = false;    // ���� �� ����
        }
    }

    public void SetHighlight(bool on)
    {
        if (outline != null)
        {
            outline.enabled = on;
        }
    }
}
