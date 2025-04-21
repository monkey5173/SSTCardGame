using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor;
using System.Diagnostics;


// CSV���� �ϳ��� ī�� ������ ���� Ŭ����
[Serializable]
public class Card
{
    public string cardName;
    public string description;
    public string frontImagePath;
    public string backImagePath;
    public int cost;
    public int damage;
    public int health;
}

// �������� ī�� �����͸� ���� ���� Ŭ���� ( JsonUtility �迭 ��ü�� ��ȯ�ϱ� ���� )
[Serializable]
public class CardCollection
{
    public List<Card> cards;
}

// CSV ������ JSON ���Ϸ� ��ȯ���ִ� ������ ��ũ��Ʈ â
// Unity �޴��� Tools/CSV TO JSON Converter �׸����� ��Ÿ��
public class CsvToJsonConverter : EditorWindow
{
    // CSV ���� ��� ( Assets/Resources ���� ��� )
    private string csvFilePath = "Assets/Resources/CardData.csv";

    // JSON ������ ������ ���
    private string jsonFilePath = "Assets/Resources/CardDataJson.json";

    // ������ â�� ���� ���� �޴� �׸� �߰�
    [MenuItem("Tools/CSV TO JSON Converter")]
    public static void ShowWindow()
    {
        GetWindow<CsvToJsonConverter>("CSV TO JSON Converter");
    }

    private void OnGUI()
    {
        GUILayout.Label("CSV TO JSON Converter", EditorStyles.boldLabel);

        // CSV ���� ��ο� JSON ���� ���� ��θ� �����Ϳ��� ������ �� �ֵ��� �ؽ�Ʈ �ʵ� ����
        csvFilePath = EditorGUILayout.TextField("CSV File Path", csvFilePath);
        jsonFilePath = EditorGUILayout.TextField("JSON File Path", jsonFilePath);

        // ��ȯ ��ư�� ������ ConvertCSVtoJSON �޼��� ����
        if (GUILayout.Button("Convert"))
        {
            ConvertCSVtoJSON();
        }
    }

    // CSV ������ �о� JSON ���Ϸ� ��ȯ �� �����ϴ� ����� �����ϴ� �޼���
    void ConvertCSVtoJSON()
    {
        // CSV ���� ���� ���θ� üũ
        if (!File.Exists(csvFilePath))
        {
            UnityEngine.Debug.LogError("CSV ������ ã�� �� �����ϴ�. " + csvFilePath);
            return;
        }

        // CSV ������ ��� ���� �о� �迭�� ����
        string[] lines = File.ReadAllLines(csvFilePath);

        // ���� ���̰� 1�� ���ٸ� ��ǻ� ��� ������ �־ ���� ����
        if(lines.Length <= 1)
        {
            UnityEngine.Debug.LogError("CSV ���Ͽ� �����Ͱ� ������� �ʽ��ϴ�.");
            return;
        }

        // ù��° ���� ����� ��� ( �� ���� �̸� )
        string[] headers = lines[0].Split(',');

        // CardCollection ��ü ����
        CardCollection cardCollection = new CardCollection();
        cardCollection.cards = new List<Card>();

        // �ι�° �ٺ��� �� ī�� �����͸� �Ľ�
        for(int i = 1; i < lines.Length; i++)
        {
            // CSV �� ���� �޸��� �и��ؼ� �� �迭�� �����
            string[] values = lines[i].Split(',');

            // ������ ��� CSV ���Ͽ� �� ���� �� ��츦 üũ
            if(values.Length < headers.Length)
            {
                UnityEngine.Debug.LogError("CSV ���Ͽ� ������ ���� �ֽ��ϴ�.");
                continue;
            }

            Card newCard = new Card();

            for(int j = 0; j < headers.Length; j++)
            {
                string header = headers[j].Trim();
                string value = values[j].Trim();

                switch (header)
                {
                    case "cardName":
                        newCard.cardName = value;
                        break;
                    case "description":
                        newCard.description = value;
                        break;
                    case "frontImagePath":
                        newCard.frontImagePath = value;
                        break;
                    case "backImagePath":
                        newCard.backImagePath = value;
                        break;
                    case "cost":
                        int.TryParse(value, out newCard.cost);
                        break;
                    case "damage":
                        int.TryParse(value, out newCard.damage);
                        break;
                    case "health":
                        int.TryParse(value, out newCard.health);
                        break;
                }                
            }

            cardCollection.cards.Add(newCard);
        }

        string jsonFile = JsonUtility.ToJson(cardCollection, true);
        File.WriteAllText(jsonFilePath, jsonFile);
        UnityEngine.Debug.Log("CSV�� JSON���� ��ȯ �Ϸ�. ���� ��ġ : " +  jsonFilePath);

        AssetDatabase.Refresh();
    }
}
