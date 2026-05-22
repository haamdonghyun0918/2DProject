using UnityEngine;
using UnityEngine.UI;

public class GameStage : UiBase
{
    [SerializeField] private Transform spawn_Character;
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private Image image_Map;

    [SerializeField] private Transform[] spawn_Monsters;
    [SerializeField] private GameObject monsterPrefab;
    private void OnEnable()
    {
        StartStage("Map_01");
    }
    private void SpawnSelectedCharacter()
    {
        foreach (Transform child in spawn_Character) Destroy(child.gameObject);

        string charId = UiManager.Instance.SelectedCharacterId;
        CharacterData charData = GameDataManager.Instance.GetCharacterData(charId);

        if (charData == null) return;
        
        GameObject charObj = Instantiate(characterPrefab, spawn_Character);
        GameCharacter characterComp = charObj.GetComponent<GameCharacter>();

        if (characterComp != null)
        {
            characterComp.SetUp(charData);
        }
        else
        {
            Debug.LogError("캐릭터 프리팹이 확인되지 않았습니다.");
        }
    }
    public void StartStage(string mapId)
    {
        MapData mapData = GameDataManager.Instance.GetMapData(mapId);
        if (mapData == null) return;

        image_Map.sprite = Resources.Load<Sprite>(mapData.MapImageAddress);

        SpawnSelectedCharacter();

        SpawnMonsters(mapData.Monster);
    }
    
    private void SpawnMonsters(string[] monsterIds)
    {
        foreach (var spawn in spawn_Monsters)
        {
            foreach (Transform child in spawn) Destroy(child.gameObject);
        }
        if (monsterIds == null || monsterIds.Length == 0) return;

        int count = monsterIds.Length;

        switch (count)
        {
            case 1:
                SpawnSingleMonster(monsterIds[0], spawn_Monsters[1]);
                break;
            case 2:
                SpawnSingleMonster(monsterIds[0], spawn_Monsters[0]);
                SpawnSingleMonster(monsterIds[1], spawn_Monsters[2]);
                break;
            case 3:
                SpawnSingleMonster(monsterIds[0], spawn_Monsters[0]);
                SpawnSingleMonster(monsterIds[1], spawn_Monsters[1]);
                SpawnSingleMonster(monsterIds[2], spawn_Monsters[2]);
                break;
        }
    }
    private void SpawnSingleMonster(string monsterId, Transform spawnPoint)
    {
        MonsterData mData = GameDataManager.Instance.GetMonsterData(monsterId);
        if (mData == null) return;

        GameObject mObj = Instantiate(monsterPrefab, spawnPoint);
        GameMonster mComp = mObj.GetComponent<GameMonster>();
        if (mComp != null) mComp.SetUp(mData);
    }
}