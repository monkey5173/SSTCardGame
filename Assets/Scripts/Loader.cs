using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class CardInfo
{
    public string cardName;
    public string description;
    public string frontImagePath;
    public string backImagePath;
    public int cost;
    public int damage;
    public int health;
}

public class CardCollect
{
    public List<CardInfo> cards = new List<CardInfo>();
}

public class Loader : MonoBehaviour
{
    public string[] lines;

    CardCollect cardCollet = new CardCollect();
    
    private void Start()
    {
        cardCollet.cards = new List<CardInfo>();
        LoadCsv();
    }

    public void LoadCsv()
    {
        lines = File.ReadAllLines("Assets/Resources/CardData.csv");

        string[] header = lines[0].Split(',');
        string[] values;

        for(int i = 1; i < lines.Length; i++)
        {
            CardInfo cardInfo = new CardInfo();
            values = lines[i].Split(",");

            cardInfo.cardName = values[0];
            cardInfo.description = values[1];
            cardInfo.frontImagePath = values[2];
            cardInfo.backImagePath = values[3];
            cardInfo.cost = int.Parse(values[4]);
            cardInfo.damage = int.Parse(values[5]);
            cardInfo.health = int.Parse(values[6]);

            cardCollet.cards.Add(cardInfo);
        }
    }
}
