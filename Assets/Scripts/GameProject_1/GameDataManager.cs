using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
        GameUtil.LoadFullData();
    }
    [Serializable]
    private class SerializationWrapper<T>
    {
        public List<T> data;
    }

    public Dictionary<string, CharacterData> CharacterDataList { get; private set; } = new Dictionary<string, CharacterData>();
    public Dictionary<string, CardData> CardDataList { get; private set; } = new Dictionary<string, CardData>();

    private Dictionary<string, T> LoadData<T>(string jsonPath) where T : GameDataBase
    {
        if(!File.Exists(jsonPath))
        {
            Debug.LogError($"[Error] 파일을 찾을 수 없습니다: {jsonPath}");
            return new Dictionary<string, T>();
        }

        try
        {
            string jsonString = File.ReadAllText(jsonPath);
            string wrappedJson = "{\"data\":" + jsonString + "}";
            SerializationWrapper<T> wrapper = JsonUtility.FromJson<SerializationWrapper<T>>(wrappedJson);

            if(wrapper != null && wrapper.data != null)
            {
                Debug.Log($"{typeof(T).Name} 데이터를 {wrapper.data.Count}개 로드했습니다.");
                return wrapper.data.ToDictionary(data => data.Id);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[{typeof(T).Name} Json 로드 오류] {ex.Message}");
        }

        return new Dictionary<string, T>();
    }

    public void LoadCharacterData(string jsonPath)
    {
        CharacterDataList = LoadData<CharacterData>(jsonPath);
    }
    public void LoadCardData(string jsonPath)
    {
        CardDataList = LoadData<CardData>(jsonPath);
    }

    public CharacterData GetCharacterData(string id)
    {
        if (CharacterDataList == null || string.IsNullOrEmpty(id)) return null;

        return CharacterDataList.TryGetValue(id, out var data) ? data : null;
    }
    public CardData GetCardData(string id)
    {
        if (CardDataList == null || string.IsNullOrEmpty(id)) return null;

        return CardDataList.TryGetValue(id, out var data) ? data : null;
    }
}