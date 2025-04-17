using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor;
using System.Diagnostics;


// CSV에서 하나의 카드 정보를 담을 클래스
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

// 여러개의 카드 데이터를 담을 래퍼 클래스 ( JsonUtility 배열 전체를 변환하기 위해 )
[Serializable]
public class CardCollection
{
    public List<Card> cards;
}

// CSV 파일을 JSON 파일로 변환해주는 에디터 스크립트 창
// Unity 메뉴에 Tools/CSV TO JSON Converter 항목으로 나타남
public class CsvToJsonConverter : EditorWindow
{
    // CSV 파일 경로 ( Assets/Resources 폴더 사용 )
    private string csvFilePath = "Assets/Resources/CardData.csv";

    // JSON 파일을 저장할 경로
    private string jsonFilePath = "Assets/Resources/CardDataJson.json";

    // 에디터 창을 열기 위한 메뉴 항목 추가
    [MenuItem("Tools/CSV TO JSON Converter")]
    public static void ShowWindow()
    {
        GetWindow<CsvToJsonConverter>("CSV TO JSON Converter");
    }

    private void OnGUI()
    {
        GUILayout.Label("CSV TO JSON Converter", EditorStyles.boldLabel);

        // CSV 파일 경로와 JSON 파일 저장 경로를 에디터에서 수정할 수 있도록 텍스트 필드 제공
        csvFilePath = EditorGUILayout.TextField("CSV File Path", csvFilePath);
        jsonFilePath = EditorGUILayout.TextField("JSON File Path", jsonFilePath);

        // 변환 버튼을 누르면 ConvertCSVtoJSON 메서드 실행
        if (GUILayout.Button("Convert"))
        {
            ConvertCSVtoJSON();
        }
    }

    // CSV 파일을 읽어 JSON 파일로 변환 후 저장하는 기능을 수행하는 메서드
    void ConvertCSVtoJSON()
    {
        // CSV 파일 존재 여부를 체크
        if (!File.Exists(csvFilePath))
        {
            UnityEngine.Debug.LogError("CSV 파일을 찾을 수 없습니다. " + csvFilePath);
            return;
        }

        // CSV 파일의 모든 줄을 읽어 배열에 저장
        string[] lines = File.ReadAllLines(csvFilePath);

        // 만약 길이가 1과 같다면 사실상 헤더 정보만 있어서 쓸모가 없음
        if(lines.Length <= 1)
        {
            UnityEngine.Debug.LogError("CSV 파일에 데이터가 충분하지 않습니다.");
            return;
        }

        // 첫번째 줄은 헤더로 사용 ( 각 열의 이름 )
        string[] headers = lines[0].Split(',');

        // CardCollection 객체 생성
        CardCollection cardCollection = new CardCollection();
        cardCollection.cards = new List<Card>();

        // 두번째 줄부터 각 카드 데이터를 파싱
        for(int i = 1; i < lines.Length; i++)
        {
            // CSV 각 줄을 콤마로 분리해서 값 배열로 만든다
            string[] values = lines[i].Split(',');

            // 만약을 대비 CSV 파일에 값 누락 될 경우를 체크
            if(values.Length < headers.Length)
            {
                UnityEngine.Debug.LogError("CSV 파일에 누락된 값이 있습니다.");
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
        UnityEngine.Debug.Log("CSV를 JSON으로 변환 완료. 저장 위치 : " +  jsonFilePath);

        AssetDatabase.Refresh();
    }
}
