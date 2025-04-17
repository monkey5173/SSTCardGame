using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObject/CardObject", order = 1)]
public class CardSobj : ScriptableObject
{
    public string cardName;
    public string description;
    public string frontImagePath;
    public string backImagePath;
    public int cost;
    public int damage;
    public int health;
}
