using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] Renderer frontCard;
    [SerializeField] Renderer backCard;

    [SerializeField] TextMeshPro cardNameText;
    [SerializeField] TextMeshPro cardCostText;
    [SerializeField] TextMeshPro cardDamageText;
    [SerializeField] TextMeshPro cardHealthText;

    public void SetCardData(CardSobj cardData)
    {
        cardNameText.text = cardData.name;
        cardCostText.text = cardData.cost.ToString();
        cardDamageText.text = cardData.damage.ToString();
        cardHealthText.text = cardData.health.ToString();

        Texture2D frontTex = Resources.Load<Texture2D>(cardData.frontImagePath);
        Texture2D backTex = Resources.Load<Texture2D>(cardData.backImagePath);

        MaterialPropertyBlock block = new MaterialPropertyBlock();

        if (frontTex != null)
        {
            frontCard.GetPropertyBlock(block);
            block.SetTexture("_MainTex", frontTex);
            frontCard.SetPropertyBlock(block);
        }

        if (backTex != null)
        {
            backCard.GetPropertyBlock(block);
            block.SetTexture("_MainTex", backTex);
            backCard.SetPropertyBlock(block);
        }
    }
}
