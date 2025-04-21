using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

// JSON ������ �ҷ��� �ش� ������ ��ũ���ͺ� ������Ʈ�� �־���
// ��ũ���ͺ� ������Ʈ�� �������ִ� ������ ��ũ��Ʈ
public class ScriptableObjectCreator : EditorWindow
{
    private string jsonFilePath = "Assets/Resources/CardDataJson.json";
    private string assetFolderPath = "Assets/Resources/";

    private string assetFolderName = "CardAssets";

    // ����Ƽ �޴����� ������ ����
    [MenuItem("Tools/ScriptableObject Creator")]
    public static void ShowWindow()
    {
        GetWindow<ScriptableObjectCreator>("Create ScriptableObj");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create ScriptableObj", EditorStyles.boldLabel);

        // �����Ϳ��� ��θ� ���� ������ �� �ְ� �ؽ�Ʈ �ʵ� ����
        jsonFilePath = EditorGUILayout.TextField("Json File Path", jsonFilePath);
        assetFolderPath = EditorGUILayout.TextField("Asset Folder Path", assetFolderPath);
        assetFolderName = EditorGUILayout.TextField("Asset Folder Name", assetFolderName);

        if (GUILayout.Button("Create"))
        {
            CreateOrUpdateCardAssets();
        }
    }

    // JSON ������ �ҷ��ͼ� ����Ʈ�� ��� �� ������ ��ũ���ͺ� ������Ʈ�� ��� ���
    void CreateOrUpdateCardAssets()
    {
        if (!File.Exists(jsonFilePath))
        {
            UnityEngine.Debug.LogError("JSON ������ �������� �ʽ��ϴ�");
            return;
        }

        string jsonText = File.ReadAllText(jsonFilePath);
        CardCollection cardCollection = JsonUtility.FromJson<CardCollection>(jsonText);

        if(cardCollection == null || cardCollection.cards == null)
        {
            UnityEngine.Debug.LogError("JSON ���Ͽ� ����� �����Ͱ� �����ϴ�");
            return;
        }

        if (!assetFolderPath.EndsWith("/"))
        {
            assetFolderPath += "/";
        }

        string fullAssetFolderPath = assetFolderPath + assetFolderName;

        // ������ ��ο� �ش� ������ ���ٸ�?
        if(!AssetDatabase.IsValidFolder(fullAssetFolderPath))
        {
            // ��ο� ������ �����Ѵ�
            AssetDatabase.CreateFolder(assetFolderPath.TrimEnd('/'), assetFolderName);
        }

        foreach(Card card in cardCollection.cards)
        {
            string fixedCardName = card.cardName.Trim();

            string assetPath = $"{fullAssetFolderPath}/{fixedCardName}.asset";

            // ���� ������ �ִ��� Ȯ��
            CardSobj cardAsset = AssetDatabase.LoadAssetAtPath<CardSobj>(assetPath);

            if (cardAsset != null)
            {
                // ���� ������ �ִٸ� ������Ʈ
                cardAsset.cardName = card.cardName;
                cardAsset.description = card.description;
                cardAsset.frontImagePath = card.frontImagePath;
                cardAsset.backImagePath = card.backImagePath;
                cardAsset.cost = card.cost;
                cardAsset.damage = card.damage;
                cardAsset.health = card.health;

                // ������Ʈ �� ���� ���忡 �ݿ���
                EditorUtility.SetDirty(cardAsset);                
            }
            else
            {
                // ���� ������ ������ ��ũ���ͺ� ������Ʈ�� ���� ����
                cardAsset = ScriptableObject.CreateInstance<CardSobj>();
                cardAsset.cardName = card.cardName;
                cardAsset.description = card.description;
                cardAsset.frontImagePath = card.frontImagePath;
                cardAsset.backImagePath = card.backImagePath;
                cardAsset.cost = card.cost;
                cardAsset.damage = card.damage;
                cardAsset.health = card.health;

                // ������ �̸��� ��������
                cardAsset.name = fixedCardName;

                // ��ũ���ͺ� ������Ʈ ������ ��ο� ����
                AssetDatabase.CreateAsset(cardAsset, assetPath);
            }
        }

        // �����ϰ� ���ΰ�ħ
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        UnityEngine.Debug.Log("��� ī�� ������ ���� �� ������Ʈ �Ǿ����ϴ�");
    }
}
