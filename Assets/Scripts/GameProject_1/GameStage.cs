using DG.Tweening;
using System.Collections;
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

    //덱 연출
    [SerializeField] private Image image_Deck;
    [SerializeField] private Transform deckTransform;
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
        for (int i = spawnedMonsters.Count -1; i >= 0; i--)
        {
            if (spawnedMonsters[i] == null)
            {
                spawnedMonsters.RemoveAt(i);
            }
        }
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

        SetUpDeckImage();

        for (int i = 0; i < 3; i++)
        {
            SpawnInitialCard();
        }
    }
    public void SpawnInitialCard()
    {
        int randomIndex = Random.Range(0, currentCharacterData.Card.Length);
        string randomCardId = currentCharacterData.Card[randomIndex];
        CardData cardData = GameDataManager.Instance.GetCardData(randomCardId);

        if (cardData != null)
        {
            GameObject instantiatedCard = Instantiate(slotCardPrefab, cardSpawnPoint);
            SlotCardUi slotCardUi = instantiatedCard.GetComponent<SlotCardUi>();
            if (slotCardUi != null) slotCardUi.SetUp(cardData);

            CanvasGroup cg = instantiatedCard.GetComponent<CanvasGroup>();
            if (cg == null) cg = instantiatedCard.AddComponent<CanvasGroup>();
            cg.alpha = 1f;

            instantiatedCard.transform.localScale = Vector3.one;
        }
    }
    public void SetUpDeckImage()
    {
        if (image_Deck == null || currentCharacterData.Card.Length == 0) return;

        string firstCardId = currentCharacterData.Card[0];
        CardData firstCardData = GameDataManager.Instance.GetCardData(firstCardId);

        if (firstCardData != null)
        {
            Sprite deckSprite = Resources.Load<Sprite>(firstCardData.ImageCardAddress);
            if (deckSprite != null)
            {
                image_Deck.sprite = deckSprite;
            }
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
            //1. 진짜 카드 생성 (Content 안에 넣어서 자리만 차지하게 만듭니다)
            GameObject realCard = Instantiate(slotCardPrefab, cardSpawnPoint);
            SlotCardUi realSlotUi = realCard.GetComponent<SlotCardUi>();
            if (realSlotUi != null) realSlotUi.SetUp(cardData);

            // 진짜 카드의 자체 애니메이션을 끄고, 크기를 0으로 만들어 '투명한 빈자리' 역할만 하게 합니다.
            realCard.transform.DOKill();
            realCard.transform.localScale = Vector3.zero;

            if (deckTransform != null)
            {
                //2. 강제로 레이아웃을 계산하여 진짜 카드가 들어갈 '최종 목적지 좌표'를 알아냅니다.
                Canvas.ForceUpdateCanvases();
                LayoutRebuilder.ForceRebuildLayoutImmediate(cardSpawnPoint.GetComponent<RectTransform>());
                Vector3 targetPos = realCard.transform.position;

                //3. 날아가는 연출 전용 '가짜 카드(더미)' 생성
                //부모를 Content가 아닌 현재 UI창(this.transform)으로 빼서 Layout Group의 간섭을 완벽히 차단합니다!
                GameObject fakeCard = Instantiate(slotCardPrefab, this.transform);
                SlotCardUi fakeSlotUi = fakeCard.GetComponent<SlotCardUi>();
                if (fakeSlotUi != null) fakeSlotUi.SetUp(cardData);

                // 가짜 카드는 상호작용(드래그) 및 자체 애니메이션 끄기
                fakeCard.transform.DOKill();
                CardInteractionHandler fakeInteraction = fakeCard.GetComponent<CardInteractionHandler>();
                if (fakeInteraction != null) fakeInteraction.enabled = false;

                //4. 가짜 카드를 덱 위치에 두고 목적지로 날려보냅니다!
                fakeCard.transform.position = deckTransform.position;
                fakeCard.transform.localScale = Vector3.one * 0.2f; // 덱에서 작게 시작

                fakeCard.transform.DOMove(targetPos, 0.5f).SetEase(Ease.OutCubic);
                fakeCard.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutCubic);
                StartCoroutine(CardDrawCompleteRoutine(fakeCard, realCard));
            }
            else
            {
                // 덱 이미지가 없을 경우를 대비한 안전 장치
                realCard.transform.localScale = Vector3.one;
            }
        }
    }
    private IEnumerator CardDrawCompleteRoutine(GameObject fakeCard, GameObject realCard)
    {
        yield return new WaitForSeconds(0.5f);

        if (fakeCard != null)
        {
            Destroy(fakeCard);
        }

        if (realCard != null)
        {
            realCard.transform.localScale = Vector3.one;
            realCard.transform.DOPunchScale(new Vector3(0.15f, 0.15f, 0.0f), 0.2f);
        }
    }
}