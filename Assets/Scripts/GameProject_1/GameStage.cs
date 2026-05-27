using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStage : UiBase
{
    //캐릭터 로드
    [SerializeField] private Transform spawn_Character;
    [SerializeField] private GameObject characterPrefab;
    //맵 이미지 로드
    [SerializeField] private Image image_Map;
    //몬스터 로드
    [SerializeField] private Transform[] spawn_Monsters;
    [SerializeField] private GameObject monsterPrefab;
    //카드 로드
    [SerializeField] private Transform cardSpawnPoint;
    [SerializeField] private GameObject slotCardPrefab;
    private void OnEnable()
    {
        int stageNum = StageManager.Instance != null ? StageManager.Instance.currentStageNum : 1;
        string mapId = $"Map_{stageNum:D2}";
        StartStage(mapId);
    }
    private GameCharacter SpawnSelectedCharacter()
    {
        foreach (Transform child in spawn_Character) Destroy(child.gameObject);

        string charId = UiManager.Instance.SelectedCharacterId;
        CharacterData charData = GameDataManager.Instance.GetCharacterData(charId);

        if (charData == null) return null;
        
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
        return characterComp;
    }
    public void StartStage(string mapId)
    {
        MapData mapData = GameDataManager.Instance.GetMapData(mapId);
        if (mapData == null) return;

        image_Map.sprite = Resources.Load<Sprite>(mapData.MapImageAddress);

        GameCharacter player = SpawnSelectedCharacter();

        if (player != null && StageManager.Instance != null && StageManager.Instance.playerSavedHp != -1)
        {
            player.SetCurrentHp(StageManager.Instance.playerSavedHp);
        }

        List<GameMonster> allmonsters = SpawnMonsters(mapData.Monster);
        SpawnRandomCards();

        if (GameManager.Instance != null && player != null)
        {
            GameManager.Instance.StartBattle(player, allmonsters, this);
        }
    }
    public void RefillUsedCard()
    {
        if (cardSpawnPoint == null) return;

        string selectedCharId = UiManager.Instance.SelectedCharacterId;
        if (string.IsNullOrEmpty(selectedCharId)) return;

        CharacterData characterData = GameDataManager.Instance.GetCharacterData(selectedCharId);
        if (characterData == null || characterData.Card == null || characterData.Card.Length == 0) return;

        int randomIndex = Random.Range(0, characterData.Card.Length);
        string randomCardId = characterData.Card[randomIndex];

        CardData cardData = GameDataManager.Instance.GetCardData(randomCardId);
        if (cardData != null)
        {
            GameObject instantiatedCard = Instantiate(slotCardPrefab, cardSpawnPoint);
            SlotCardUi slotCardUi = instantiatedCard.GetComponent<SlotCardUi>();

            if (slotCardPrefab != null)
            {
                slotCardUi.SetUp(cardData);
            }
        }
    }
    
    private List<GameMonster> SpawnMonsters(string[] monsterIds)
    {
        List<GameMonster> spawnedMonsters = new List<GameMonster>();
        foreach (var spawn in spawn_Monsters)
        {
            foreach (Transform child in spawn) Destroy(child.gameObject);
        }
        if (monsterIds == null || monsterIds.Length == 0) return spawnedMonsters;

        int count = monsterIds.Length;

        switch (count)
        {
            case 1:
                spawnedMonsters.Add(SpawnSingleMonster(monsterIds[0], spawn_Monsters[1]));
                break;
            case 2:
                spawnedMonsters.Add(SpawnSingleMonster(monsterIds[0], spawn_Monsters[0]));
                spawnedMonsters.Add(SpawnSingleMonster(monsterIds[1], spawn_Monsters[2]));
                break;
            case 3:
                spawnedMonsters.Add(SpawnSingleMonster(monsterIds[0], spawn_Monsters[0]));
                spawnedMonsters.Add(SpawnSingleMonster(monsterIds[1], spawn_Monsters[1]));
                spawnedMonsters.Add(SpawnSingleMonster(monsterIds[2], spawn_Monsters[2]));
                break;
        }

        spawnedMonsters.RemoveAll(m => m == null);
        return spawnedMonsters;
    }
    private GameMonster SpawnSingleMonster(string monsterId, Transform spawnPoint)
    {
        MonsterData mData = GameDataManager.Instance.GetMonsterData(monsterId);
        if (mData == null) return null;

        GameObject mObj = Instantiate(monsterPrefab, spawnPoint);
        GameMonster mComp = mObj.GetComponent<GameMonster>();
        if (mComp != null) mComp.SetUp(mData);

        return mComp;
    }
    private void SpawnRandomCards()
    {
        if (cardSpawnPoint == null) return;

        foreach (Transform child in cardSpawnPoint)
        {
            Destroy(child.gameObject);
        }
        string selectedCharId = UiManager.Instance.SelectedCharacterId;
        if (string.IsNullOrEmpty(selectedCharId)) return;

        CharacterData characterData = GameDataManager.Instance.GetCharacterData(selectedCharId);
        if (characterData == null || characterData.Card == null || characterData.Card.Length == 0) return;

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, characterData.Card.Length);
            string randomCardId = characterData.Card[randomIndex];

            CardData cardData = GameDataManager.Instance.GetCardData(randomCardId);
            if (cardData != null)
            {
                GameObject instantiatedCard = Instantiate(slotCardPrefab, cardSpawnPoint);
                SlotCardUi slotCardUi = instantiatedCard.GetComponent<SlotCardUi>();

                if (slotCardUi != null)
                {
                    slotCardUi.SetUp(cardData);
                }
            }
        }
    }
}