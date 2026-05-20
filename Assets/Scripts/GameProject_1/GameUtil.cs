using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class GameUtil
{
    public static void LoadFullData()
    {
        GameDataManager.Instance.LoadCharacterData(GetFullDataPath("Character"));
        GameDataManager.Instance.LoadCardData(GetFullDataPath("Card"));
    }

    public static string GetFullDataPath(string dataTableName)
    {
        if(string.IsNullOrEmpty(dataTableName))
        {
            Debug.Log("테이블 이름이 올바르지 않습니다!");
            return string.Empty;
        }
        string relativePath = $"Assets/Resources/Jsonoutput/{dataTableName}.json";
        string fullPath = Path.GetFullPath(relativePath);
        return fullPath;
    }
}