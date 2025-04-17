using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

// JSON 파일을 불러와 해당 값들을 스크립터블 오브젝트에 넣어줌
// 스크립터블 오브젝트를 생성해주는 에디터 스크립트
public class ScriptableObjectCreator : EditorWindow
{
    private string jsonFilePath = "Assets/Resources/CardDataJson.json";
    private string assetFolderPath = "Assets/Resources/";

    private string assetFolderName = "CardAssets";

    // 유니티 메뉴에서 에디터 실행
    [MenuItem("Tools/ScriptableObject Creator")]
    public static void ShowWindow()
    {
        GetWindow<ScriptableObjectCreator>("Create ScriptableObj");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create ScriptableObj", EditorStyles.boldLabel);

        // 에디터에서 경로를 따로 수정할 수 있게 텍스트 필드 만듬
        jsonFilePath = EditorGUILayout.TextField("Json File Path", jsonFilePath);
        assetFolderPath = EditorGUILayout.TextField("Asset Folder Path", assetFolderPath);
        assetFolderName = EditorGUILayout.TextField("Asset Folder Name", assetFolderName);

        if (GUILayout.Button("Create"))
        {
            CreateOrUpdateCardAssets();
        }
    }

    // JSON 파일을 불러와서 리스트에 담고 그 값들을 스크립터블 오브젝트에 담는 기능
    void CreateOrUpdateCardAssets()
    {
        if (!File.Exists(jsonFilePath))
        {
            UnityEngine.Debug.LogError("JSON 파일이 존재하지 않습니다");
            return;
        }

        string jsonText = File.ReadAllText(jsonFilePath);
        CardCollection cardCollection = JsonUtility.FromJson<CardCollection>(jsonText);

        if(cardCollection == null || cardCollection.cards == null)
        {
            UnityEngine.Debug.LogError("JSON 파일에 저장된 데이터가 없습니다");
            return;
        }

        if (!assetFolderPath.EndsWith("/"))
        {
            assetFolderPath += "/";
        }

        string fullAssetFolderPath = assetFolderPath + assetFolderName;

        // 지정한 경로에 해당 폴더가 없다면?
        if(!AssetDatabase.IsValidFolder(fullAssetFolderPath))
        {
            // 경로에 폴더를 생성한다
            AssetDatabase.CreateFolder(assetFolderPath.TrimEnd('/'), assetFolderName);
        }

        foreach(Card card in cardCollection.cards)
        {
            string fixedCardName = card.cardName.Trim();

            string assetPath = $"{fullAssetFolderPath}/{fixedCardName}.asset";

            // 기존 에셋이 있는지 확인
            CardSobj cardAsset = AssetDatabase.LoadAssetAtPath<CardSobj>(assetPath);

            if (cardAsset != null)
            {
                // 기존 에셋이 있다면 업데이트
                cardAsset.cardName = card.cardName;
                cardAsset.description = card.description;
                cardAsset.frontImagePath = card.frontImagePath;
                cardAsset.backImagePath = card.backImagePath;
                cardAsset.cost = card.cost;
                cardAsset.damage = card.damage;
                cardAsset.health = card.health;

                // 업데이트 된 내용 저장에 반영해
                EditorUtility.SetDirty(cardAsset);                
            }
            else
            {
                // 기존 에셋이 없으면 스크립터블 오브젝트를 새로 생성
                cardAsset = ScriptableObject.CreateInstance<CardSobj>();
                cardAsset.cardName = card.cardName;
                cardAsset.description = card.description;
                cardAsset.frontImagePath = card.frontImagePath;
                cardAsset.backImagePath = card.backImagePath;
                cardAsset.cost = card.cost;
                cardAsset.damage = card.damage;
                cardAsset.health = card.health;

                // 에셋의 이름도 지정해줌
                cardAsset.name = fixedCardName;

                // 스크립터블 오브젝트 지정한 경로에 만듬
                AssetDatabase.CreateAsset(cardAsset, assetPath);
            }
        }

        // 저장하고 새로고침
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        UnityEngine.Debug.Log("모든 카드 에셋이 생성 및 업데이트 되었습니다");
    }
}
