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

        SpawnRandomCards();
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