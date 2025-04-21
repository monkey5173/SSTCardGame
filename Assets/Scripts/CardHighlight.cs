using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHighlight : MonoBehaviour
{
    [SerializeField] Renderer frontRender;

    private MaterialPropertyBlock mpb;

    private static readonly int emissionColorID = Shader.PropertyToID("_EmissionColor");

    private Color highlightColor = Color.green;
    private Color defaultColor = Color.black;

    private void Awake()
    {
        mpb = new MaterialPropertyBlock();
        SetHighlight(false);
    }

    public void SetHighlight(bool on)
    {
        frontRender.GetPropertyBlock(mpb);
        mpb.SetColor(emissionColorID, on ? highlightColor : defaultColor);
        frontRender.SetPropertyBlock(mpb);
    }
}
