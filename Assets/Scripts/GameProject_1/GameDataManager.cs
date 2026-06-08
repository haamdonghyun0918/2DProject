using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    //GameDataManager를 싱글톤 패턴으로 작성 => 이 GameDataManager는 게임 내에서 하나여야만 하기에 싱글턴을 사용
    public static GameDataManager Instance { get; set; }

    private void Awake()
    {
        if (Instance == null) Instance = this; // 인스턴스가 비어있다면, 현재 자기 자신을 할당한다.
        else Destroy(gameObject); //이미 존재하면 중복의 위험이 있으므로 파괴한다.
    }
    //JsonUtility의 한계를 극복하기 위한 클래스(Wrapper) => JSON의 최상위 데이터가 배열인 [..]이 부분을 읽지 못하므로
    // {data: [...]} 형태로 한 겹 감싸서 읽어오기 위한 눈속임이다.
    [Serializable]
    private class SerializationWrapper<T>
    {
        public List<T> data;
    }

    // 불러온 데이터들을 메모리에 저장해두는 자료구조임(딕셔너리) => 딕셔너리인 이유는 검색속도가 월등히 빠르기에
    // GameData에서 공통으로 갖고 있는 ID가 Key값으로 되어 있기에 특정 데이터를 찾을 때의 속도가 매우 빠르다.
    public Dictionary<string, CharacterData> CharacterDataList { get; private set; } = new Dictionary<string, CharacterData>();
    public Dictionary<string, CardData> CardDataList { get; private set; } = new Dictionary<string, CardData>();
    public Dictionary<string, MonsterData> MonsterDataList { get; private set; } = new Dictionary<string, MonsterData>();
    public Dictionary<string, MapData> MapDataList { get; private set; } = new Dictionary<string, MapData>();

    public Dictionary<string, ExampleData> ExampleDataList { get; private set; } = new Dictionary<string, ExampleData>();

    // 여러 종류의 데이터를 가져오는 중복 코드를 줄이기 위한, Generic데이터 로더이다. where T : GameDataBase는 가져올 데이터 타입이 반드시 GameDataBase를 상속받은 클래스여야 한다로 안정장치의 역할을 한다
    // GameDataBase가 아닌 것은 무시
    private Dictionary<string, T> LoadData<T>(string resourcePath) where T : GameDataBase
    {
        // Resources 폴더 안에 있는 Json 파일을 텍스트 형태(TextAsset)로 읽어옵니다.
        TextAsset jsonAsset = Resources.Load<TextAsset>(resourcePath);

        if (jsonAsset == null)
        {
            Debug.LogError($"[Error] Resources 폴더에서 파일을 찾을 수 없습니다: {resourcePath}");
            return new Dictionary<string, T>();
        }

        try
        {
            // 읽어온 JSON문자열을 앞서 만든 SerializationWrapper형태로 감싼 {"data": ...}뒤, C# 객체로 변환시킨다.
            string jsonString = jsonAsset.text;
            string wrappedJson = "{\"data\":" + jsonString + "}";
            SerializationWrapper<T> wrapper = JsonUtility.FromJson<SerializationWrapper<T>>(wrappedJson);

            if (wrapper != null && wrapper.data != null)
            {
                Debug.Log($"{typeof(T).Name} 데이터를 {wrapper.data.Count}개 로드했습니다.");
                return wrapper.data.ToDictionary(data => data.Id); // List형태로 파싱된 데이터를 ToDictionary를 사용하여 고유 ID를 Key로 가지는 Dictionary로 변환하여 사용합니다
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[{typeof(T).Name} Json 로드 오류] {ex.Message}");
        }

        return new Dictionary<string, T>();
    }

    //Load매세더는 LoadData를 사용해서 특정 데이터들을 Dictionary에 채워 넣습니다.
    public void LoadCharacterData(string jsonPath)
    {
        CharacterDataList = LoadData<CharacterData>(jsonPath);
    }
    public void LoadCardData(string jsonPath)
    {
        CardDataList = LoadData<CardData>(jsonPath);
    }
    public void LoadMonsterData(string jsonPath)
    {
        MonsterDataList = LoadData<MonsterData>(jsonPath);
    }
    public void LoadMapData(string jsonPath)
    {
        MapDataList = LoadData<MapData>(jsonPath);
    }
    public void LoadExampleData(string jsonPath)
    {
        ExampleDataList = LoadData<ExampleData>(jsonPath);
    }
    // Get 매서드를 사용하여 Dictionary에서 데이터를 찾아 변환하는 내용 => TryGetValue를 사용하여 있으면 가져오고, 없으면 null을 반환하도록 처리
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
    public MonsterData GetMonsterData(string id)
    {
        if (MonsterDataList == null || string.IsNullOrEmpty(id)) return null;

        return MonsterDataList.TryGetValue(id, out var data) ? data : null;
    }
    public MapData GetMapData(string id)
    {
        if (MapDataList == null || string.IsNullOrEmpty(id)) return null;

        return MapDataList.TryGetValue(id, out var data) ? data : null;
    }
    public ExampleData GetExampleData(string id)
    {
        if (ExampleDataList == null || string.IsNullOrEmpty(id)) return null;

        return ExampleDataList.TryGetValue(id,out var data) ? data : null;
    }

    // 로딩화면에서 데이터들을 가져오는 과 연동하기 위한 코드
    public IEnumerator CoLoadAllData(Action<float> onProgress, Action onComplete)
    {
        string charPath = GameUtil.GetFullDataPath("Character");
        string cardPath = GameUtil.GetFullDataPath("Card");
        string monsPath = GameUtil.GetFullDataPath("Monster");
        string mapPath = GameUtil.GetFullDataPath("Map");
        string examplePath = GameUtil.GetFullDataPath("ExampleData");

        LoadCharacterData(charPath);
        onProgress?.Invoke(0.20f);
        yield return null;

        LoadCardData(cardPath);
        onProgress?.Invoke(0.40f);
        yield return null;

        LoadMonsterData(monsPath);
        onProgress?.Invoke(0.60f);
        yield return null;

        LoadMapData(mapPath);
        onProgress?.Invoke(0.80f);

        LoadExampleData(examplePath);
        onProgress?.Invoke(1.00f);

        yield return new WaitForSeconds(0.2f);

        onComplete?.Invoke();
    }

    // 데이터 초기화 코드로 게임중 다시하기를 눌렀을 때, 저장되었던, 카드들의 목록을 원래 상태로 되돌리는 매서드
    public void ResetCharacterData()
    {
        string charPath = GameUtil.GetFullDataPath("Character");
        LoadCharacterData(charPath);
        Debug.Log("캐릭터 데이터가 초기화 되었습니다");
    }
}