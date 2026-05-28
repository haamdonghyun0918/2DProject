using System.Collections.Generic;
using Unity.VisualScripting;
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
    //카드를 다시 뽑을 때를 대비하여 현재 캐릭터 데이터를 담는 변수
    private CharacterData currentCharacterData;

    private void OnEnable()
    {
        if (StageManager.Instance != null)
        {
            //스테이지 매니저에서 준 데이터를 바탕으로 몬스터랑 맵 데이터를 가져옴
            StageManager.Instance.SetUpStage(this);
        }
    }
    public void SetMapImage(string mapImagePath)
    {
        image_Map.sprite = Resources.Load<Sprite>(mapImagePath);
    }
    public GameCharacter SpawnPlayer(CharacterData charData, int savedHp)
    {
        foreach (Transform child in spawn_Character) Destroy(child.gameObject);
        if (charData == null) return null;

        currentCharacterData = charData;

        GameObject charObj = Instantiate(characterPrefab, spawn_Character);
        GameCharacter characterComp = charObj.GetComponent<GameCharacter>();

        if (characterComp != null)
        {
            characterComp.SetUp(charData);
            if (savedHp != -1) characterComp.SetCurrentHp(savedHp);
        }
        return characterComp;
    }
    public List<GameMonster> SpawnMonsters(string[] monsterIds)
    {
        List<GameMonster> spawnedMonsters = new List<GameMonster>();
        foreach(var spawn in spawn_Monsters)
        {
            foreach (Transform child in spawn) Destroy(child.gameObject);
        }
        if (monsterIds == null || monsterIds.Length == 0) return spawnedMonsters;

        int count = monsterIds.Length;
        switch (count)
        {
            case 1://몬스터 1마리만 있는 경우
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

    private GameMonster SpawnSingleMonster(string monsterIds, Transform spawnPoint)
    {
        MonsterData mData = GameDataManager.Instance.GetMonsterData(monsterIds);
        if (mData == null) return null;

        GameObject mObj = Instantiate(monsterPrefab, spawnPoint);
        GameMonster mComp = mObj.GetComponent<GameMonster>();
        if (mComp != null) mComp.SetUp(mData);

        return mComp;
    }

    public void SpawnRandomCards()
    {
        if (cardSpawnPoint == null || currentCharacterData == null || currentCharacterData.Card == null) return;

        foreach (Transform child in cardSpawnPoint) Destroy(child.gameObject);

        for (int i = 0; i < 3; i++)
        {
            RefillUsedCard();
        }
    }

    public void RefillUsedCard()
    {
        if (cardSpawnPoint == null || currentCharacterData == null || currentCharacterData.Card == null) return;

        int randomIndex = Random.Range(0, currentCharacterData.Card.Length);
        string randomCardId = currentCharacterData.Card[randomIndex];
        CardData cardData = GameDataManager.Instance.GetCardData(randomCardId);

        if (cardData != null)
        {
            GameObject instantiatedCard = Instantiate(slotCardPrefab, cardSpawnPoint);
            SlotCardUi slotCardUi = instantiatedCard.GetComponent<SlotCardUi>();
            if (slotCardUi != null) slotCardUi.SetUp(cardData);
        }
    }
}