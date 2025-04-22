using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHighlight : MonoBehaviour
{
    [SerializeField] private Outline outline;   //frontCardÀÇ Outline ÄÄÆ÷³ÍÆ® ¿¬°á

    private void Awake()
    {
        if (outline != null)
        {
            outline.enabled = false;    // ½ÃÀÛ ½Ã ²¨Áü
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
